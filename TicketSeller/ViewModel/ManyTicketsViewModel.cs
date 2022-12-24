using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClosedXML.Excel;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using TicketSeller.DBModel;
using TicketSeller.View;

namespace TicketSeller.ViewModel;

public sealed class ManyTicketsViewModel : INotifyPropertyChanged
{
    private readonly TicketDbContext _dbContext;
    private User _currentUser;
    private EventHandler _nextPage;
    private EventHandler _refreshChanges;
    private readonly ManyTicketsView _manyTicketsView;
    private IEnumerable<Ticket> _ticketsNotUpdate;
    private ObservableCollection<Ticket> _tickets;
    private ObservableCollection<Tag> _tags;
    private bool _isUserAdmin;
    private Tag _selectedTag;

    public ManyTicketsViewModel(TicketDbContext dbContext, User currentUser, EventHandler nextPage,
        ManyTicketsView manyTicketsView)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
        _nextPage = nextPage;
        _manyTicketsView = manyTicketsView;
        IsUserAdmin = CurrentUser.Role is UsersRole.Admin;

        Tags = new ObservableCollection<Tag>(_dbContext.Tags.AsQueryable());
        
        _refreshChanges += RefreshList;
        RefreshList();

        BuyCommand = new ActionCommand(BuyTicket);
        EditCommand = new ActionCommand(EditTicket);
        DeleteCommand = new ActionCommand(DeleteTicket);
        BockedCommand = new ActionCommand(BockTicket);
        NewTicketCommand = new ActionCommand(NewTicket);
        ReportCommand = new ActionCommand(Report);
    }

    private void RefreshList(object sender = null, EventArgs e = null)
    {
        _ticketsNotUpdate = _dbContext.Tickets.AsQueryable();
        Tickets = new ObservableCollection<Ticket>(_ticketsNotUpdate);
    }
    
    private void NewTicket()
    {
        var ticketView = new TicketView { DataContext = new TicketViewModel(_dbContext, _nextPage, _refreshChanges)
        {
            PrevPage = _manyTicketsView,
            Ticket = new Ticket(),
            Tags = Tags
        } };
        
        _nextPage(ticketView, EventArgs.Empty);
    }
    
    private void EditTicket(object obj)
    {
        var ticketView = new TicketView { DataContext = new TicketViewModel(_dbContext, _nextPage, _refreshChanges)
        {
            PrevPage = _manyTicketsView,
            IsChange = true,
            Ticket = _dbContext.Tickets.Find(obj),
            Tags = Tags
        } };
        
        _nextPage(ticketView, EventArgs.Empty);
    }

    private void BuyTicket(object obj)
    {
        var ticket = _dbContext.Tickets.Find(obj);
        if (ticket != null)
        {
            ticket.SoldUser = CurrentUser;
            ticket.IsSold = true;
        }
        _dbContext.SaveChanges();
        RefreshList();

        var rnd = new Random();
        var res = rnd.Next(int.MaxValue - 10000000, int.MaxValue);
        
        var ticketCheck = new CheckPayment { DataContext = new CheckViewModel ("покупку")
        {
            PrevPage = _manyTicketsView,
            PrevPageHandler = _nextPage,
            CheckCode = res,
            Ticket = _dbContext.Tickets.Find(obj)
        } };
        
        _nextPage(ticketCheck, EventArgs.Empty);
    }
    
    private void BockTicket(object obj)
    {
        var ticket = _dbContext.Tickets.Find(obj);
        if (ticket != null)
        {
            ticket.BockedUser = CurrentUser;
            ticket.IsBocked = true;
        }
        _dbContext.SaveChanges();
        RefreshList();
        
        var rnd = new Random();
        var res = rnd.Next(int.MaxValue - 10000000, int.MaxValue);
        
        var ticketCheck = new CheckPayment { DataContext = new CheckViewModel ("бронирование")
        {
            PrevPage = _manyTicketsView,
            PrevPageHandler = _nextPage,
            CheckCode = res,
            Ticket = _dbContext.Tickets.Find(obj)
        } };
        
        _nextPage(ticketCheck, EventArgs.Empty);
    }

    private void DeleteTicket(object obj)
    {
        var ticket = _dbContext.Tickets.Find(obj);
        _dbContext.Tickets.Remove(ticket!);
        _dbContext.SaveChanges();
        RefreshList();
    }

    private void Report()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Xlsx file (*.xlsx)|*.xlsx",
            FileName = "Report.xlsx",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };

        if (saveFileDialog.ShowDialog() is not true) return;
        
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Отчёт о продажах");
        
        var currentRow = 1;
        
        worksheet.Cell(currentRow, 1).Value = "Имя пользователя";
        worksheet.Cell(currentRow, 2).Value = "Купленный товар";
        worksheet.Cell(currentRow, 3).Value = "Бронированный товар";

        foreach (var ticket in Tickets.Where(x => x.IsBocked))
        {
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = ticket.BockedUser.UserName;
            worksheet.Cell(currentRow, 3).Value = ticket.TicketName;
        }
        
        foreach (var ticket in Tickets.Where(x => x.IsSold))
        {
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = ticket.SoldUser.UserName;
            worksheet.Cell(currentRow, 2).Value = ticket.TicketName;
        }
        
        workbook.SaveAs(saveFileDialog.FileName);
    }

    public bool IsUserAdmin
    {
        get => _isUserAdmin;
        set
        {
            _isUserAdmin = value;
            OnPropertyChanged();
        }
    }

    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Ticket> Tickets
    {
        get => _tickets;
        set
        {
            _tickets = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set
        {
            _tags = value;
            OnPropertyChanged();
        }
    }

    public Tag SelectedTag
    {
        get => _selectedTag;
        set
        {
            _selectedTag = value;
            OnPropertyChanged();
            SelectionChanged();
        }
    }

    private void SelectionChanged()
    {
        if (SelectedTag is null)
        {
            Tickets = new ObservableCollection<Ticket>(_ticketsNotUpdate);
            return;
        }

        Tickets = new ObservableCollection<Ticket>(_ticketsNotUpdate.Where(x => x.Tag == SelectedTag));
    }

    public ICommand NewTicketCommand { get; set; }
    
    public ICommand ReportCommand { get; set; }
    
    public ICommand EditCommand { get; set; }
    
    public ICommand DeleteCommand { get; set; }
    
    public ICommand BuyCommand { get; set; }
    
    public ICommand BockedCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
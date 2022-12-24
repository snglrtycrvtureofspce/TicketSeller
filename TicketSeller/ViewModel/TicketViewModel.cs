using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using TicketSeller.DBModel;

namespace TicketSeller.ViewModel;

public sealed class TicketViewModel : INotifyPropertyChanged
{
    private readonly TicketDbContext _dbContext;
    private readonly EventHandler _nextPage;
    private readonly EventHandler _saveHandler;
    private ObservableCollection<Tag> _tags;

    public TicketViewModel(TicketDbContext dbContext, EventHandler nextPage, EventHandler saveHandler)
    {
        _dbContext = dbContext;
        _nextPage = nextPage;
        _saveHandler = saveHandler;

        AddChangeCommand = new ActionCommand(AddChange);
    }

    private void AddChange()
    {
        if (!IsChange)
        {
            _dbContext.Tickets.Add(Ticket);
        }
        
        _dbContext.SaveChanges();
        
        _nextPage?.Invoke(PrevPage, EventArgs.Empty);
        _saveHandler?.Invoke(null, EventArgs.Empty);
    }

    public Page PrevPage { get; set; }

    public ICommand AddChangeCommand { get; set; }

    public bool IsChange { get; set; }
    
    public Ticket Ticket { get; set; }
    
    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set
        {
            _tags = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
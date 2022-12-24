using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors.Core;
using TicketSeller.DBModel;
using System.Windows.Controls;

namespace TicketSeller.ViewModel;

public sealed class CheckViewModel : INotifyPropertyChanged
{
    private Ticket _ticket;
    private string _status;

    public CheckViewModel(string status)
    {
        Status = status;
        PrevButtonCommand = new ActionCommand(Action);
    }

    private void Action()
    {
        PrevPageHandler?.Invoke(PrevPage, EventArgs.Empty);
    }

    public Ticket Ticket
    {
        get => _ticket;
        set
        {
            _ticket = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    public EventHandler PrevPageHandler { get; set; }
    
    public Page PrevPage { get; set; }
    
    public int CheckCode { get; set; }
    
    public ImageSource Barcode { get; set; } 
    
    public ICommand PrevButtonCommand { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
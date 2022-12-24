using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TicketSeller.View;

namespace TicketSeller.ViewModel;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private Page _currentPage;
 
    public MainViewModel(AuthenticationView authenticationView, AuthenticationViewModel authenticationViewModel)
    {
        authenticationViewModel.LoginComplete += AuthenticationViewModelOnLoginComplete;
        authenticationView.DataContext = authenticationViewModel;
        _currentPage = authenticationView;
    }

    private void AuthenticationViewModelOnLoginComplete(object sender, EventArgs e)
    {
        CurrentPage = sender as Page;
    }

    public Page CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
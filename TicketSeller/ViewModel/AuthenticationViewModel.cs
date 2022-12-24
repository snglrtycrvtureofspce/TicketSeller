using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Castle.Core.Internal;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using TicketSeller.DBModel;
using TicketSeller.View;

namespace TicketSeller.ViewModel;

public class AuthenticationViewModel : INotifyPropertyChanged
{
    private const string RandomAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly TicketDbContext _dbContext;
    private bool _isRegistration;
    private string _name;
    private string _password;
    private string _secondPassword;
    private SnackbarMessageQueue _errorMessage;

    public AuthenticationViewModel(TicketDbContext dbContext)
    {
        _dbContext = dbContext;
        LoginCommand = new ActionCommand(Login);
        _errorMessage = new SnackbarMessageQueue();
    }

    public bool IsRegistration
    {
        get => _isRegistration;
        set
        {
            _isRegistration = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string SecondPassword
    {
        get => _secondPassword;
        set
        {
            _secondPassword = value;
            OnPropertyChanged();
        }
    }

    public SnackbarMessageQueue ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; set; }

    private void Login(object obj)
    {
        if (Name.IsNullOrEmpty())
        {
            ErrorMessage?.Enqueue("Имя пусто", null, null, null, false, true,
                TimeSpan.FromSeconds(3));
            return;
        }

        if (Password.IsNullOrEmpty())
        {
            ErrorMessage?.Enqueue("Пароль пуст", null, null, null, false, true,
                TimeSpan.FromSeconds(3));
            return;
        }

        var findUser = _dbContext.Users.FirstOrDefault(x => x.UserName == _name);
        User user;

        if (IsRegistration)
        {
            if (SecondPassword.IsNullOrEmpty())
            {
                ErrorMessage?.Enqueue("Второй пароль пуст", null, null, null, false, true,
                    TimeSpan.FromSeconds(3));
                return;
            }

            if (SecondPassword != Password)
            {
                ErrorMessage?.Enqueue("Введённые пароли не совпадают", null, null, null, false, true,
                    TimeSpan.FromSeconds(3));
                return;
            }

            if (findUser is not null)
            {     
                ErrorMessage?.Enqueue("Пользователь уже существует", null, null, null, false, true,
                    TimeSpan.FromSeconds(3));
                return;
            }

            var rnd = new Random();

            var newUser = new User
            {
                Id = new string(Enumerable.Repeat(RandomAlphabet, 450)
                    .Select(s => s[rnd.Next(s.Length)]).ToArray()),
                UserName = Name,
                Role = UsersRole.User,
                HashPassword = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password))
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            user = newUser;
        }
        else
        {
            if (findUser is null)
            {
                ErrorMessage?.Enqueue("Пользователь не найден, зарегестрируйтесь", null, null, null, false, true,
                    TimeSpan.FromSeconds(3));
                return; 
            }

            if (!MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password)).SequenceEqual(findUser.HashPassword))
            {
                ErrorMessage?.Enqueue("Неправильный пароль", null, null, null, false, true,
                    TimeSpan.FromSeconds(3));
                return; 
            }

            user = findUser;
        }

        var newWindow = new ManyTicketsView();
        newWindow.DataContext = new ManyTicketsViewModel(_dbContext, user, LoginComplete, newWindow);

        LoginComplete?.Invoke(newWindow, EventArgs.Empty);
    }

    public event EventHandler LoginComplete;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
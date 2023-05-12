using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.Counters;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorp.Core.ViewModels.DoctorViewModels;
using ZdravoCorp.Core.ViewModels.NurseViewModel;
using ZdravoCorp.Core.ViewModels.PatientViewModel;
using ZdravoCorp.View.Director;
using ZdravoCorp.View.DoctorView;
using ZdravoCorp.View.NurseView;
using ZdravoCorp.View.PatientV;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private readonly RepositoryManager _repositoryManager;
    private string? _email;
    private string? _password;

    public LoginDialog(RepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
        InitializeComponent();
        DataContext = this;
    }

    public string Email
    {
        get => _email;
        set
        {
            if (value != _email)
            {
                _email = value;
                OnPropertyChanged("EmailBox");
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value != _password)
            {
                _password = value;
                OnPropertyChanged("PasswordBox");
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        var user = GetLoggedUser();
        if (user == null)
            return;
        DialogResult = true;


        switch (user.Type)
        {
            case User.UserType.Director:
                //start director view
                Application.Current.MainWindow = new DirectorWindow
                    { DataContext = new DirectorViewModel(_repositoryManager) };
                ;
                break;
            case User.UserType.Patient:
                //start patient view
                var state = user.UserState;
                var patient = _repositoryManager.PatientRepository.GetPatientByEmail(user.Email);
                var counterDictionary = new CounterDictionary();
                if (counterDictionary.IsForBlock(user.Email))
                {
                    MessageBox.Show("You are blocked", "Error", MessageBoxButton.OK);
                    DialogResult = false;
                }
                else
                {
                    Application.Current.MainWindow = new PatientWindow
                    {
                        DataContext = new PatientViewModel(patient, _repositoryManager)
                    };
                }

                break;
            case User.UserType.Nurse:
                //start nurse view
                Application.Current.MainWindow = new NurseWindow
                    { DataContext = new NurseViewModel(_repositoryManager) };
                break;
            case User.UserType.Doctor:
                //start doctor view
                Application.Current.MainWindow = new DoctorWindow
                    { DataContext = new DoctorViewModel(user, _repositoryManager) };
                break;
        }
    }

    private User? GetLoggedUser()
    {
        if (!_repositoryManager.UserRepository.ValidateEmail(Email))
        {
            MessageBox.Show("Invalid Email", "Error", MessageBoxButton.OK);
            return null;
        }

        var user = _repositoryManager.UserRepository.GetUserByEmail(Email);
        if (user != null && user.ValidatePassword(Password)) return user;
        MessageBox.Show("Invalid Password", "Error", MessageBoxButton.OK);
        return null;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = PasswordBox.Password;
    }
}
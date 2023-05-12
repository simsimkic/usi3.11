using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.ViewModels;

public class PatientsViewModel : ViewModelBase
{
    private readonly Patient _patient;


    public PatientsViewModel(Patient patient)
    {
        _patient = patient;
    }

    public string FirstName => _patient.FirstName;
    public string LastName => _patient.LastName;
    public string Email => _patient.Email;

    public override string ToString()
    {
        return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Email)}: {Email}";
    }
}
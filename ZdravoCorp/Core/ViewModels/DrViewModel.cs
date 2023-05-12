using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.ViewModels;

public class DrViewModel
{
    private readonly Doctor _doctor;

    public DrViewModel(Doctor appointment)
    {
        _doctor = appointment;
    }

    public string DoctorName => _doctor.FullName;
}
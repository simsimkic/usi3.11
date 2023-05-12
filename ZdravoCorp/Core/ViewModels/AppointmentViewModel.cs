using System;
using ZdravoCorp.Core.Models.Appointments;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentViewModel : ViewModelBase
{
    private readonly Appointment _appointment;


    public AppointmentViewModel(Appointment appointment)
    {
        // PatientName = patientFullName;
        _appointment = appointment;
    }

    public int Id => _appointment.Id;
    public string DoctorName => _appointment.Doctor.FullName;

    public string PatientName => _appointment.PatientEmail;
    public DateTime Date => _appointment.Time.start;
    public string Anamnesis => _appointment.Anamnesis.KeyWord;
    public string PatientMail => _appointment.PatientEmail;

    public string Specialization => _appointment.Doctor.Specialization.ToString();
}
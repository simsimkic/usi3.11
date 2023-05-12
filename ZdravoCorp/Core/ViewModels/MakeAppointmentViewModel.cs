using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels;

public class MakeAppointmentViewModel : ViewModelBase
{
    private readonly ObservableCollection<string> _doctors;
    private DateTime _date = DateTime.Now + TimeSpan.FromHours(1);

    private string _doctorName;
    private readonly DoctorRepository _doctorRepository;
    private int _hours;
    private int _minutes;
    private readonly Patient _patient;
    private readonly ScheduleRepository _scheduleRepository;


    public MakeAppointmentViewModel(ScheduleRepository scheduleRepository,
        ObservableCollection<AppointmentViewModel> Appointments, DoctorRepository doctorRepository, Patient patient)
    {
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        _patient = patient;
        _doctors = new ObservableCollection<string>();
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        var doctors = doctorRepository.GetAll();
        foreach (var doctor in doctors) _doctors.Add(doctor.FullName + "-" + doctor.Email);

        CreateAppointmentCommand = new DelegateCommand(o => CreateAppointment(Appointments));
    }

    public IEnumerable<string> AllDoctors => _doctors;

    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }

    public ICommand CreateAppointmentCommand { get; set; }

    public string DoctorName
    {
        get => _doctorName;
        set
        {
            _doctorName = value;
            OnPropertyChanged();
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }

    public int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnPropertyChanged();
        }
    }

    public int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnPropertyChanged();
        }
    }

    private void CreateAppointment(ObservableCollection<AppointmentViewModel> Appointments)
    {
        try
        {
            var h = Hours;
            var m = Minutes;
            var d = Date;
            var dm = DoctorName;

            var start = new DateTime(d.Year, d.Month, d.Day, h, m, 0);
            var end = start.AddMinutes(15);
            var time = new TimeSlot(start, end);

            var tokens = dm.Split("-");
            var mail = tokens[1];
            var doctor = _doctorRepository.GetDoctorByEmail(mail);


            var appointment = _scheduleRepository.CreateAppointment(time, doctor, _patient.Email);
            if (appointment != null)
                Appointments.Add(new AppointmentViewModel(appointment));
            else
                MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
        }
    }
}
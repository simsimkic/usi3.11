using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AdvancedMakeAppointmentViewModel : ViewModelBase
{
    private readonly ObservableCollection<string> _doctors;

    private DateTime _date = DateTime.Now + TimeSpan.FromHours(1);

    private string _doctorName;
    private readonly DoctorRepository _doctorRepository;
    private int _endHours;
    private int _endMinutes;
    private readonly Patient _patient;
    private readonly List<Appointment> _possibleAppointments;

    private string _priority;
    private readonly ScheduleRepository _scheduleRepository;

    private int _startHours;
    private int _startMinutes;

    public AdvancedMakeAppointmentViewModel(DoctorRepository doctorRepository, ScheduleRepository scheduleRepository,
        Patient patient, ObservableCollection<AppointmentViewModel> allAppointments)
    {
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _doctors = new ObservableCollection<string>();
        _patient = patient;
        Appointments = new ObservableCollection<AppointmentViewModel>();
        _possibleAppointments = new List<Appointment>();
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        PriorityOptions = new[] { "Doctor", "Time" };
        var doctors = doctorRepository.GetAll();
        foreach (var doctor in doctors) _doctors.Add(doctor.FullName + "-" + doctor.Email);

        RecommendAppointmentCommand = new DelegateCommand(o => RecommendAppointments());
        MakeAppointmentAdvancedCommand = new DelegateCommand(o => MakeAppointmentAdvanced(allAppointments));
    }

    public ObservableCollection<AppointmentViewModel> Appointments { get; }

    public AppointmentViewModel SelectedAppointment { get; set; }


    public IEnumerable<string> AllDoctors => _doctors;
    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }
    public string[] PriorityOptions { get; set; }

    public ICommand RecommendAppointmentCommand { get; set; }
    public ICommand MakeAppointmentAdvancedCommand { get; set; }

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

    public int StartHours
    {
        get => _startHours;
        set
        {
            _startHours = value;
            OnPropertyChanged();
        }
    }

    public int StartMinutes
    {
        get => _startMinutes;
        set
        {
            _startMinutes = value;
            OnPropertyChanged();
        }
    }

    public int EndHours
    {
        get => _endHours;
        set
        {
            _endHours = value;
            OnPropertyChanged();
        }
    }

    public int EndMinutes
    {
        get => _endMinutes;
        set
        {
            _endMinutes = value;
            OnPropertyChanged();
        }
    }

    public string Priority
    {
        get => _priority;
        set
        {
            _priority = value;
            OnPropertyChanged();
        }
    }

    private void RecommendAppointments()
    {
        try
        {
            Appointments.Clear();
            _possibleAppointments.Clear();
            var lastDate = Date;
            if (Date < DateTime.Now)
                throw new Exception();
            lastDate = new DateTime(lastDate.Year, lastDate.Month, lastDate.Day, 23, 59, 0);
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartHours,
                StartMinutes, 0);
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndHours, EndMinutes,
                0);
            if (startTime >= endTime)
                throw new Exception();
            var wantedTimeSlot = new TimeSlot(startTime, endTime);

            var tokens = DoctorName.Split("-");
            var doctorsMail = tokens[1];
            var doctor = _doctorRepository.GetDoctorByEmail(doctorsMail);

            if (Priority.Equals("Doctor"))
            {
                var fittingAppointments =
                    _scheduleRepository.FindAppointmentsByDoctorPriority(doctor, wantedTimeSlot, lastDate,
                        _patient.Email);
                foreach (var singleAppointment in fittingAppointments)
                {
                    _possibleAppointments.Add(singleAppointment);
                    Appointments.Add(new AppointmentViewModel(singleAppointment));
                }
            }
            else
            {
                var possibleAppointments =
                    _scheduleRepository.FindAppointmentsByTimePriority(doctor, wantedTimeSlot, lastDate, _patient.Email,
                        _doctorRepository);
                foreach (var singleAppointment in possibleAppointments)
                {
                    _possibleAppointments.Add(singleAppointment);
                    Appointments.Add(new AppointmentViewModel(singleAppointment));
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Error", "Error", MessageBoxButton.OK);
        }
    }

    private void MakeAppointmentAdvanced(ObservableCollection<AppointmentViewModel> allAppointments)
    {
        var selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
            foreach (var appointment in _possibleAppointments.Where(appointment =>
                         appointment.Id == selectedAppointment.Id))
                if (_scheduleRepository.isDoctorAvailable(appointment.Time, appointment.Doctor.Email) &&
                    _scheduleRepository.isPatientAvailable(appointment.Time, appointment.Doctor.Email))
                {
                    _scheduleRepository.AddAppointment(appointment);
                    allAppointments.Add(selectedAppointment);
                    return;
                }
                else
                {
                    MessageBox.Show("Invalid appointment", "Error", MessageBoxButton.OK);
                    return;
                }
        else
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
    }
}
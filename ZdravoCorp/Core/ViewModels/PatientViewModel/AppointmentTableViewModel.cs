using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.View;
using ZdravoCorp.View.PatientV;
using ZdravoCorp.View.PatientView;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AppointmentTableViewModel : ViewModelBase
{
    private readonly List<Appointment> _allAppointments;
    private ObservableCollection<AppointmentViewModel> _appointments;
    private readonly ScheduleRepository _controller;
    private readonly DoctorRepository _doctorRepository;
    private readonly Patient _patient;


    public AppointmentTableViewModel()
    {
    }

    public AppointmentTableViewModel(ScheduleRepository scheduleRepository,
        DoctorRepository doctorRepository, Patient patient)
    {
        _patient = patient;
        _controller = scheduleRepository;
        _appointments = new ObservableCollection<AppointmentViewModel>();
        _doctorRepository = doctorRepository;
        _allAppointments = _controller.GetPatientAppointments(_patient.Email);
        UpdateTable(_allAppointments);
        NewAppointmentCommand = new DelegateCommand(o => NewAppointment());
        ChangeAppointmentCommand = new DelegateCommand(o => ChangeAppointmentComm());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointmentComm());
        RecommendAppointmentCommand = new DelegateCommand(o => RecommendAppointmentComm());
    }

    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public AppointmentViewModel SelectedAppointment { get; set; }

    public ICommand NewAppointmentCommand { get; set; }
    public ICommand ChangeAppointmentCommand { get; set; }
    public ICommand CancelAppointmentCommand { get; set; }
    public ICommand RecommendAppointmentCommand { get; set; }

    public ObservableCollection<AppointmentViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = value;
            UpdateTable(_controller.GetPatientAppointments(_patient.Email));
        }
    }

    private void UpdateTable(List<Appointment> appointments)
    {
        foreach (var appointment in appointments) _appointments.Add(new AppointmentViewModel(appointment));
    }


    private void ChangeAppointmentComm()
    {
        var selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            var appointment = _controller.GetAppointmentById(selectedAppointment.Id);
            var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                var window = new ChangeAppointmentView
                {
                    DataContext = new ChangeAppointmentViewModel(selectedAppointment,
                        _controller, Appointments, _doctorRepository, _patient)
                };
                window.Show();
            }
            else
            {
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void NewAppointment()
    {
        var window = new MakeAppointmentView
        {
            DataContext = new MakeAppointmentViewModel(_controller, Appointments,
                _doctorRepository, _patient)
        };
        //var window = new MakeAppointmentView(_doctorRepository, _controller, Appointments, _patient);
        window.Show();
    }

    public void CancelAppointmentComm()
    {
        var selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            var appointment = _controller.GetAppointmentById(selectedAppointment.Id);
            var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                _controller.CancelAppointment(appointment);
                Appointments.Remove(GetById(selectedAppointment.Id, Appointments));
            }
            else
            {
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void RecommendAppointmentComm()
    {
        var window = new AdvancedMakeAppointmentView
        {
            DataContext = new AdvancedMakeAppointmentViewModel(_doctorRepository, _controller, _patient, Appointments)
        };
        window.Show();
    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
            if (appointmentViewModel.Id == id)
                return appointmentViewModel;

        return null;
    }
}
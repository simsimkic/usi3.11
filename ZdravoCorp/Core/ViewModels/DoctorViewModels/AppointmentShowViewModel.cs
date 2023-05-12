using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.View;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AppointmentShowViewModel : ViewModelBase
{
    private readonly ObservableCollection<MedicalRecordViewModel> _medicalRecords;

    private DateTime _dateAppointment = DateTime.Now + TimeSpan.FromHours(1);
    private readonly Doctor _doctor;
    private readonly DoctorRepository _doctorRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly MedicalRecordRepository _medicalRecordRepository;
    private readonly PatientRepository _patientRepository;
    private readonly RoomRepository _roomRepository;
    private readonly ScheduleRepository _scheduleRepository;
    private int counterViews;

    public AppointmentShowViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository,
        PatientRepository patientRepository, MedicalRecordRepository medicalRecordRepository,
        InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        counterViews = 0;
        _patientRepository = patientRepository;
        _scheduleRepository = scheduleRepository;
        _inventoryRepository = inventoryRepository;
        _roomRepository = roomRepository;

        _doctorRepository = doctorRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);

        var appointments = _scheduleRepository.GetDoctorAppointments(_doctor.Email);
        _medicalRecordRepository = medicalRecordRepository;

        Appointments = new ObservableCollection<AppointmentViewModel>();

        AddAppointmentCommand = new DelegateCommand(o => OpenAddDialog());
        ChangeAppointmentCommand = new DelegateCommand(o => OpenChangeDialog());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointment());
        SearchAppointmentCommand = new DelegateCommand(o => SearchAppointments());
        ViewMedicalRecordCommand = new DelegateCommand(o => ShowMedicalRecord());
        PerformAppointmentCommand = new DelegateCommand(o => ShowPerformingView());
    }

    public ObservableCollection<AppointmentViewModel> Appointments { get; }

    public AppointmentViewModel SelectedAppointments { get; set; }
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand AddAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand SearchAppointmentCommand { get; }
    public ICommand ViewMedicalRecordCommand { get; }
    public ICommand PerformAppointmentCommand { get; }

    public DateTime DateAppointment
    {
        get => _dateAppointment;
        set
        {
            _dateAppointment = value;
            if (_dateAppointment < DateTime.Today)
            {
                MessageBox.Show("Select date in future", "Error", MessageBoxButton.OK);
                _dateAppointment = DateTime.Today;
                return;
            }

            OnPropertyChanged();
        }
    }

    public void OpenAddDialog()
    {
        var addAp = new AddAppointmentView
        {
            DataContext = new AddAppointmentViewModel(_scheduleRepository, _doctorRepository, Appointments,
                _patientRepository, _doctor, _medicalRecordRepository, _dateAppointment)
        };
        addAp.Show();
    }

    public void OpenChangeDialog()
    {
        var appointmentViewModel = SelectedAppointments;
        if (appointmentViewModel != null)
        {
            var appointment = _scheduleRepository.GetAppointmentById(appointmentViewModel.Id);
            if (appointment.Status)
            {
                MessageBox.Show("Appointment is performed", "Error", MessageBoxButton.OK);
                return;
            }

            var patientMail = appointmentViewModel.PatientMail;
            var patient = _patientRepository.GetPatientByEmail(patientMail);
            var changeAp = new DrChangeAppointmentView
            {
                DataContext = new DrChangeAppointmentViewModel(SelectedAppointments, _scheduleRepository,
                    _doctorRepository, Appointments, _patientRepository, _doctor, patient, appointmentViewModel,
                    _dateAppointment)
            };
            changeAp.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void CancelAppointment()
    {
        var selectedAppointment = SelectedAppointments;
        if (selectedAppointment != null)
        {
            var appointment = _scheduleRepository.GetAppointmentById(selectedAppointment.Id);
            if (appointment.Status)
            {
                MessageBox.Show("Appointment is performed", "Error", MessageBoxButton.OK);
                return;
            }

            var cancelAppointment = _scheduleRepository.CancelAppointmentByDoctor(appointment);
            if (cancelAppointment != null)
                Appointments.Remove(GetById(selectedAppointment.Id, Appointments));
            else
                MessageBox.Show("Unable to cancel this appointment", "Error", MessageBoxButton.OK);
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
            if (appointmentViewModel.Id == id)
                return appointmentViewModel;
        return null;
    }

    public void SearchAppointments()
    {
        var showAppointments = _scheduleRepository.GetAppointmentsForShow(_dateAppointment);
        Appointments.Clear();
        foreach (var appointment in showAppointments)
            if (!appointment.IsCanceled && appointment.Doctor.Email.Equals(_doctor.Email))
                Appointments.Add(new AppointmentViewModel(appointment));
    }

    public void ShowMedicalRecord()
    {
        var appointment = SelectedAppointments;
        if (appointment != null)
        {
            var medicalR = _medicalRecordRepository.GetById(appointment.PatientMail);
            var window = new MedicalRecordView
                { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordRepository) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowPerformingView()
    {
        var appointment = SelectedAppointments;
        if (appointment != null)
        {
            var checkPerformAppointment = _scheduleRepository.CanPerformAppointment(appointment.Id);
            var appointmentPerforming = _scheduleRepository.GetAppointmentById(appointment.Id);
            if (checkPerformAppointment && !appointmentPerforming.Status)
            {
                var window = new PerformAppointmentView
                {
                    DataContext = new PerformAppointmentViewModel(appointmentPerforming, _scheduleRepository,
                        _patientRepository, _medicalRecordRepository, _inventoryRepository, _roomRepository)
                };
                window.Show();
            }
            else
            {
                MessageBox.Show("Appointment cannot be performed", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}
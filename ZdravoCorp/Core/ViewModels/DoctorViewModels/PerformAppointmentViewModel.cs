using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PerformAppointmentViewModel : ViewModelBase
{
    private string _allergens;
    private readonly Appointment _appointment;
    private AppointmentViewModel _appointmentViewModel;
    private readonly InventoryRepository _inventoryRepository;

    private string _keyWord;
    private readonly MedicalRecordRepository _medicalRecordRepository;

    private string _opinion;
    private readonly Patient _patient;
    private PatientRepository _patientRepository;
    private int _roomId;
    private readonly RoomRepository _roomRepository;
    private readonly ScheduleRepository _schedulerRepository;


    private string _symptoms;


    public PerformAppointmentViewModel(Appointment performingAppointment, ScheduleRepository scheduleRepository,
        PatientRepository patientRepository, MedicalRecordRepository medicalRecordRepository,
        InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _appointment = performingAppointment;
        _medicalRecordRepository = medicalRecordRepository;
        _schedulerRepository = scheduleRepository;
        _patientRepository = patientRepository;
        _patient = patientRepository.GetPatientByEmail(performingAppointment.PatientEmail);
        assignRoom();

        CancelCommand = new DelegateCommand(o => CloseWindow());
        MedicalRCommand = new DelegateCommand(o => ShowMedicalRecordDialog());
        PerformCommand = new DelegateCommand(o => SavePerformingAppointment());
    }

    public string PatientMail => _patient.Email;
    public string PatientName => _patient.FullName;
    public ICommand PerformCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand MedicalRCommand { get; }

    public string Symptoms
    {
        get => _symptoms;
        set
        {
            _symptoms = value;
            OnPropertyChanged();
        }
    }

    public string Opinion
    {
        get => _opinion;
        set
        {
            _opinion = value;
            OnPropertyChanged();
        }
    }

    public string Allergens
    {
        get => _allergens;
        set
        {
            _allergens = value;
            OnPropertyChanged();
        }
    }

    public string KeyWord
    {
        get => _keyWord;
        set
        {
            _keyWord = value;
            OnPropertyChanged();
        }
    }

    private void assignRoom()
    {
        foreach (var room in _roomRepository.Rooms)
        {
            var checkRoom = true;
            foreach (var appointment in _schedulerRepository.GetAllAppointments())
                if (room.Id == appointment.Room && _appointment.Time.Overlap(appointment.Time))
                    checkRoom = false;
            if (!checkRoom) continue;
            _roomId = room.Id;
            return;
        }
    }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ShowMedicalRecordDialog()
    {
        if (_patient != null)
        {
            var medicalR = _medicalRecordRepository.GetById(_patient.Email);
            var window = new ChangeMedicalRecordView
                { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordRepository) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowDEquipmentSpentDialog()
    {
        var window = new DEquipmentSpentView
            { DataContext = new DEquipmentSpentViewModel(_inventoryRepository, _roomId) };
        window.Show();
    }


    public void SavePerformingAppointment()
    {
        try
        {
            var patientSymptoms = Symptoms.Trim().Split(",").ToList();
            var patientAllergens = Allergens.Trim().Split(",").ToList();
            var doctorOpinion = Opinion.Trim();
            var anamnesisKeyWord = KeyWord;
            if (_schedulerRepository.CheckPerformingAppointmentData(patientSymptoms, doctorOpinion, patientAllergens,
                    anamnesisKeyWord))
            {
                CloseWindow();
                ShowDEquipmentSpentDialog();
                _schedulerRepository.ChangePerformingAppointment(_appointment.Id, patientSymptoms, doctorOpinion,
                    patientAllergens, anamnesisKeyWord, _roomId);
            }
            else
            {
                MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
        }
    }
}
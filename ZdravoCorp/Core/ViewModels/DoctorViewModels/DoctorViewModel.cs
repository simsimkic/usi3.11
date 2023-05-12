using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DoctorViewModel : ViewModelBase
{
    private object _currentView;
    private readonly Doctor _doctor;
    private readonly DoctorRepository _doctorRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly MedicalRecordRepository _medicalRecordRepository;
    private readonly PatientRepository _patientRepository;
    private readonly RoomRepository _roomRepository;
    private readonly ScheduleRepository _scheduleRepository;
    private readonly User _user;

    public DoctorViewModel(User user, RepositoryManager repositoryManager)
    {
        _inventoryRepository = repositoryManager.InventoryRepository;
        _roomRepository = repositoryManager.RoomRepository;
        _doctorRepository = repositoryManager.DoctorRepository;
        _scheduleRepository = repositoryManager.ScheduleRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);
        _patientRepository = repositoryManager.PatientRepository;
        var appointments = _scheduleRepository.GetDoctorAppointments(_doctor.Email);
        _medicalRecordRepository = repositoryManager.MedicalRecordRepository;

        _user = user;
        LoadAppointmentCommand = new DelegateCommand(o => LoadAppointments());
        LoadPatientsCommand = new DelegateCommand(o => LoadPatinets());
        _currentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository,
            _medicalRecordRepository, _inventoryRepository, _roomRepository);
    }

    public ICommand LoadAppointmentCommand { get; private set; }
    public ICommand LoadPatientsCommand { get; private set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository,
            _medicalRecordRepository, _inventoryRepository, _roomRepository);
    }

    public void LoadPatinets()
    {
        CurrentView = new PatientTableViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository,
            _medicalRecordRepository);
    }
}
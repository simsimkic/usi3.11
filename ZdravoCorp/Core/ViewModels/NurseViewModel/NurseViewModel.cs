using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel;

public class NurseViewModel : ViewModelBase

{
    private object _currentView;
    private readonly DoctorRepository _doctorRepository;
    private readonly MedicalRecordRepository _medicalRecordRepository;
    private readonly PatientRepository _patientRepository;
    private readonly ScheduleRepository _scheduleRepository;

    public NurseViewModel(RepositoryManager repositoryManager)
    {
        _patientRepository = repositoryManager.PatientRepository;
        _medicalRecordRepository = repositoryManager.MedicalRecordRepository;
        _scheduleRepository = repositoryManager.ScheduleRepository;
        _doctorRepository = repositoryManager.DoctorRepository;
        NewPatientReceptionCommand = new DelegateCommand(o => NewPatientReception());
        UrgentAppointmentReservationCommand = new DelegateCommand(o => UrgentAppointmentReservation());
        _currentView = new PatientReceptionViewModel(_patientRepository, _scheduleRepository);
    }

    public ICommand NewPatientReceptionCommand { get; private set; }
    public ICommand UrgentAppointmentReservationCommand { get; private set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void NewPatientReception()
    {
        CurrentView = new PatientReceptionViewModel(_patientRepository, _scheduleRepository);
    }

    public void UrgentAppointmentReservation()
    {
        CurrentView = new UrgentAppointmentViewModel(_medicalRecordRepository, _scheduleRepository, _doctorRepository);
    }
}
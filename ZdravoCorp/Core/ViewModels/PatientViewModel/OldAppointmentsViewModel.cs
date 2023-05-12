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
using ZdravoCorp.View.PatientView;
using static ZdravoCorp.Core.Models.Users.Doctor;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class OldAppointmentsViewModel : ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _allAppointments;
    private ObservableCollection<AppointmentViewModel> _appointments;
    private DoctorRepository _doctorRepository;
    private ObservableCollection<AppointmentViewModel> _filteredAppointments;
    private readonly Patient _patient;
    private readonly ScheduleRepository _scheduleRepository;
    private string _searchText = "";
    private string _selectedDoctor = "None";

    private string _selectedSpecialization = "None";

    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public List<Appointment> CompleteAppointments;

    public OldAppointmentsViewModel(ScheduleRepository scheduleRepository,
        DoctorRepository doctorRepository, Patient patient)
    {
        _patient = patient;
        _scheduleRepository = scheduleRepository;
        _allAppointments = new ObservableCollection<AppointmentViewModel>();
        _doctorRepository = doctorRepository;
        CompleteAppointments = _scheduleRepository.GetPatientsOldAppointments(_patient.Email);
        PossibleDoctors = new HashSet<string>();
        PossibleDoctors.Add("None");
        PossibleSpecializations = new HashSet<string>();
        PossibleSpecializations.Add("None");
        LoadComboBoxCollecitons();
        foreach (var appointment in CompleteAppointments)
        {
            _allAppointments.Add(new AppointmentViewModel(appointment));
            PossibleDoctors.Add(appointment.Doctor.FullName);
        }

        _appointments = _allAppointments;
        ViewAnamnesisCommand = new DelegateCommand(o => ViewAnamnesisComm());
    }

    public AppointmentViewModel SelectedAppointment { get; set; }

    public ICommand ViewAnamnesisCommand { get; set; }

    public string SearchBox
    {
        get => _searchText;
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }

    public string SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public string SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            _selectedSpecialization = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public IEnumerable<AppointmentViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = new ObservableCollection<AppointmentViewModel>(value);
            OnPropertyChanged();
        }
    }

    public HashSet<string> PossibleDoctors { get; }

    public HashSet<string> PossibleSpecializations { get; }

    private void UpdateTable()
    {
        _filteredAppointments = _allAppointments;
        var f1 = _filteredAppointments.Intersect(UpdateTableFromSearch());
        var f2 = f1.Intersect(UpdateTableFromDoctor());
        var f3 = f2.Intersect(UpdateTableFromSpecialization());
        Appointments = f3;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<AppointmentViewModel>(Search(_searchText));
        return _allAppointments;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromDoctor()
    {
        if (_selectedDoctor != "None")
            return new ObservableCollection<AppointmentViewModel>(FilterByDoctor(_selectedDoctor));
        return _allAppointments;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromSpecialization()
    {
        if (_selectedSpecialization != "None")
            return new ObservableCollection<AppointmentViewModel>(FilterBySpecialization(_selectedSpecialization));
        return _allAppointments;
    }

    public IEnumerable<AppointmentViewModel> Search(string inputText)
    {
        return _allAppointments.Where(item => item.Anamnesis.ToLower().Contains(inputText.ToLower()));
    }

    public IEnumerable<AppointmentViewModel> FilterByDoctor(string doctor)
    {
        return _allAppointments.Where(item => item.DoctorName == doctor);
    }

    public IEnumerable<AppointmentViewModel> FilterBySpecialization(string specialization)
    {
        return _allAppointments.Where(item => item.Specialization == specialization);
    }

    public void ViewAnamnesisComm()
    {
        if (SelectedAppointment != null)
        {
            Appointment? selectedAppointment = null;
            foreach (var appointment in CompleteAppointments.Where(appointment =>
                         appointment.Id == SelectedAppointment.Id)) selectedAppointment = appointment;

            var window = new FullAnamnesisView
            {
                DataContext = new FullAnamnesisViewModel(selectedAppointment.Anamnesis)
            };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void LoadComboBoxCollecitons()
    {
        PossibleSpecializations.Add("None");
        PossibleSpecializations.Add(SpecializationType.Surgeon.ToString());
        PossibleSpecializations.Add(SpecializationType.Psychologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Neurologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Urologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Anesthesiologist.ToString());
    }
}
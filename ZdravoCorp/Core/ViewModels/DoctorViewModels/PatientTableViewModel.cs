using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PatientTableViewModel : ViewModelBase
{
    private readonly ObservableCollection<PatientsViewModel> _Allpatients;


    private readonly Doctor _doctor;
    private readonly DoctorRepository _doctorRepository;
    private readonly MedicalRecordRepository _medicalRecordRepository;
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<PatientsViewModel> _patients;
    private readonly ScheduleRepository _scheduleRepository;

    private ObservableCollection<PatientsViewModel> _searchedPatients;

    // public ObservableCollection<PatientsViewModel> Patients => _patients; 
    private string _searchText = "";

    public PatientTableViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository,
        PatientRepository patientRepository, MedicalRecordRepository medicalRecordRepository)
    {
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);
        _patientRepository = patientRepository;
        _medicalRecordRepository = medicalRecordRepository;

        var patinets = _patientRepository.Patients;

        _Allpatients = new ObservableCollection<PatientsViewModel>();
        foreach (var patient in patinets) _Allpatients.Add(new PatientsViewModel(patient));
        _patients = _Allpatients;

        ChangeMedicalRecordCommand = new DelegateCommand(o => OpenMedicalRecordChange());
    }

    public PatientsViewModel SelectedPatient { get; set; }

    public ICommand ChangeMedicalRecordCommand { get; }

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

    public IEnumerable<PatientsViewModel> Patients
    {
        get => _patients;
        set
        {
            _patients = new ObservableCollection<PatientsViewModel>(value);
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PatientsViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<PatientsViewModel>(Search(_searchText));
        return _patients;
    }

    public IEnumerable<PatientsViewModel> Search(string inputText)
    {
        var p = _Allpatients.Where(patient => patient.ToString().Contains(inputText));
        return p;
    }


    private void UpdateTable()
    {
        _searchedPatients = _Allpatients;
        var f1 = _searchedPatients.Intersect(UpdateTableFromSearch());
        Patients = f1;
    }

    public void OpenMedicalRecordChange()
    {
        var patient = SelectedPatient;
        if (patient != null)
        {
            var _patient = _patientRepository.GetPatientByEmail(patient.Email);
            var isExamined = _scheduleRepository.IsPatientExamined(_patient, _doctor);
            if (isExamined)
            {
                var medicalR = _medicalRecordRepository.GetById(patient.Email);
                var window = new ChangeMedicalRecordView
                    { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordRepository) };
                window.Show();
            }
            else
            {
                MessageBox.Show("Patient is not examined", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;

namespace ZdravoCorp.Core.ViewModels;

internal class MedicalRecordViewModel : ViewModelBase
{
    private readonly MedicalRecord _medicalRecord;

    private string _diseaseHistory;

    private int _height;
    private readonly MedicalRecordRepository _medicalRecordRepository;

    private int _weight;

    public MedicalRecordViewModel(MedicalRecord medicalRecord, MedicalRecordRepository medicalRecordRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _medicalRecord = medicalRecord;
        _height = _medicalRecord.height;
        _weight = _medicalRecord.weight;
        _diseaseHistory = medicalRecord.DiseaseHistoryToString();
        SaveCommand = new DelegateCommand(o => SaveChangesMedicalRecord());
        CloseCommand = new DelegateCommand(o => CloseWindow());
    }

    public int PatientHeight => _medicalRecord.height;
    public int PatientWeight => _medicalRecord.weight;
    public string PatientName => _medicalRecord.user.FullName;
    public string PatientDeseaseHistory => _medicalRecord.DiseaseHistoryToString();

    public ICommand SaveCommand { get; }
    public ICommand CloseCommand { get; }

    public int Height
    {
        get => _height;
        set
        {
            _height = value;
            OnPropertyChanged();
        }
    }

    public int Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            OnPropertyChanged();
        }
    }

    public string DiseaseHistory
    {
        get => _diseaseHistory;
        set
        {
            _diseaseHistory = value;
            OnPropertyChanged();
        }
    }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void SaveChangesMedicalRecord()
    {
        try
        {
            var height = Height;
            var weight = Weight;
            var diseasHistory = DiseaseHistory.Trim().Split(",").ToList();
            var checkData = _medicalRecordRepository.CheckDataForChanges(weight, height, diseasHistory);
            if (checkData)
            {
                _medicalRecordRepository.ChangeRecord(_medicalRecord.user.Email, height, weight, diseasHistory);
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
        }
    }
}
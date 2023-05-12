using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel;

public class UrgentAppointmentViewModel : ViewModelBase
{
    private DoctorRepository _doctorRepository;
    private MedicalRecordRepository _medicalRecordRepository;

    private ScheduleRepository _scheduleRepository;

    public UrgentAppointmentViewModel(MedicalRecordRepository medicalRecordRepository,
        ScheduleRepository scheduleRepository, DoctorRepository doctorRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        FindUrgentAppointmentCommand = new DelegateCommand(o => FindUrgentAppointment());
        SpecializationTypes = new ObservableCollection<string>();
        SpecializationTypes.Add(Doctor.SpecializationType.Surgeon.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Psychologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Neurologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Urologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Anesthesiologist.ToString());
        //_specializationTypes.Add("Any");
    }

    public ICommand FindUrgentAppointmentCommand { get; set; }

    public string PatientEmail { get; set; }

    public Doctor.SpecializationType SpecializationType { get; set; }

    public ObservableCollection<string> SpecializationTypes { get; }

    public void FindUrgentAppointment()
    {
        /*DateTime now = DateTime.Now;
        List<Doctor> suitableDoctors = _doctorRepository.GetAllSpecialized(_specializationType);
        if (suitableDoctors.Count == 0)
        {
            //napraviti pop up obavestenja da nema doktora za ovaj posao
        }
        else
        {
            List<List<Appointment>> allDoctorsAppointments = new List<List<Appointment>> { };
            List<Appointment> appointments = new List<Appointment> { };
            List<Tuple<TimeSlot, string>> termins = new List<Tuple<TimeSlot, string>> { }; 
            //List<string> doctorsEmails = new List<string>();
            DateTime latestTime = DateTime.Now.AddHours(2);

            TimeSlot interval = new TimeSlot(now, now.AddHours(2));
            foreach (Doctor suitableDoctor in suitableDoctors)
            {
                //ovde pitati kuma sta kako ako je ikako moguce private/public kod metode mu smeta
                //TimeSlot compareTime = _scheduleRepository.FindAvailableTimeslotsForOneDoctor(suitableDoctor.Email, interval, now, null);
                TimeSlot termin = _scheduleRepository.FindAvailableTimeslotsForOneDoctor(suitableDoctor.Email, interval, DateTime.Today);
                if (termin != null) {
                    termins.Add(new Tuple<TimeSlot, string>(termin, suitableDoctor.Email));
                }
                termins.Sort((a, b) => a.Item1.IsBefore(b.Item1));

                //allDoctorsAppointments.Add(_scheduleRepository.GetDoctorAppointments(suitableDoctor.Email));
            }



        }*/
    }

    public DateTime GetFirstTime(List<List<Appointment>> allDoctorsAppointments)
    {
        var latestTime = DateTime.Now.AddHours(2);
        var earliestTime = DateTime.Now;

        foreach (var oneDoctorsAppointments in allDoctorsAppointments)
        {
        }

        return latestTime;
    }
}
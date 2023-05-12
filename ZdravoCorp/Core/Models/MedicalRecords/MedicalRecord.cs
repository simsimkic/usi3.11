using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Models.MedicalRecords;

public class MedicalRecord
{
    public Patient user;

    public MedicalRecord()
    {
    }

    public MedicalRecord(Patient patient)
    {
        user = patient;
        height = 0;
        weight = 0;
        deseaseHistory = new List<string>();
    }

    public MedicalRecord(Patient patient, int h, int w)
    {
        user = patient;
        height = h;
        weight = w;
        deseaseHistory = new List<string>();
    }

    public MedicalRecord(Patient patient, int height, int weight, List<string> deseaseHistory)
    {
        user = patient;
        this.height = height;
        this.weight = weight;
        this.deseaseHistory = deseaseHistory;
    }

    public int height { get; set; }
    public int weight { get; set; }
    public List<string> deseaseHistory { get; set; }

    public override string ToString()
    {
        return "Patient : " + user + "height : " + height + "weight : " + weight;
    }

    public string DiseaseHistoryToString()
    {
        var result = deseaseHistory.Any() ? string.Join(",", deseaseHistory) : string.Empty;
        return result;
    }
}
using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Users;

public class Doctor

{
    public enum SpecializationType
    {
        Psychologist,
        Surgeon,
        Neurologist,
        Urologist,
        Anesthesiologist
    }

    [JsonConstructor]
    public Doctor(string email, string firstName, string lastName, SpecializationType specialization)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }


    public SpecializationType Specialization { get; set; }

    [JsonIgnore] public string FullName => string.Format("Dr {0} {1}", FirstName, LastName);

    protected bool Equals(Doctor other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName &&
               Specialization == other.Specialization;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Doctor)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName, (int)Specialization);
    }
}
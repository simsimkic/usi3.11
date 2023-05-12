using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Users;

public class Director
{
    [JsonConstructor]
    public Director(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [JsonIgnore] public string FullName => string.Format("Nurse {0} {1}", FirstName, LastName);


    protected bool Equals(Director other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Director)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName);
    }
}
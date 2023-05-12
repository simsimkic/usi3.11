using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Users;

public class User
{
    public enum State
    {
        BlockedBySystem,
        BlockedBySecretary,
        NotBlocked
    }

    public enum UserType
    {
        Director,
        Doctor,
        Patient,
        Nurse
    }


    [JsonConstructor]
    public User(string? password, string email, UserType type, State userState)
    {
        Password = password;
        Email = email;
        UserState = userState;
        Type = type;
    }

    public User(string email, UserType type, State userState)
    {
        Email = email;
        UserState = userState;
    }

    public UserType Type { get; set; }
    public string Email { get; set; }
    public string? Password { private get; set; }
    public State UserState { get; set; }


    public bool ValidatePassword(string password)
    {
        return Password == password;
    }

    protected bool Equals(User other)
    {
        return Type == other.Type && Email == other.Email && Password == other.Password && UserState == other.UserState;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Email, Password, (int)UserState);
    }
}
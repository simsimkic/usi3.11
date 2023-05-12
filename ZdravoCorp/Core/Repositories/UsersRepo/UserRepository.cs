using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public class UserRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\users.json";
    private List<User> _users;


    public UserRepository(List<User> us)
    {
        _users = us;
        //this.LoadFromFile();
    }

    public UserRepository()
    {
        _users = new List<User>();
        Serializer.Load(this);
    }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _users;
    }

    public void Import(JToken token)
    {
        _users = token.ToObject<List<User>>();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }


    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return _users.Exists(user => user.Email == email);
    }
}
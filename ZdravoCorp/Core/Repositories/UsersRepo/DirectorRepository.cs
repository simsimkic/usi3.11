using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public class DirectorRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\directors.json";


    public DirectorRepository()
    {
        Serializer.Load(this);
    }

    public DirectorRepository(Director director)
    {
        Director = director;
        Serializer.Load(this);
    }

    public Director? Director { get; private set; }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        var list = new List<object>();
        if (Director != null) list.Add(Director);
        return list;
    }

    public void Import(JToken token)
    {
        Director = token.ToObject<Director>();
    }
}
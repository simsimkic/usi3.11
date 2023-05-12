using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ZdravoCorp.Core.Utilities;

public interface ISerializable
{
    public string FileName();

    public IEnumerable<object>? GetList();
    public void Import(JToken token);
}
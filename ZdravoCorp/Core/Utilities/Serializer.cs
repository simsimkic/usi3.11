using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;

namespace ZdravoCorp.Core.Utilities;

public static class Serializer
{
    private static readonly JsonSerializer _serializer;

    static Serializer()
    {
        _serializer = new JsonSerializer();
        _serializer.Converters.Add(new StringEnumConverter());
    }

    public static void Save(ISerializable repository)
    {
        var enumerable = repository.GetList();
        if (enumerable == null)
        {
            Console.WriteLine($"Repository is empty! {repository.FileName()}");
            return;
        }

        var data = JsonConvert.SerializeObject(enumerable);
        File.WriteAllText(repository.FileName(), data);
    }

    public static void Load(ISerializable repository)
    {
        var text = File.ReadAllText(repository.FileName());
        if (text == "")
            throw new EmptyFileException("File is empty!");

        try
        {
            var result = JsonConvert.DeserializeObject<JToken>(text);
            repository.Import(result);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
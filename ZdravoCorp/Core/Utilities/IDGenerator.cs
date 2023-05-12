using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using ZdravoCorp.Core.Exceptions;
using JsonException = System.Text.Json.JsonException;

namespace ZdravoCorp.Core.Utilities;

public class IDGenerator
{
    private static int _currentId;
    private static readonly string _fileName = @".\..\..\..\Data\idg.json";

    public IDGenerator()
    {
        _currentId = LoadFromFile();
    }

    public int CurrentId { get; set; }

    public static void SaveToFile()
    {
        var id = JsonConvert.SerializeObject(_currentId);

        File.WriteAllText(_fileName, id);
    }

    public int LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            var id = JsonConvert.DeserializeObject<int>(text);
            return id;
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
        }

        return -1;
    }

    public static int GetId()
    {
        _currentId++;
        SaveToFile();
        return _currentId;
    }
}
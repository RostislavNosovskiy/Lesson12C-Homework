using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Serialization;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

// {"Current":{"Time":"2023-06-18T20:35:06.722127+04:00","Temperature":29,"Weathercode":1,"Windspeed":2.1,"Winddirection":1},
// "History":
// [{"Time":"2023-06-17T20:35:06.77707+04:00","Temperature":29,"Weathercode":2,"Windspeed":2.4,"Winddirection":1},
// {"Time":"2023-06-16T20:35:06.777081+04:00","Temperature":22,"Weathercode":2,"Windspeed":2.4,"Winddirection":1},
// {"Time":"2023-06-15T20:35:06.777082+04:00","Temperature":21,"Weathercode":4,"Windspeed":2.2,"Winddirection":1}]}

namespace Lesson11C;
public class Program
{


    public static string json = """
        {"Current":{"Time":"2023-06-18T20:35:06.722127+04:00","Temperature":29,"Weathercode":1,"Windspeed":2.1,"Winddirection":1},"History":[{"Time":"2023-06-17T20:35:06.77707+04:00","Temperature":29,"Weathercode":2,"Windspeed":2.4,"Winddirection":1},{"Time":"2023-06-16T20:35:06.777081+04:00","Temperature":22,"Weathercode":2,"Windspeed":2.4,"Winddirection":1},{"Time":"2023-06-15T20:35:06.777082+04:00","Temperature":21,"Weathercode":4,"Windspeed":2.2,"Winddirection":1}]}
        """;

    public static void Main()
    {
        var res = (new JsonParser()).ParseJson(json, "Temperature");
        foreach(var item in res)
        {
            Console.WriteLine(item);
        }
        var obj = JsonSerializer.Deserialize<WetherData>(json);
        Console.WriteLine(obj);
        XmlSerializer serializer = new XmlSerializer(typeof(WetherData));
        using (StreamWriter writer = new StreamWriter("/Users/rostislavnosovskij/Projects/Lesson11C/Lesson11C/Task1.txt"))
        {
            serializer.Serialize(writer, obj);
        }
        Console.WriteLine("object has been serialized");
    }

   
    
}


public class JsonParser
{
    private string? _value;
    private List<string>? _results = new List<string>();
    public List<string> ParseJson(string json, string value)
    {
        _value = value;
        var jsonDocument = JsonDocument.Parse(json);
        var root = jsonDocument.RootElement;
        ParseElement(root);
        return _results;
    }

    private void ParseElement(JsonElement je, bool save = false)
    { 
        switch (je.ValueKind)
        {
            case JsonValueKind.Object:
                parseObject(je);
                break;
            case JsonValueKind.Array:
                parseArray(je);
                break;
            case JsonValueKind.String:
                parseString(je, save);
                break;
            case JsonValueKind.Number:
                parseNumber(je, save);
                break;
            case JsonValueKind.True:
            case JsonValueKind.False:
                parseBoolean(je);
                break;
            case JsonValueKind.Null:
                parseNull();
                break;
            default:
                throw new NotSupportedException("Unsupportet JSON value kind " + je.ValueKind);
        }
    }

    private void parseObject(JsonElement js)
    {
        foreach (var el in js.EnumerateObject())
        {
            Console.WriteLine($"proprty = {el.Name}");
            bool save = el.Name == _value;
            ParseElement(el.Value, save);
        }
    }

    private void parseArray(JsonElement js)
    {
        foreach (var el in js.EnumerateArray())
        {
            ParseElement(el);
        }
    }

    private void parseString(JsonElement js, bool save )
    {
        if (save)
        {
            _results.Add(js.GetString());
        }
        Console.WriteLine("String = " + js.GetString()); 
    }

    private void parseNumber(JsonElement js, bool save )
    {
        if (save)
        {
            _results.Add(js.GetRawText());
        }
        Console.WriteLine("Number = " + js.GetRawText());
    }

    private void parseBoolean(JsonElement js)
    {
        Console.WriteLine("Boolean value " + js.GetBoolean());
    }

    private void parseNull()
    {
        Console.WriteLine("Null value");
    }
}

public class WetherInfo
{
    public DateTime Time { get; set; }
    public double Temperature { get; set; }
    public int Wethercode { get; set; }
    public double Windspeed { get; set; }
    public int Winddirection { get; set; }
}


public class WetherData
{
    public WetherInfo Current { get; set; }
    public List<WetherInfo> History { get; set; }
}






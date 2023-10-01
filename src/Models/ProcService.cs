using System;
using System.IO;
using System.Linq;

namespace YAAM;

class ProcService
{
    public static Meminfo GetReadings()
    {
        var path = "/proc/meminfo";
        var lines = File.ReadAllLines(path);

        Meminfo meminfo = new()
        {
            MemTotal = ParseLine(lines, "MemTotal"),
            MemAvailable = ParseLine(lines, "MemAvailable"),
            MemFree = ParseLine(lines, "MemFree"),
        };

        return meminfo;
    }

    private static MeminfoReading ParseLine(string[] lines, string name)
    {
        var line = lines.FirstOrDefault(line => line.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))/*.AsSpan()*/;

        if (line is null) return new MeminfoReading(name, 0, "kB");

        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3) throw new Exception("Unable to parse input");

        var value = (int)decimal.Parse(parts[1]) / 1024; //MB /*/ 1024; // In GB*/
        var unit = "MB";

        return new MeminfoReading(name, value, unit);
    }
}
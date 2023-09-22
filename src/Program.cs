namespace YAAM;

internal class Program
{
    private static void Main(string[] args)
    {
        var path = "/proc/meminfo";
        var lines = File.ReadAllLines(path);

        Meminfo meminfo = new()
        {
            MemTotal = ParseLine(lines, "MemTotal"),
            MemAvailable = ParseLine(lines, "MemAvailable"),
            MemFree = ParseLine(lines, "MemFree"),
        };

        Console.WriteLine($"{nameof(meminfo.MemTotal)}: {meminfo.MemTotal.Value} {meminfo.MemTotal.Unit}");
        Console.WriteLine($"{nameof(meminfo.MemFree)}: {meminfo.MemFree.Value} {meminfo.MemFree.Unit}");
        Console.WriteLine($"{nameof(meminfo.MemAvailable)}: {meminfo.MemAvailable.Value} {meminfo.MemAvailable.Unit}");
        Console.WriteLine($"Memory used: {meminfo.MemTotal.Value - meminfo.MemAvailable.Value} GB");
    }

    private static MeminfoReading ParseLine(string[] lines, string name)
    {
        var line = lines.FirstOrDefault(line => line.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))/*.AsSpan()*/;

        if (line is null) return new MeminfoReading(name, 0, "kB");

        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3) throw new Exception("Unable to parse input");

        var value = (int)decimal.Parse(parts[1]) / 1024 / 1024; // In GB
        var unit = "GB";

        return new MeminfoReading(name, value, unit);
    }
}
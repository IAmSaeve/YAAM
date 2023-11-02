namespace YAAM.Models;

internal record Meminfo(MeminfoReading? MemTotal, MeminfoReading? MemFree, MeminfoReading? MemAvailable)
{
    public Meminfo() : this(default, default, default)
    {
    }
}
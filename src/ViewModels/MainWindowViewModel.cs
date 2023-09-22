namespace YAAM.ViewModels;

using System;
using System.Threading;
using ReactiveUI;

public class MainWindowViewModel : ViewModelBase
{
    private string _memFree;
    private string _memAvailable;
    private string _memTotal;
    private string _memUsed;

    public string MemTotal { get => _memTotal; set => this.RaiseAndSetIfChanged(ref _memTotal, value); }
    public string MemFree { get => _memFree; set => this.RaiseAndSetIfChanged(ref _memFree, value); }
    public string MemAvailable { get => _memAvailable; set => this.RaiseAndSetIfChanged(ref _memAvailable, value); }
    public string MemUsed { get => _memUsed; set => this.RaiseAndSetIfChanged(ref _memUsed, value); }

    public MainWindowViewModel()
    {
        MemTotal = $"{Math.Round(ProcService.GetReadings().MemTotal.Value / 1024, 1)} GB";
        var timer = new Timer(Refresher, null, 0, 1000);
    }

    public void Refresher(object? state)
    {
        var readings = ProcService.GetReadings();

        var memFree = $"{nameof(readings.MemFree)}: {Math.Round(readings.MemFree.Value / 1024, 1)} GB";
        var memAvailable = $"{nameof(readings.MemAvailable)}: {Math.Round(readings.MemAvailable.Value / 1024, 1)} GB";

        MemFree = memFree;
        MemAvailable = memAvailable;
        MemUsed = $"{Math.Round((readings.MemTotal.Value - readings.MemAvailable.Value) / 1024, 1)} GB";
    }
}

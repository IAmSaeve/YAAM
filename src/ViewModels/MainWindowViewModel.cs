using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using YAAM.Models;

namespace YAAM.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ObservableCollection<decimal> _memoryUsageReadings = new();
    private string? _memAvailable;
    private string? _memFree;
    private string? _memTotal;
    private string? _memUsed;

    public MainWindowViewModel()
    {
        var initialReading = ProcService.GetReadings();
        MemTotal = $"MemTotal: {Math.Round((initialReading.MemTotal?.Value ?? 0) / 1024, 1)} GB";

        new DispatcherTimer(TimeSpan.FromMilliseconds(300), DispatcherPriority.Normal, Refresher).Start();

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<decimal>
            {
                Values = _memoryUsageReadings,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                GeometrySize = 0,
                Stroke = new SolidColorPaint(SKColors.CornflowerBlue),
                IsHoverable = false,
                GeometryFill = null,
                GeometryStroke = null
            }
        };
    }

    public string? MemTotal
    {
        get => _memTotal;
        set => this.RaiseAndSetIfChanged(ref _memTotal, value);
    }

    public string? MemFree
    {
        get => _memFree;
        set => this.RaiseAndSetIfChanged(ref _memFree, value);
    }

    public string? MemAvailable
    {
        get => _memAvailable;
        set => this.RaiseAndSetIfChanged(ref _memAvailable, value);
    }

    public string? MemUsed
    {
        get => _memUsed;
        set => this.RaiseAndSetIfChanged(ref _memUsed, value);
    }

    public ObservableCollection<ISeries> Series { get; set; }

    public Axis[] YAxes { get; set; } =
    {
        new()
        {
            MinLimit = 0,
            MaxLimit = 32
        }
    };

    public Axis[] XAxes { get; set; } =
    {
        new()
        {
            MinLimit = null,
            MaxLimit = null
        }
    };

    private void Refresher(object? sender, EventArgs e)
    {
        var readings = ProcService.GetReadings();

        var memFree = $"{nameof(readings.MemFree)}: {Math.Round((readings.MemFree?.Value ?? 0) / 1024, 1)} GB";
        var memAvailable = $"{nameof(readings.MemAvailable)}: {Math.Round((readings.MemAvailable?.Value ?? 0) / 1024, 1)} GB";

        MemFree = memFree;
        MemAvailable = memAvailable;
        MemUsed = $"MemUsed: {Math.Round(((readings.MemTotal?.Value ?? 0) - (readings.MemAvailable?.Value ?? 0)) / 1024, 1)} GB";

        if (_memoryUsageReadings.Count < 10)
        {
            _memoryUsageReadings.Add(Math.Round(((readings.MemTotal?.Value ?? 0) - (readings.MemAvailable?.Value ?? 0)) / 1024, 1));
        }
        else
        {
            _memoryUsageReadings.RemoveAt(0);
            _memoryUsageReadings.Add(Math.Round(((readings.MemTotal?.Value ?? 0) - (readings.MemAvailable?.Value ?? 0)) / 1024, 1));
        }
    }
}
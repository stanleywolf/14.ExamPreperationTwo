using System;
using System.Collections.Generic;
using System.Text;

public class Track
{
    private Weather weather;

    public Track(int lapsNumber, int trackLength)
    {
        this.LapsNumber = lapsNumber;
        this.TrackLength = trackLength;
        this.Weather = Weather.Sunny;
        this.CurrentLap = 0;
    }

    public Weather Weather { get; set; }
    
    public int LapsNumber { get; }
    public int TrackLength { get; }
    public int CurrentLap { get; set; }
}
using System;
using System.Collections.Generic;
using System.Text;

public static class OutputMessages
{
    public static string BlowTyre => "Blown Tyre";
    public static string OutOfFuel => "Out of fuel";
    public static string Crashed => "Crashed";
    public static string InvalidTyreType => "Invalid Tyre Type";
    public static string InvalidDriverType => "Invalid Driver Type";
    public static string InvalidWeatherType => "Invalid Weather Type";
    public static string OutOfLaps => "There is no time! On lap {0}.";

    public static string OvertakeMessage =>
        "{0} has overtaken {1} on lap {2}.";
}

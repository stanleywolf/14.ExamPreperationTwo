using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

public class RaceTower
{
    private const string CRASH = "Crashed";

    public RaceTower()
    {
        this.tyreFactory = new TyreFactory();
        this.driverFactory = new DriverFactory();
        this.racingDrivers = new List<Driver>();
        this.failedDrivers = new Stack<Driver>();
    }
    private IList<Driver> racingDrivers;
    private Stack<Driver> failedDrivers;
    private Track track;
    private DriverFactory driverFactory;
    private TyreFactory tyreFactory;

    public bool RaceOver => this.track.CurrentLap == this.track.LapsNumber;

    public void SetTrackInfo(int lapsNumber, int trackLength)
    {
       this.track = new Track(lapsNumber, trackLength);
    }
    public void RegisterDriver(List<string> commandArgs)
    {
        try
        {
            string driverType = commandArgs[0];
            string driverName = commandArgs[1];

            int hp = int.Parse(commandArgs[2]);
            double fuelAmount = double.Parse(commandArgs[3]);

            string[] tyreArgs = commandArgs.Skip(4).ToArray();

            Tyre tyre = this.tyreFactory.CreateTyre(tyreArgs);
            Car car = new Car(hp, fuelAmount, tyre);

            Driver driver = this.driverFactory.CreateDriver(driverType, driverName, car);

            this.racingDrivers.Add(driver);
        }
        catch  { }
    }

    //private Driver CreateDriver(string driverType, string driverName,Car car)
    //{
    //    Driver driver = null;
    //    if (driverType == "Aggressive")
    //    {
    //        driver = new AggressiveDriver(driverName, car);
    //    }
    //    else if (driverType == "Endurance")
    //    {
    //        driver = new EnduranceDriver(driverName, car);
    //    }
    //    if (driver == null)
    //    {
    //        throw new ArgumentException(OutputMessages.InvalidDriverType);
    //    }
    //    return driver;
    //}

    //private Tyre CreateTyre(string[] tyreArgs)
    //{
    //    string tyreType = tyreArgs[0];
    //    double tyreHardness = double.Parse(tyreArgs[1]);

    //    Tyre tyre = null;
    //    if (tyreType == "Hard")
    //    {
    //        tyre = new HardTyre(tyreHardness);
    //    }
    //    else if (tyreType == "Ultrasoft")
    //    {
    //        tyre = new UltrasoftTyre(tyreHardness, double.Parse(tyreArgs[2]));
    //    }
    //    if (tyre == null)
    //    {
    //        throw new ArgumentException(OutputMessages.InvalidTyreType);
    //    }
    //    return tyre;
    //}

    public void DriverBoxes(List<string> commandArgs)
    {
        string boxReason = commandArgs[0];
        string driverName = commandArgs[1];

        Driver driver = this.racingDrivers.FirstOrDefault(d => d.Name == driverName);

        string[] methodArgs = commandArgs.Skip(2).ToArray();
        if (boxReason == "Refuel")
        {
            driver.Refuel(methodArgs);
        }
        else if (boxReason == "ChangeTyres")
        {
            Tyre tyre = this.tyreFactory.CreateTyre(methodArgs);
            driver.ChangeTyres(tyre);
        }

    }

    public string CompleteLaps(List<string> commandArgs)
    {
        var sb = new StringBuilder();

        int numberOfLaps = int.Parse(commandArgs[0]);
        if (numberOfLaps > track.LapsNumber)
        {
            try
            {
                throw new ArgumentException(String.Format(OutputMessages.OutOfLaps, this.track.CurrentLap));
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
            
        }
        for (int laps = 0; laps < numberOfLaps; laps++)
        {
            for (int index = 0; index < this.racingDrivers.Count; index++)
            {
                Driver driver = racingDrivers[index];
                try
                {
                    driver.CompleteLap(this.track.TrackLength);
                }
                catch (ArgumentException e)
                {
                    driver.Fail(e.Message);
                    this.failedDrivers.Push(driver);
                    this.racingDrivers.RemoveAt(index);
                    index--;
                }
            }
            this.track.CurrentLap++;

            IList<Driver> orderedDrivers = this.racingDrivers.OrderByDescending(d => d.TotalTime).ToList();

            for (int i = 0; i < orderedDrivers.Count - 1; i++)
            {
                Driver overtakingDriver = orderedDrivers[i];
                Driver targetDriver = orderedDrivers[i + 1];

                bool isOvertake = this.TryOvertake(overtakingDriver, targetDriver);
                if (isOvertake)
                {
                    i++;
                    sb.AppendLine(String.Format(OutputMessages.OvertakeMessage, overtakingDriver.Name,
                        targetDriver.Name,
                        this.track.CurrentLap));
                }
                else
                {
                    if (!overtakingDriver.IsRace)
                    {
                        this.failedDrivers.Push(overtakingDriver);
                        this.racingDrivers.Remove(overtakingDriver);
                    }
                }
            }
        }

        if (this.RaceOver)
        {
            Driver winner = this.racingDrivers.OrderBy(d => d.TotalTime).First();
            sb.AppendLine($"{winner.Name} wins the race for {winner.TotalTime:f3} seconds.");
        }
        string result = sb.ToString().TrimEnd();
        return result;
    }

    private bool TryOvertake(Driver overtakingDriver, Driver targetDriver)
    {
        double timeDiff = overtakingDriver.TotalTime - targetDriver.TotalTime;
        bool success = false;


        if ((overtakingDriver is AggressiveDriver && overtakingDriver.Car.Tyre is UltrasoftTyre) && timeDiff <= 3)
        {
            if (this.track.Weather == Weather.Foggy)
            {
                overtakingDriver.Fail(CRASH);
                return false;
            }
            overtakingDriver.TotalTime -= 3;
            targetDriver.TotalTime += 3;
            return true;
        }
        if ((overtakingDriver is EnduranceDriver && overtakingDriver.Car.Tyre is HardTyre) && timeDiff <= 3)
        {
            if (this.track.Weather == Weather.Rainy)
            {
                overtakingDriver.Fail(CRASH);
                return false;
            }
            overtakingDriver.TotalTime -= 3;
            targetDriver.TotalTime += 3;
            return true;
        }
        if (timeDiff <= 2)
        {
            overtakingDriver.TotalTime -= 2;
            targetDriver.TotalTime += 2;
            return true;
        }
        return false;
    }

    public string GetLeaderboard()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Lap {this.track.CurrentLap}/{this.track.LapsNumber}");

        IEnumerable<Driver> leaderBoardDrivers = this.racingDrivers.OrderBy(d => d.TotalTime)
            .Concat(this.failedDrivers);

        int position = 1;
        foreach (Driver driver in leaderBoardDrivers)
        {
            sb.AppendLine($"{position} {driver.ToString()}");
            position++;
        }
        string result = sb.ToString().TrimEnd();
        return result;
    }

    public void ChangeWeather(List<string> commandArgs)
    {
        string weatherType = commandArgs[0];

        bool validWeather = Enum.TryParse(typeof(Weather), weatherType, out object weatherObj);

        if (!validWeather)
        {
            throw new ArgumentException(OutputMessages.InvalidWeatherType);
        }
        Weather weather = (Weather) weatherObj;
        this.track.Weather = weather;
    }

}
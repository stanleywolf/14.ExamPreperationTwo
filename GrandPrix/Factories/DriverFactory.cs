using System;
using System.Collections.Generic;
using System.Text;

public class DriverFactory
{
    public Driver CreateDriver(string driverType, string driverName, Car car)
    {
        Driver driver = null;
        if (driverType == "Aggressive")
        {
            driver = new AggressiveDriver(driverName, car);
        }
        else if (driverType == "Endurance")
        {
            driver = new EnduranceDriver(driverName, car);
        }
        if (driver == null)
        {
            throw new ArgumentException(OutputMessages.InvalidDriverType);
        }
        return driver;
    }
}

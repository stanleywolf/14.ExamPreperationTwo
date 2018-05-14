using System;
using System.Collections.Generic;
using System.Text;

public class AggressiveDriver:Driver
{
    public AggressiveDriver(string name, Car cars) : 
        base(name, cars, 2.7)
    {      
    }

    public override double Speed => base.Speed * 1.3;
}
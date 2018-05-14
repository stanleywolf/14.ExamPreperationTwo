using System;
using System.Collections.Generic;
using System.Text;

public class TyreFactory
{
    public Tyre CreateTyre(string[] tyreArgs)
    {
        string tyreType = tyreArgs[0];
        double tyreHardness = double.Parse(tyreArgs[1]);

        Tyre tyre = null;
        if (tyreType == "Hard")
        {
            tyre = new HardTyre(tyreHardness);
        }
        else if (tyreType == "Ultrasoft")
        {
            tyre = new UltrasoftTyre(tyreHardness, double.Parse(tyreArgs[2]));
        }
        if (tyre == null)
        {
            throw new ArgumentException(OutputMessages.InvalidTyreType);
        }
        return tyre;
    }
}

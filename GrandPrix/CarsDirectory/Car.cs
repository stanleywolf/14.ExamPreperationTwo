using System;
using System.Collections.Generic;
using System.Text;

public class Car
{
    private const double MAX_FUEL = 160;

    public Car(int hp, double fuelAmount, Tyre tyre)
    {
        this.Hp = hp;
        this.FuelAmount = fuelAmount;
        this.Tyre = tyre;
    }

    public int Hp { get; }
    public Tyre Tyre { get; private set; }
    private double fuelAmount;

    public double FuelAmount
    {
        get { return this.fuelAmount; }
        protected set
        {
            if (value < 0)
            {
                throw new ArgumentException(OutputMessages.OutOfFuel);
            }
           this. fuelAmount = Math.Min(value, MAX_FUEL);
        }
    }


    internal void Refuel(double fuelAmount)
    {
        this.FuelAmount += fuelAmount;
    }

    internal void ChangeTyres(Tyre tyre)
    {
        this.Tyre = tyre;
    }


    public void CompleteLap(int trackLenght, double fuelConsumPerKm)
    {
        this.FuelAmount -= trackLenght * fuelConsumPerKm;

        this.Tyre.CompleteLap();
    }
}
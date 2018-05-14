using System;
using System.Collections.Generic;
using System.Text;

public abstract class Driver
{
    protected Driver(string name, Car car, double fuelConsumPerKm)
    {
        this.Name = name;
        this.Car = car;
        this.FuelConsumPerKm = fuelConsumPerKm;
        this.TotalTime = 0.0;
        this.IsRace = true;
    }
    public string Name { get; }
    public double TotalTime{ get; set;}
    public Car Car { get; }
    public double FuelConsumPerKm { get;}

    public virtual double Speed => (this.Car.Hp + this.Car.Tyre.Degradation) / this.Car.FuelAmount;

    public string FailureReason { get; private set; }

    public bool IsRace { get; private set; }

    private string Status => IsRace ? this.TotalTime.ToString("f3") : this.FailureReason;

    private void Box()
    {
        this.TotalTime += 20;
    }

    internal void Refuel(string[] methodArgs)
    {
        this.Box();
        double fuelAmount = double.Parse(methodArgs[0]);
        this.Car.Refuel(fuelAmount);
    }

    internal void ChangeTyres(Tyre tyre)
    {
        this.Box();
        this.Car.ChangeTyres(tyre);
    }

    internal void CompleteLap(int trackLenght)
    {
        this.TotalTime += 60 / (trackLenght / this.Speed);
        this.Car.CompleteLap(trackLenght, FuelConsumPerKm);
    }

    public void Fail(string eMessage)
    {
        this.IsRace = false;
        this.FailureReason = eMessage;
    }

    public override string ToString()
    {
        return $"{this.Name} {this.Status}";
    }
}
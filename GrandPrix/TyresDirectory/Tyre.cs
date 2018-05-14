using System;
using System.Collections.Generic;
using System.Text;

public abstract class Tyre
{
    protected Tyre(string name, double hardness)
    {
        this.Name = name;
        this.Hardness = hardness;
        this.Degradation = 100;
    }

    public string Name { get; }
    public double Hardness { get; }
    private double degradation;
    public virtual double Degradation
    {
        get { return this.degradation; }
        protected set
        {
            if (value < 0)
            {
                throw new ArgumentException(OutputMessages.BlowTyre);
            }
            this.degradation = value;
        }
    }
    
   

    public virtual void CompleteLap()
    {
        this.Degradation -= this.Hardness;
    }
}
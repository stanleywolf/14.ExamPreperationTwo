using System;


public class Program
{
    static void Main(string[] args)
    {
        int numberOfLaps = int.Parse(Console.ReadLine());
        int trackLenght = int.Parse(Console.ReadLine());

        RaceTower raceTower = new RaceTower();
        raceTower.SetTrackInfo(numberOfLaps,trackLenght);

        Engine engine = new Engine(raceTower);
        engine.Run();
    }
}
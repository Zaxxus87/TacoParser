using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        //create logger variable and file variable
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            //TODO finish logger
            logger.LogInfo("Log initialized");

            // use File.ReadAllLines(path) to grab all the lines from your csv file
            // Log an error if you get 0 lines and a warning if you get 1 line
            var lines = File.ReadAllLines(csvPath);
            //TODO finish logger
            logger.LogInfo($"Lines: {lines[0]}");

            // Create a new instance of your TacoParser class
            var parser = new TacoParser();

            // Grab an IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var locations = lines.Select(parser.Parse).ToArray();

            

            //Variables to track 2 stores and a distance
            ITrackable one = null;
            ITrackable two = null;
            double distance = 0.0;

            // using GeoCoordinatePortable to create GeoCordinate objects.
            //https://docs.microsoft.com/en-us/dotnet/api/system.device.location.geocoordinate?view=netframework-4.8
            //Distance is based on calling GetDistanceTo the 2 objects - returns distnace in meters.
            //Nested Loop - Compares all locations Time O(n^2) where n = number of distinct locations
            for (int i = 0; i < locations.Length; i++)
            {
                //Outer Location
                Point corA = new Point(locations[i].Location.Latitude, locations[i].Location.Longitude);
                for(int j = i+1; j < locations.Length; j++)
                {
                    //Inner Location
                    Point corB = new Point(locations[j].Location.Latitude, locations[j].Location.Longitude);
                    var s1 = new GeoCoordinate(corA.Latitude, corA.Longitude);
                    var s2 = new GeoCoordinate(corB.Latitude, corB.Longitude);
                    //Updates the distance and stores if next stores have greater distance
                    if (s1.GetDistanceTo(s2) > distance)
                    {
                        distance = s1.GetDistanceTo(s2);
                        one = locations[i];
                        two = locations[j];
                    }
                }
            }
            

            
            // Once you've looped through everything, you've found the two Taco Bells farthest away from each other.
            Console.WriteLine("These are the 2 Taco Bells the farthest apart.");
            Console.WriteLine($"Taco Bell #1: {one.Name}");
            Console.WriteLine($"Taco Bell #2: {two.Name}");
            distance = Math.Round(distance*0.000621371, 2); // Converts Distance to miles (round 2 places)
            Console.WriteLine($"They are {distance} miles from each other.");

        }
    }
}

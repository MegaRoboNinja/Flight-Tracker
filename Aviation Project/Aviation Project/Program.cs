using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Timers;
using DynamicData;
using ExCSS;
using NetworkSourceSimulator;
using FlightTrackerGUI;
using Mapsui.Projections;

namespace Aviation_Project
{
    internal class Program
    {
        // TCP data provider
        private static NetworkSourceSimulator.NetworkSourceSimulator NSS;
        // List of all objects
        static List<Object> allObjects = new List<Object>();
        private static object lockAllObj = new object();
        // Time for GUI update
        public static DateTime Time = DateTime.Now;
        // News Reports generator
        private static NewsGenerator NG;
        
        public static void Main(string[] args)
        {
            SetCulture(); // for . and , in ftr files
            
            // CHECK INPUT, using default values if necessary
            if (args.Length < 1)
            {
                Console.WriteLine("Paths to files containing data weren't provided. Trying hardcoded paths...");
                args = new string[1];
                args[0] = @"/Users/jansobieski/Documents/Projektowanie Obiektowe/example_data.ftr.txt";
            }
            if (args.Length < 2)
            {
                Console.WriteLine("Desired data input source wasn't provided. Trying to get data from the FTR file...");
                string[] temp = new string[2];
                temp[0] = args[0];
                temp[1] = "FTR";
                args = temp;
            }

            // SELECT DATA SOURCE
            switch (args[1])
            {
                case "FTR":
                    // Get data from FTR file
                    ReadFTR(args[0], allObjects);
                    break;
                case "NSS":
                    // start second THREAD that will simulate network source
                    NSS = new NetworkSourceSimulator.NetworkSourceSimulator(args[0], 50, 100);
                    Thread RunThread = new Thread(new ThreadStart(NSS.Run));
                    NSS.OnNewDataReady += NSS_NewDataReady;
                    RunThread.Start();
                    break;
            }
            
            // NEWS GENERATOR – initialization
            // create media:
            Radio r1 = new Radio("Radio Kwantyfikator");
            Radio r2 = new Radio("Radio Shmem");
            Television tv1 = new Television("Telewizja Abelowa");
            Television tv2 = new Television("Kanał TV-tensor");
            Newspaper np1 = new Newspaper("Gazeta Kategoryczna");
            Newspaper np2 = new Newspaper("Dziennik Politechniczny");
            List<Media> Media = new List<Media> { r1, r2, tv1, tv2, np1, np2 };
            List<IReportable> Reportables = new List<IReportable>();
            Reportables = Reportables.Concat(Airport.allAirports.Values.Cast<IReportable>().ToList()).ToList();
            Reportables = Reportables.Concat(PassengerPlane.AllPassengerPlanes.Cast<IReportable>().ToList()).ToList();
            Reportables = Reportables.Concat(CargoPlane.allCargoPlanes.Cast<IReportable>().ToList()).ToList();
            NG = new NewsGenerator(Media, Reportables);
            
            // GUI UPDATES (stage 3)
            // start the Timer in main thread
            System.Timers.Timer updateTimer = new System.Timers.Timer(1000); // 1s interval
            updateTimer.Elapsed += UpdateGUI;
            updateTimer.Start();
            
            // CONSOLE COMMANDS - start waiting in an additional thread
            Task RunnerTask = Task.Run(() => ListenToCommands());
            
            // All GUI stuff's got to be in the main thread
            Runner.Run();
        }
        static void SetCulture()
        {
            CultureInfo NewCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = NewCulture;
            CultureInfo.DefaultThreadCurrentUICulture = NewCulture;
        }
        
        public static void ReadFTR(string path, List<Object> LoadedObjects) // (STAGE 1)
        {
            // getting input
            string[] input = {};
            if (File.Exists(path))
            {
                input = File.ReadAllLines(path);
            }

            // Create all objects described in the provided data file
            foreach (string objDesc in input)
            {
                LoadedObjects.Add(ObjectFactory.CreateObject(objDesc));
            }
        }
        
        static void NSS_NewDataReady(object sender, NewDataReadyArgs args) // STAGE 2
        {
            byte[] info = NSS.GetMessageAt(args.MessageIndex).MessageBytes;

            lock (lockAllObj)
            {
                allObjects.Add(ObjectFactory.CreateObject(info));
            }
        }
        
        static void ListenToCommands()
        {
            string input;
            bool exitRequested = false;
            while (!exitRequested)
            {
                input = Console.ReadLine();

                switch (input)
                {
                    case "exit":
                        exitRequested = true;
                        break;
                    case "print":
                        lock (lockAllObj)
                        {
                            Serializer.Snapshot(allObjects);
                        }
                        break;
                    case "report":
                        string news = NG.GenerateNextNews();
                        Console.WriteLine(news);
                        break;
                    default:
                        Console.WriteLine("Whaddya mean?");
                        break;
                }
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
        
        static void UpdateGUI(object? o, ElapsedEventArgs args) // STAGE 3
        {
            List<FlightGUI> flightGUIList = new List<FlightGUI>();
            
            foreach (Flight flight in Flight.allFlights)
            {
                // I hope it's not a problem if the plane lands the next day (flies through midnight)
                // If the flight is still on the ground or has already landed
                if( Time > flight.LandingTime || Time < flight.TakeoffTime)
                    continue;
                
               // Create a FlightGUI object using an adapter
               FlightGUIAdapter flightGUI = new FlightGUIAdapter(flight);
                
                // Add it to the list that will be passed to GUI
                flightGUIList.Add(flightGUI);
            }

            FlightsGUIData data = new FlightsGUIData(flightGUIList);
            Runner.UpdateGUI(data);
            Time = DateTime.Now;
        }
    }
}
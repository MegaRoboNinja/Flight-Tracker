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
        // Data Base containing all objects
        private static DataBase DB = new();
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
                args[0] = @"/Users/miloszwysocki/Documents/Projektowanie Obiektowe/example_data.ftr.txt";
            }
            if (args.Length < 2)
            {
                Console.WriteLine("Desired data input source wasn't provided. Trying to get data from the FTR file...");
                string[] temp = new string[2];
                temp[0] = args[0];
                temp[1] = "FTR";
                args = temp;
            }

            // start second THREAD that will simulate network source
            string PathFtre = @"/Users/miloszwysocki/Developer/ProjOb_24L_01177925/Aviation Project/Aviation Project/Resources/example.ftre"; // harcoded for now
            NSS = new NetworkSourceSimulator.NetworkSourceSimulator(PathFtre, 500, 1000);
            Thread NssThread = new Thread(new ThreadStart(NSS.Run));
            
            
            // SELECT DATA SOURCE
            switch (args[1])
            {
                case "FTR":
                    // Get data from FTR file
                    List<Object> LoadedObjects = new List<Object>();
                    ReadFTR(args[0], LoadedObjects);
                    DB = new DataBase(LoadedObjects);
                    break;
                case "NSS":
                    // Add event handler that gets new data
                    NSS.OnNewDataReady += NSS_NewDataReady;
                    break;
            }
            // Add event handlers that get updates from the network source
            NSS.OnIDUpdate += DB.IDUpdate;
            NSS.OnPositionUpdate += DB.PositionUpdate;
            NSS.OnContactInfoUpdate += DB.ContactInfoUpdate;
            NssThread.Start();
            
            
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
            // TODO: avoid copying - change it to sth else
            
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

            DB.AddObject(ObjectFactory.CreateObject(info));
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
                        DB.Serialize();
                        break;
                    case "report":
                        string news;
                        while ((news = NG.GenerateNextNews()) != null)
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
            
            foreach (FlightDecorator flight in DB.GetAllFlights() ) //Flight.allFlights
            {
                // I hope it's not a problem if the plane lands the next day (flies through midnight)
                // If the flight is still on the ground or has already landed
                if( Time > flight.LandingTime || Time < flight.TakeoffTime)
                    continue;
                
               // Create a FlightGUI object using an adapter
               FlightGUIConverter flightGUI = new FlightGUIConverter(flight);
                
                // Add it to the list that will be passed to GUI
                flightGUIList.Add(flightGUI);
            }

            FlightsGUIData data = new FlightsGUIData(flightGUIList);
            Runner.UpdateGUI(data);
            Time = DateTime.Now;
        }
    }
}
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
        private static NetworkSourceSimulator.NetworkSourceSimulator NSS;
        static List<Object> allObjects = new List<Object>();
        private static object lockAllObj = new object();
        public static DateTime Time = DateTime.Now;
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
            
            // start the Timer in second thread with
            System.Timers.Timer updateTimer = new System.Timers.Timer(1000); // 1s interval
            updateTimer.Elapsed += UpdateGUI;
            updateTimer.Start();
            
            // Waiting for print or exit
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
                DateTime Landing = DateTime.Parse(flight.LandingTime);
                DateTime TakeOff = DateTime.Parse(flight.TakeoffTime);
                
                // I hope it's not a problem if the plane lands the next day (flies through midnight)
                // If the flight is still on the ground or has already landed
                if( Time > Landing || Time < TakeOff)
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
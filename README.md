 Features: 
 ---------------
- Getting input from:
    * FTR file
    * TCP server simulated by an additional thread
- Reacting to console commands: 
    * print (creates a snapshot of the data collected by the program in a json file)
    * report (prints out a newsreport to the console - demonstration of the _visitor_ design pattern)
    * exit
- Displaying ongoing flights in the GUI
- Reacting to object modification requests sent by the TCP server
  * teleport (modify position, _decorator_ design pattern used to make displaying teleported flights possible)
  * modify ID (for whatever reason)
  * change contact info (for people)
- Logging changes
  * modifications
  * new sessions
  * errors
  * a new log file for each day with distinguishable sessions
 

_The project is still being developed..._

---------------

<img width="1134" alt="Zrzut ekranu 2024-03-27 o 10 32 15" src="https://github.com/MegaRoboNinja/Flight-Tracker/assets/131467705/c94723c9-2fac-4099-b9dd-68cf26e7e642">



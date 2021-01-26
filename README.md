# TeleportVR

Teleport App WS 20/21 (including construct)

## How to install
* Download the main branch of this repository
* Open the Project with Unity 2019.3.6f1
* Add the UnityPackages X and Y from the Roboy Google Drive Folder
* Add the Animus SDK to the project (Version 2.0.13) from https://animus.cyberanimus.com/login
* For the 4 error messages in Animus scripts, double click these error messages and comment the faulty lines out (lines should look like 'using Newtonsoft')
* Add your credentials in the Script AnimusPasswordStore.cs
* Open the scene Scenes/AnimusOUI
* Enter your Robot name in the Editor by clicking on the GameObject ClientLogic in the Hierarchy window and set the name in the field 'Robot Name'
* You should now be able to use the application!

## How to use
* Press space to switch between the construct and the HUD
* Hover the laser over the connection widget and press the left mouse button to unfold the child widget
* Move the laser away from the widgets to fold in the child widget
* Hover the laser over one of the other widgets and press the left mouse button to interact with them (e.g. mute)

## Known Problems
* Going from the HUD to the construct and back again into the HUD can cause problems
* There are many bugs that have a low chance of ocurring that crash the application or lead to receiving no messages from the server. Try again in case it was not working the first time. 

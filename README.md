# TeleportVR

Teleport App WS 20/21 (including construct)

## How to install
* Download the main branch of this repository
* Open the Project with Unity 2019.3.6f1
* Add the UnityPackages "ShowRoom", "AnimusPasswordStore", "Plugins" and "OpenCV" from the Google Drive Folder https://drive.google.com/drive/folders/1b-Ez4EaQvI0c8lEPN2esZWvLFtbvm95R to the project
* Add the Animus SDK to the project (Version 2.0.13) from https://animus.cyberanimus.com/login. Make sure to uncheck UnityAnimusClient.cs and ClientLogic.cs in the import process.
* For the 4 error messages in Animus scripts, double click these error messages and comment the faulty lines out (lines should look like 'using Newtonsoft')
* In the Unity Project, add ANIMUS_USE_OPENCV to the Scripting Define Symbols in the Project settings
* Add your credentials in the Script AnimusPasswordStore.cs
* Open the scene Scenes/AnimusOUI
* Enter your Robot name in the Editor by clicking on the GameObject ClientLogic in the Hierarchy window and set the name in the field 'Robot Name'
* In the script AnimusUtilities.cs, insert the line `UnityAnimusClient.DisplayLatency(AverageLag, AverageFps);` in line 233
* You are now be able to start and use the application!

## How to use
* Press space to switch between the construct and the HUD
* Hover the laser over the connection widget and press the left mouse button to unfold the child widget
* Move the laser away from the widgets to fold in the child widget
* Hover the laser over one of the other widgets and press the left mouse button to interact with them (e.g. mute)

### Changing between Controllers and Mouse input
* Go to the scene Scenes/OUI_HUD
* In the editor in the Hierarchy window, go the object HUD_Managers/PointerManager and set the Pointer Technique to the required Input (e.g. controller for oculus touch)
* Go back to Scenes/AnimusOUI
* Under the XR Rig, make sure the HandAnchors are enabled if you want to use controllers. You can disable them if you are using the mouse as input.

## Known Problems
* Going from the HUD to the construct and back again into the HUD can cause problems
* There are many bugs that have a low chance of ocurring that crash the application or lead to receiving no messages from the server. Try again in case it was not working the first time. 

## Hardware Plugins
If you want to use hardware that was implemented for the Vive Pro Eye and XTal, but is not used with the Oculus Quest, download the UnityPackage "UnusedHardarePlugins" from the Google Drive Folder https://drive.google.com/drive/folders/1b-Ez4EaQvI0c8lEPN2esZWvLFtbvm95R. In the Unity Project, add the corresponding symbols to the Scripting Define Symbols in the Project settings:
* TOBII
* BHAPTICS
* SENSEGLOVE
* VIVESR
* UNITY_POST_PROCESSING_STACK_V2

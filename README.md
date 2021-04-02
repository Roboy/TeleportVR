# TeleportVR

Teleport App WS 20/21

## How to install
* Download the main branch of this repository
* Open the Project with Unity 2020.3.1f1
* Add the UnityPackage "UsedPlugins" from the Google Drive Folder https://drive.google.com/drive/folders/1b-Ez4EaQvI0c8lEPN2esZWvLFtbvm95R to the project. Not all of the plugins are needed, look at the Section "Plugins" for more information
* Add the Animus SDK to the project (Version 2.3.2, newer versions might fix some bugs but might not be compatible) from https://animus.cyberanimus.com/login. Make sure to uncheck UnityAnimusClient.cs and ClientLogic.cs in the import process.
* For the 4 error messages in Animus scripts, double click these error messages and comment the faulty lines out (lines should look like 'using Newtonsoft'). Might not be needed in newer Animus versions anymore.
* In the Unity Project, add ANIMUS_USE_OPENCV to the Scripting Define Symbols in the Project settings
* Add your credentials in the Script AnimusPasswordStore.cs
* Open the scene Scenes/main
* Enter your Robot name in the Editor by clicking on the GameObject ClientLogic in the Hierarchy window and set the name in the field 'Robot Name'
* In the script AnimusUtilities.cs, insert the line `UnityAnimusClient.DisplayLatency(AverageLag, AverageFps);` in line 233
* You are now be able to start and use the application!
* For further functionality and support for more hardware, look at the section "Plugins"

## How to use
* Press space or the left menu button to switch between the construct and the HUD
* Hover the right laser over the connection widget and press the left mouse button or right trigger to unfold the child widget
* Move the laser away from the widgets to fold in the child widget
* Hover the right laser over the connection widget and press the left mouse button or right trigger to interact with them (e.g. mute)
* Press the left/right grip trigger to toggle the left/right body state
* Use the joystick and Primary and secondary controller buttons to move the displays

### Changing between Controllers and Mouse input
* Go to the scene Scenes/OUI_HUD
* In the editor in the Hierarchy window, go the object HUD_Managers/PointerManager and set the Pointer Technique to the required Input (e.g. controller for oculus touch)
* Go back to Scenes/AnimusOUI
* Under the XR Rig, make sure the HandAnchors are enabled if you want to use controllers. You can disable them if you are using the mouse as input.

## Known Problems
* Going from the HUD to the construct and back again into the HUD can cause problems
* There are many bugs that have a low chance of ocurring that crash the application or lead to receiving no messages from the server. Try again in case it was not working the first time. 

## Plugins
### RosSharp
RosSharp is a Plugin that allows to use Ros in C#. It works on Windows and can work on Android. However, as it is using some dll's Animus is also using with different versions, the android app crashes when having Animus, Rossharp and Unity XR in the project. RosSharp is currently used for the Interface to the cage, and is thus needed if the cage is used. If the cage is not used, RosSharp can be removed, so that the App can run on android. To install or removed RosSharp, perform the following steps:

Installation:
* Import the "UsedPlugins" from the Google Drive Folder https://drive.google.com/drive/folders/1b-Ez4EaQvI0c8lEPN2esZWvLFtbvm95R and only check the Folder RosSharp
* Import the two UnityPackages that are stored under Assets, FaceRosMessages and RosMessages and add everything to the project, if it is not yet part of the project.
* Add ROSSHARP to the Scripting Define Symbols in the Project settings

Removal:
* Remove ROSSHARP from the Scripting Define Symbols in the Project settings
* Remove the Folders Assets/RosSharpMessages and Assets/RoboyUnityFace
* (optional) Delete the Folder Assets/OperatorUserInterface/Plugins/RosSharp

### Portals
The Portals Package can give out errors that will sometimes cause the building process to fail. It can be installed from the "UsedPackages" UnityPackage and can be removed by deleting the Folder Assets/OperatorUserInterface/Plugins/PortalsPackage

### Hardware
If you want to use hardware that was implemented for the Vive Pro Eye and XTal, but is not used with the Oculus Quest, download the UnityPackage "UnusedHardarePlugins" from the Google Drive Folder https://drive.google.com/drive/folders/1b-Ez4EaQvI0c8lEPN2esZWvLFtbvm95R. In the Unity Project, add the corresponding symbols to the Scripting Define Symbols in the Project settings:
* TOBII
* BHAPTICS
* SENSEGLOVE
* VIVESR
* UNITY_POST_PROCESSING_STACK_V2

### Others
There are a few more packages that I think can be removed without getting compile errors and installed via the UsedPlugins package, but I didn't try it. The plugins are the really old Showroom Assets/OperatorUserInterface/Plugins/PBR_Modern_room and the old Showrrom Assets/OperatorUserInterface/Plugins/ShowRoom and the current ShowRoom (will remove the visuals in the Trainings scene) Assets/OperatorUserInterface/Plugins/"_Creepy_Cat". Removing the Plugins might reduce building times.

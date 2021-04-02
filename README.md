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

## Build Instructions
The App can be run on a computer without VR, with a PC VR headset (tested with the Quest) in the Editor or in a build. Furthermore, it can run (without RosSharp) standalone on the Oculus Quest. To build it, select the plattform you want to use in the BuildSettings, select the scenes you want to have in the build (e.g. Training for PC VR and TrainingMobile) in the order 

0. Main
1. Construct
2. HUD
3. Training

For non-Quest headsets or the VR Mock, it might be needed to change the XR-Plugin Management in the Project Settings.

## How to use
The tutorial in the app explains how to use the app. I can recommend to use the Tutorial if you have a headset. If not available, here is a short overview of interactions:
* Press space or the left menu button to switch between the training and the HUD
* Hover the right laser over the connection widget and press the left mouse button or right trigger to unfold the child widget
* Click again on the widget to fold in the child widget
* Hover the right laser over the connection widget and press the left mouse button or right trigger to interact with them (e.g. mute)
* Press the left/right grip trigger to open or close the left/right body hand
* Press the left/right index trigger to move the left/right arm
* Use the joystick or arrow keys to move around the wheelchair
* Use the joystick and Primary and secondary controller buttons to move the displays after clicking the Display widget
* Show emotions by pressing the Primary or Secondary buttons on the controllers
* If rossharp is installed and enabled and the cage is running, press both grip triggers simulatneously while performing the initial pose (put down both hands next to your hips) to send the init messagee to the cage

### Changing between Controllers and Mouse input
* Go to the scene Scenes/HUD
* In the editor in the Hierarchy window, go the object HUD_Managers/PointerManager and set the Pointer Technique to the required Input (e.g. controller for oculus touch)
* Go back to Scenes/AnimusOUI
* Under the XR Rig, make sure the HandAnchors are enabled if you want to use controllers. You can disable them if you are using the mouse as input.

## Known Problems
* The animus connection is often not working. There are many problems from animus, that can crash or freeze the application. Furthermore, some modalities are sometimes not opening. Try restarting the App, Unity, your computer, the animus server or the pybullet simulation on the server computer.
* Runnning Animus, Unity XR Interaction and Rossharp on android crashes the app after a few seconds, most likely because animus and rossharp need different versions of the same dll's.
* If Unity gives a lot of compile errors that say UnityEngine.UI or similar is unknown, try to reimport the Package UnityUI. If it persists, try restarting your coomputer and look if the file Packages/UnityUI/Editor/UnityEditor.UI is showing errors in the editor.
* I think Mouse Input for the laser is currently not working in the editor. This might be because some settings are not correctly set.
* The help widget might not work as expected.
* The headset control widget is currently not implemented, since the head control implementation was changed. The functionality should be added to the WidgetInteractions script.
* The microphone functionality might not work, as it might disable the micro for Unity, not for animus. I never had a chance to test it, so I can't say if it will work or not.
* The sound widget is currently not working, as animus is not providing the utility to open and close modalities.
* When using the app with Oculus Link, you might disconnect often. This might be caused by a bad cable.
* When using Oculus Link, Unity might show a black screen, flimmer, or not start. Try restarting the app, Unity or your PC. 
* Oculus Link sometimes does not allow to reset the view. Neiither restarting the Quest nor my Laptop did help here, but after some time it usually works again.
* A ROS message is not received by either side: ROS messages should not contain Timestamps or Durations, as these Ros Types are not supported for RosSharp and ROS 2
* When entering the HUD through the portal it is currently only possible to get back into the training by simulataneously pushing the left joystick back in the HUD and pressing the system menu, as the operator respawns in the same location where he left the Training. When leaving through the portal this means he’s still in the portal and thus get’s directly back into the HUD, if he isn’t moving backwards.

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

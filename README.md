# SUMO-Unity-VR-traffic-simulation
This project connects Unity, SUMO, VR headset, e-scooter and car driving simulator to create a real-time, interactive 3D traffic simulation platform. It combines the traffic modeling strengths of SUMO with Unity’s immersive visualization and control capabilities.

![2D Junction](https://imgur.com/eLwiwDj.png)
![3D Junction](https://i.imgur.com/Quj2AWV.png)

The project extends https://github.com/DarraghMac97/Real-time-Traffic-Simulation-with-3D-Visualisation/tree/master, by integreting e-scooter riding simulator, pedestrian simulation and VR headset.
 
 ## How It Works
The simulation environment is based off an OSM file of an area of northeastern Manhattan, selected using [OpenStreetMap.org.](https://www.openstreetmap.org)
The same OSM file used to create the SUMO simulation environment is used to create the 3D environment, through importing the file
into Esri CityEngine. The environment is constructed in CityEngine, with the model being exported as an FBX file for use in Unity.
![Comparison of the SUMO and Unity Environment](https://i.imgur.com/9TRSNy1.png)
SUMO's module TraCI is used to send commands from a class in Unity to control the SUMO simulation, requesting information on other vehicles in the simulation
to display in Unity, requesting traffic light data and manipulating the position of the user-controlled vehicle and in the SUMO simulation.
The C# port of the TraCI module developed by CodingConnected, found [here, ](https://github.com/CodingConnected/CodingConnected.Traci)
is used in this project to achieve these actions.

The player vehicle can be controlled either by standard keyboard controls or by a Logitech driving wheel. The Logitech Driving Force GT wheel was used in the creation of this project.

The player e-scooter can be controlled by a real e-scooter connected to an external device (Arduino) for sensing control data.

VR headset is used to demonstrate the point of view of the player e-scooter.

## Installation 
This project was built Using Unity 2018.2.17 and SUMO 1.22.0. Later releases of each software may cause the project to function incorrectly.

Before using this project, download the following:
- [SUMO 1.22.0]
- [Unity 2018.2.17](https://unity3d.com/get-unity/download/archive)

Once both have been installed, open the scene in Unity.

### Control Options

This project has been configured to run with a Logitech Driving Wheel by default. If you wish to use keyboard controls, please do
the following:

1. In the Unity Scene, go to Edit > Project Settings > Input.
2. Change the "Type" for the Horizontal and Vertical axes to "Key or Mouse Button". Deselect "Invert" on the Vertical axis.
3. Delete WheelDrive.cs from the scripts folder. Replace it with the WheelDrive.cs file found in the OtherAssets directory.
4. Attach this script to the player-controlled car, named 0 in the Project Hierarchy.

## Running the Project
1. Open the Command Prompt and navigate to the directory in which you have downloaded the project to
2. Navigate to the SumoSimulation directory.
3. Type the following command: ```sumo-gui -c map.sumocfg --start --remote-port 4001 --step-length 0.01111111111```(adapt to your VR timestep length)
4. A SUMO-GUI window should appear. When ready to start, press the play button in the Unity Scene.

I hope you find this project useful. If you have any questions or issues using it, please contact me and I will try to resolve them as best I can.

## Works not created by me used in this project
- [Low Poly Street Pack](https://assetstore.unity.com/packages/3d/environments/urban/low-poly-street-pack-67475) by Dynamic Art. Prefabs Lampost_C, Lampost_D and Material Grass used.
- [Vehicle Tools](https://assetstore.unity.com/packages/essentials/tutorial-projects/vehicle-tools-83660) by Unity Technologies. Family Car prefab, Easy Suspension Script and Drift Camera script used. WheelDrive script used but modified to allow for Logitech Driving Force GT input
- [Logitech GamingSDK](https://assetstore.unity.com/packages/tools/integration/logitech-gaming-sdk-6630) By Logitech Gaming
- [C# port of TraCI](https://github.com/CodingConnected/CodingConnected.Traci) by @CodingConnected 

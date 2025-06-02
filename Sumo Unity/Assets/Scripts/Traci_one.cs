using System;
using System.Collections;
using System.Collections.Generic;
using CodingConnected.TraCI.NET;
using CodingConnected.TraCI.NET.Types;
using UnityEngine;

using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CodingConnected.TraCI.NET.Commands;
using Color = UnityEngine.Color;
using Object = System.Object;
using System.Threading;

public class Traci_one : MonoBehaviour
{
    
    public Light ttLight;
    public GameObject tLight;
    public GameObject egoVehicle;
    public Material roadmat;
    public List<string> vehicleidlist;
    public List<GameObject> carlist;
    public List<GameObject> pedlist;
    public GameObject NPCVehicle;
    public TraCIClient client;
    private List<string> tlightids;
    private int phaser;
    private List<traLights> listy;
    public Dictionary<string, List<traLights>> dicti;
    public GameObject eScooter;
    public GameObject NPCeScooter;
    public List<string> pedestrianIDList = new List<string> { 
    "Ch_0CP00F00", 
    "DemoPlayer00", 
    "Ch_0CP00F00B", 
    "Ch_0CP00F00Bn" 
    };
    private StreamWriter csvWriter; // >>> ADDED FOR DATA LOGGING <<<
    private bool headerWritten = false; // >>> ADDED FOR DATA LOGGING <<<

    // Start is called before the first frame update
    
    void Start()
    {
       

        
        client = new TraCIClient();
        try
        {
            Debug.Log("Trying to connect to SUMO...");
            client.Connect("127.0.0.1", 4001); //connects to SUMO simulation
            client.Control.SimStep();
            Debug.Log("Connected to SUMO.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to connect or communicate with SUMO: " + e.Message);
        }

        // >>> ADDED FOR DATA LOGGING <<<
        csvWriter = new StreamWriter(@"H:\sumo_unity\Real-time-Traffic-Simulation-with-3D-Visualisation_zhiwei_ped_vr\DataExtraction\output_data.csv", false);
        // string filePath = "output_data.csv";
        // csvWriter = new StreamWriter(filePath, false); // overwrite each time
    
        // tlightids = client.TrafficLight.GetIdList().Content; //all traffic light IDs in the simulation
       
        client.Gui.TrackVehicle("View #0", "escooter");
        client.Gui.SetZoom("View #0", 1200); //tracking the player vehicle
        
        
        
        // createTLS();


        // client.Control.SimStep();
        // client.Control.SimStep();//making sure vehicle is loaded in
        
        // Initialize ego vehicle
        // try
        // {

        client.Vehicle.SetSpeed("veh", 0); //stops SUMO controlling player vehicle
        var location = client.Vehicle.GetPosition("veh").Content;
        var angle = client.Vehicle.GetAngle("veh").Content;
        //puts the player vehicle in the starting position
        egoVehicle.transform.position = new Vector3((float) location.X, 1.33f, (float) location.Y);
        egoVehicle.transform.rotation = Quaternion.Euler(0, (float)angle, 0);
        carlist.Add(egoVehicle);
        // client.Vehicle.SetLaneChangeMode("veh", 0);
        // }
        // catch
        // {
        //     Debug.LogWarning("Ego vehicle not found in SUMO.");
        // }


        // Initialize eScooter
        // try
        // {
        // Stop SUMO from controlling eScooter
        client.Vehicle.SetSpeed("escooter", 0);
        // Get initial position from SUMO
        var scooterLocation = client.Vehicle.GetPosition("escooter").Content;
        var scooterAngle = client.Vehicle.GetAngle("escooter").Content;

        // Place it in Unity
        eScooter.transform.position = new Vector3((float) scooterLocation.X, 1.2f, (float) scooterLocation.Y);
        eScooter.transform.rotation = Quaternion.Euler(0, (float)scooterAngle, 0);
        carlist.Add(eScooter);
        //  }
        // catch
        // {
        //     Debug.LogWarning("eScooter not found in SUMO.");
        // }   

        // initialize pedestrians
        foreach (string pedID in pedestrianIDList)
        {
            // Optional: set speed to 0 (only if desired, otherwise remove this line)
            client.Person.SetSpeed(pedID, 0);

            // Get initial position and angle from SUMO
            var pedLocation = client.Person.GetPosition(pedID).Content;
            var pedAngle = client.Person.GetAngle(pedID).Content;

            // Instantiate or find the GameObject for this pedestrian (assumes it's already in the scene)
            GameObject pedestrianObject = GameObject.Find(pedID); // Make sure pedestrian GameObject names match the IDs

            // Set position and rotation in Unity
            pedestrianObject.transform.position = new Vector3((float)pedLocation.X, 0.5f, (float)pedLocation.Y);  // Adjust Y if needed
            pedestrianObject.transform.rotation = Quaternion.Euler(0, (float)pedAngle, 0);

            pedlist.Add(pedestrianObject); // Add to the same list if used for tracking all moving agents

        }

    }

    private void OnApplicationQuit()
    {
        client.Control.Close();//terminates the connection upon ending of the scene
        // >>> ADDED FOR DATA LOGGING <<<
        if (csvWriter != null)
        {
            csvWriter.Close();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        // Step SUMO first so it’s aligned with Unity's update
        client.Control.SimStep();

        var newvehicles = client.Simulation.GetDepartedIDList("").Content; //new vehicles this step
        // Debug.Log("Checking newvehicles: " + string.Join(", ", newvehicles));
        var vehiclesleft = client.Simulation.GetArrivedIDList("").Content; //vehicles that have left the simulation

        //if any vehicles have left the scene they are removed and destroyed
        for (int j = 0; j < vehiclesleft.Count; j++)
        {
            GameObject toremove = GameObject.Find(vehiclesleft[j]);
         
            if (toremove)
            {
                
                carlist.Remove(toremove);
                Destroy(toremove);
                

            }

        }

        // // road and lane client are on, necessary for manipulation in SUMO
        // var road = client.Vehicle.GetRoadID(egoVehicle.name).Content;
        // // Debug.Log($"Checking road: {road}");
        // var lane = client.Vehicle.GetLaneIndex(egoVehicle.name).Content;
        
        // Update ego vehicle position to SUMO
        // try
        // {
        var road = client.Vehicle.GetRoadID(egoVehicle.name).Content;
        var lane = client.Vehicle.GetLaneIndex(egoVehicle.name).Content;
        client.Vehicle.MoveToXY("veh", road, lane, (double) egoVehicle.transform.position.x,
        (double) egoVehicle.transform.position.z, (double) egoVehicle.transform.eulerAngles.y, 2);
        // }
        // catch
        // {
        //     Debug.LogWarning("Ego vehicle not available to update in SUMO.");
        // }

        // Update eScooter position from Unity to SUMO
        // try
        // if (newvehicles.Contains(eScooter.name) || carlist.Contains(eScooter.name))
        // {
        var eScooterRoad = client.Vehicle.GetRoadID(eScooter.name).Content;
        // Debug.Log($"Checking eScooterRoad: {eScooterRoad}");
        var eScooterLane = client.Vehicle.GetLaneIndex(eScooter.name).Content;
        client.Vehicle.MoveToXY("escooter", eScooterRoad, eScooterLane,
        (double)eScooter.transform.position.x,
        (double)eScooter.transform.position.z,
        (double)eScooter.transform.eulerAngles.y,
        2);
        // }
        // else
        // catch
        // {
        //     Debug.LogWarning("Ego vehicle not available to update in SUMO.");
        // }
        // Debug.Log($"Checking eScooter.name: {eScooter.name}");
        // var eScooterRoad = client.Vehicle.GetRoadID(eScooter.name).Content;
        // Debug.Log($"Checking eScooterRoad: {eScooterRoad}");
        // var eScooterLane = client.Vehicle.GetLaneIndex(eScooter.name).Content;
        /*
         * Updates the ego-vehicle's position in the SUMO scene
         * @params id: ego-vehicle's name in the SUMO simulation
         * @params road: current edge the vehicle is on in the SUMO simulation
         * @params lane: current lane number the ego vehicle is on
         * @params xPosition: X-axis position of the ego-vehicle in the Unity scene
         * @params yPosition: Z-axis position of the ego-vehicle in the Unity scene
         * @params angle: The angle that the ego vehicle is facing at
         * @params keepRoute: maps the ego-vehicle to the exact X-Y position in the SUMO simulation
         */
        // client.Vehicle.MoveToXY("veh", road, lane, (double) egoVehicle.transform.position.x,
        //     (double) egoVehicle.transform.position.z, (double) egoVehicle.transform.eulerAngles.y, 2);
        

        foreach (string pedID in pedestrianIDList)
        {
            // Get current road ID and lane index from SUMO for the pedestrian
            var pedRoadID = client.Person.GetRoadID(pedID).Content;
            // var pedLaneIndex = client.Person.GetLaneIndex(pedID).Content;

            // Get the GameObject of the pedestrian in Unity
            GameObject pedObject = GameObject.Find(pedID);
            // Move the pedestrian in SUMO to match Unity position and orientation
            client.Person.MoveToXY(
                pedID,                            // ID of the pedestrian
                pedRoadID,                        // Road ID from SUMO
                // pedLaneIndex,                     // Lane index
                (double)pedObject.transform.position.x,   // X position
                (double)pedObject.transform.position.z,   // Y in SUMO is Unity Z
                (double)pedObject.transform.eulerAngles.y, // Angle (heading)
                0                                 // Keep route flag (0 = ignore route, 2 = keep route)
            );
        }

        for (int carid = 0; carid <carlist.Count;carid++)
        {
            // client.Vehicle.SetLaneChangeMode(carlist[carid].name, 512);

            if (carlist[carid].name != "escooter" && carlist[carid].name != "veh")
            {
                var carpos = client.Vehicle.GetPosition(carlist[carid].name).Content; //gets position of NPC vehicle
                if (carlist[carid].name.StartsWith("escooter_"))
                {
                    carlist[carid].transform.position = new Vector3((float) carpos.X, 1.2f, (float) carpos.Y);
                }
                else
                {
                    carlist[carid].transform.position = new Vector3((float) carpos.X, 1.33f, (float) carpos.Y);
                }
                

                
                
                var newangle = client.Vehicle.GetAngle(carlist[carid].name).Content; //gets angle of NPC vehicle
                carlist[carid].transform.rotation = Quaternion.Euler(0f, (float) newangle, 0f);
            }    
                
                // Debug.Log("Checking carlist: " + string.Join(", ", carlist));
        }

        for (int i = 0; i < newvehicles.Count; i++)
            {
                // client.Vehicle.SetLaneChangeMode(newvehicles[i], 512);
   
                var newcarposition = client.Vehicle.GetPosition(newvehicles[i]).Content; //gets position of new vehicle
                
                // Debug.Log($"Checking newvehicles[i]: {newvehicles[i]}");

                // GameObject newcar = GameObject.Instantiate(NPCeScooter); //creates the vehicle GameObject //NPCVehicle
                
                GameObject newcar = null; // Declare outside if-else
        
                if (newvehicles[i].StartsWith("escooter_"))
                {
                    newcar = GameObject.Instantiate(NPCeScooter);//GameObject
                    newcar.transform.position = new Vector3((float) newcarposition.X, 1.2f,
                    (float) newcarposition.Y);//maps its initial position
                }
                else
                {
                    newcar = GameObject.Instantiate(NPCVehicle);//GameObject 
                    newcar.transform.position = new Vector3((float) newcarposition.X, 1.33f,
                    (float) newcarposition.Y);//maps its initial position
                }
                
                var newangle = client.Vehicle.GetAngle(newvehicles[i]).Content;
                newcar.transform.rotation = Quaternion.Euler(0f, (float) newangle, 0f);//maps initial angle
                
                newcar.name = newvehicles[i];//object name the same as SUMO simulation version
                carlist.Add(newcar);
               
            }
        // var currentphase = client.TrafficLight.GetCurrentPhase("42443658");// 42442618 42443658
        // client.Control.SimStep();
        //checks traffic light's phase to see if it has changed
        // if (client.TrafficLight.GetCurrentPhase("42443658").Content != currentphase.Content)
        // {
        //     changeTrafficLights();
        // }

        try
        {
            // client.SimulationStep();

            // >>> ADDED FOR DATA LOGGING <<<
            double simTime = client.Simulation.GetCurrentTime("").Content / 1000.0;

            if (!headerWritten)
            {
                csvWriter.WriteLine("Time,Type,ID,X,Y,Speed,Angle,Fuel,Electricity,CO2");
                headerWritten = true;
            }

            for (int carid = 0; carid <carlist.Count;carid++)
            {
                var pos = client.Vehicle.GetPosition(carlist[carid].name).Content;
                double speed = client.Vehicle.GetSpeed(carlist[carid].name).Content;
                // double accel = client.Vehicle.GetAcceleration(carlist[carid].name).Content;
                double angle = client.Vehicle.GetAngle(carlist[carid].name).Content;
                double fuel = client.Vehicle.GetFuelConsumption(carlist[carid].name).Content;
                double electricity = client.Vehicle.GetElectricityConsumption(carlist[carid].name).Content;
                double co2 = client.Vehicle.GetCO2Emission(carlist[carid].name).Content;

                csvWriter.WriteLine($"{simTime},Vehicle,{carlist[carid].name},{pos.X},{pos.Y},{speed},{angle},{fuel},{electricity},{co2}");
            }

            foreach (string pedID in pedestrianIDList)
            {
                var pos = client.Person.GetPosition(pedID).Content;
                double speed = client.Person.GetSpeed(pedID).Content; // if speed not available for person
                double angle = client.Person.GetAngle(pedID).Content; // person may not have this info
                // double accel = client.Person.GetAcceleration(pedID).Content;
                double fuel = 0.0;
                double electricity = 0.0;
                double co2 = 0.0;

                csvWriter.WriteLine($"{simTime},Person,{pedID},{pos.X},{pos.Y},{speed},{angle},{fuel},{electricity},{co2}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("TraCI Error: " + e.Message);
        }

        var vehicleIDs = client.Vehicle.GetIdList().Content;
        Debug.Log("Vehicles in SUMO: " + string.Join(", ", vehicleIDs));


    }

    
    
    //Changes traffic lights to their next phase
    void changeTrafficLights()
    {
        for (int i = 0; i < tlightids.Count; i++)
        {
            //for each traffic light value of a junctions name
            for (int k = 0; k < dicti[tlightids[i]].Count; k++)
            {
				
                var newstate = client.TrafficLight.GetState(tlightids[i]).Content;
                var lightchange = dicti[tlightids[i]][k]; //retrieves traffic light object from list
                
                var chartochange = newstate[lightchange.index].ToString();//traffic lights new state based on its index
                if (lightchange.isdual == false)
                {
                    lightchange.changeState(chartochange.ToLower());//single traffic light change
                }
                else
                {
                    lightchange.changeStateDual(chartochange.ToLower());//dual traffic light change
                }

            }
        }

    }
    
    
    // Creates the TLS for of all junctions in the SUMO simulation
    
    void createTLS()
    {
        dicti = new Dictionary<string, List<traLights>>(); //the dictionary to hold each junctions traffic lights
        for (int ids = 0; ids < tlightids.Count; ids++)	
        {
            string tlsId = tlightids[ids];
            Debug.Log($"Checking TLS ID: {tlsId}");

            List<traLights> traLightslist = new List<traLights>();
            int numconnections = 0;  //The index that represents the traffic light's state value
            var newjunction = GameObject.Find(tlightids[ids]); //the traffic light junction
            
            // Check if the GameObject exists in Unity
            if (newjunction == null)
            {
                Debug.LogWarning($"Traffic light GameObject with ID '{tlightids[ids]}' not found in the Unity scene.");
                continue; // skip to the next traffic light
            }

            for (int i = 0; i < newjunction.transform.childCount; i++)
            {
                bool isdouble = false;
                var trafficlight = newjunction.transform.GetChild(i);//the next traffic light in the junction
                //Checks if the traffic light has more than 3 lights
                if (trafficlight.childCount > 3)
                {
                    isdouble = true;
                }
                Light[] newlights = trafficlight.GetComponentsInChildren<Light>();//list of light objects belonging to
                                                                                  //the traffic light
               //Creation of the traffic light object, with its junction name, list of lights, index in the junction
               //and if it is a single or dual traffic light
                traLights newtraLights = new traLights(newjunction.name, newlights, numconnections, isdouble);
                traLightslist.Add(newtraLights);
                //try
                var controlledLinksResult = client.TrafficLight.GetControlledLinks(tlsId);

                if (controlledLinksResult?.Content?.Links == null)
                {
                    Debug.LogError($"No controlled links returned from SUMO for TLS ID: {tlsId}. Skipping...");
                    continue;
                }
                int linkcount = controlledLinksResult.Content.NumberOfSignals;
                var laneconnections = controlledLinksResult.Content.Links;
                Debug.Log($"TLS ID {tlsId} has {linkcount} controlled links.");

                // var linkcount = client.TrafficLight.GetControlledLinks(newjunction.name).Content.NumberOfSignals;
                // var laneconnections = client.TrafficLight.GetControlledLinks(newjunction.name).Content.Links;
                if (numconnections+1 < linkcount - 1)
                {
                    numconnections++;//index increases
                    //increases index value until the next lane is reached
                    while ((laneconnections[numconnections][0] == laneconnections[numconnections - 1][0] || isdouble) &&
                           numconnections < linkcount - 1)
                    {
						//if the next lane is reached but the traffic light is a dual lane, continue until the
						//lane after is reached
                        if (laneconnections[numconnections][0] != laneconnections[numconnections - 1][0] && isdouble)
                        {
                            isdouble = false;
                        }
                        numconnections++;
                    }
                }
            }
            dicti.Add(newjunction.name, traLightslist);
        }
        changeTrafficLights(); //displays the initial state of all traffic lights
        print(550.4 + + 0.54 +776.4);
    }
    

   

    




}

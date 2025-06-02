using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class zhiwei : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM4", 115200);
    // Set the port (can be found on the right bottom corner of the Arduino interface)
    // and the baud rate (9600) as indicated in Arduino.

    int buttonState = 0;

    void Start()
    {
        stream.Open(); // Open the Serial Stream.
    }

    void Update()
    {
    	if (stream.IsOpen) // Check if serial port is open before reading
    	{
        	try
        	{
            		string value = stream.ReadLine();
            		buttonState = int.Parse(value);
        	}
        	catch (System.Exception e)
        	{
            		Debug.LogWarning("Serial Read Error: " + e.Message);
        	}
    	}
    	else
    	{
        	Debug.LogWarning("Serial port is closed! Trying to reopen...");
        	try
        	{
            		stream.Open(); // Attempt to reopen
        	}
        	catch (System.Exception e)
        	{
            		Debug.LogWarning("Failed to open port: " + e.Message);
        	}
    	}
    }

    void OnGUI()
    {
        string newString = "Connected: " + buttonState;
        GUI.Label(new Rect(10, 10, 300, 100), newString); // Display new values.
    }
}
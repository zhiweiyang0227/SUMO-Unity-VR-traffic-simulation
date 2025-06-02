using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; 
using TMPro;

public class WheelColliderScooterController : MonoBehaviour {
	//SerialPort serialPort = new SerialPort("COM4", 115200); // Replace with your port
	public string portName = "COM4"; // Replace with your Arduino port
    	private SerialPort arduinoStream;
	
	public float maxAcc = 3f;  // Maximum scooter speed (based on real e-scooter speed ~36km/h)
    	public float maxBrake = 5f; // How quickly the scooter slows down
    	public float maxTurn = 45f; // Turning speed 
    	float steerSpeed = 30f;        // How fast the scooter can turn handlebars (degrees/sec)
    	float currentSteeringAngle = 0f;


	private float accVal = 0f;   // Acceleration input
    	private float brakeVal = 0; // Brake input
    	private float steerVal = 0; // Steering input
    	private float speed;
    	private Vector3 lastVelocity;
    	public TextMeshProUGUI EscooterDataText;  // Assign this in Inspector

    	private Rigidbody rb;
	public WheelCollider frontWheelCollider;
    	public WheelCollider rearWheelCollider;

    	private string serialBuffer = ""; // New buffer for serial parsing
    	private string latestData = "";   // Holds the latest valid line of data

    	private float smoothTorque = 0f;
		private float smoothBrake = 0f;

	// Use this for initialization
	void Start ()
	{
        	
        	rb = GetComponent<Rigidbody>();
			lastVelocity = rb.velocity;
			rb.centerOfMass = new Vector3(0f, -0.5f, 0f); // lowers the CoM to help balance
			WheelFrictionCurve sidewaysFriction = frontWheelCollider.sidewaysFriction;
			sidewaysFriction.stiffness = 0.5f;
			frontWheelCollider.sidewaysFriction = sidewaysFriction;
			rearWheelCollider.sidewaysFriction = sidewaysFriction;
    		// Rigidbody setup
    		//rb.mass = 30f;
    		//rb.drag = 0.2f;
    		//rb.angularDrag = 1f;
		//rb.useGravity = true;
    		//rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Optional for scooter stability
		// Serial port setup        	
		OpenSerialPort();
		Application.targetFrameRate = 60; // Set to a fixed frame rate, e.g., 60 FPS
    	}

    	void OpenSerialPort()
    	{
        	try
        	{
            		if (arduinoStream == null)
            		{
                		arduinoStream = new SerialPort(portName, 115200);
            		}

            		if (!arduinoStream.IsOpen)
            		{
                		arduinoStream.Open();
                		arduinoStream.ReadTimeout = 50; // Set a timeout to prevent freezes
            		}
        	}
        	catch (System.Exception e)
        	{
            		Debug.LogWarning("Failed to open port: " + e.Message);
        	}
    	}


    	void FixedUpdate()
    	{
        	if (arduinoStream != null && arduinoStream.IsOpen)
        	{
            		try
            		{
				// Read all available data
                		serialBuffer += arduinoStream.ReadExisting();

                		// Process all full lines
                		string[] lines = serialBuffer.Split('\n');
                		for (int i = 0; i < lines.Length - 1; i++) {
                    		latestData = lines[i].Trim();
                		}

                		// Keep the last incomplete line
                		serialBuffer = lines[lines.Length - 1];

                		if (!string.IsNullOrEmpty(latestData)) 
				{
                    		ParseArduinoData(latestData);
                		}
            		}
            		
			catch (System.Exception e)
            		{
                		Debug.LogWarning("Error reading from Arduino: " + e.Message);
            		}
        	}
		else
        	{
            		OpenSerialPort(); // Attempt to reopen if disconnected
        	}
        
		MoveScooter();

    		// Print speed
    		//float speed = rb.velocity.magnitude;
    		//float speedKmh = speed * 3.6f;
    		//Debug.Log($"Speed: {speed:F2} m/s");//({speedKmh:F1} km/h)
    	}

	void ParseArduinoData(string data)
    	{
        	// Expected format: "accVal;brakeVal;steerVal"
        	string[] values = data.Split(';');
        	if (values.Length == 3)
        	{
           		float.TryParse(values[0], out accVal);  // Acceleration (0 to 1)
            		float.TryParse(values[1], out brakeVal); // Brake (0 to 1)
            		float.TryParse(values[2], out steerVal); // Steering (-1 to 1)
			
			Debug.Log($"Parsed: Acc={accVal}, Brake={brakeVal}, Steer={steerVal}");
        	}
    	}

    	void MoveScooter()
    	{
        	float targetTorque = accVal * maxAcc * 30f;
    		float brakeAmount = brakeVal * maxBrake * 30f; // Convert brake input into force
    		// Smooth torque changes
		    smoothTorque = Mathf.Lerp(smoothTorque, targetTorque, 5f * Time.fixedDeltaTime);
		    


        	if (accVal * maxAcc > 0.01f) // If accelerating
        	{
            		//rb.AddForce(force, ForceMode.Acceleration);
        	rb.drag = 0f; // Reset drag while moving
			rearWheelCollider.brakeTorque = 0f;
			rearWheelCollider.motorTorque = smoothTorque;
			frontWheelCollider.brakeTorque = 0f;
			frontWheelCollider.motorTorque = 0f;
			
			//Debug.Log($"Applying force: {moveAcc}");
        	}
		else

        	{
            		//rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeAmount * Time.fixedDeltaTime);
			//rb.drag = brakeVal * maxBrake;
        	rb.drag = 0f; // Reset drag while moving
			rearWheelCollider.motorTorque = 0f;
			rearWheelCollider.brakeTorque = brakeAmount;
			frontWheelCollider.motorTorque = 0f;
			frontWheelCollider.brakeTorque = brakeAmount;
			
        	}
        	

        	// Adjust steering based on normalized input
        	//float turnAmount = steerVal * maxTurn * Time.fixedDeltaTime;
        	//Quaternion turnRotation = Quaternion.Euler(0, 0, -turnAmount);  
		//rb.MoveRotation(rb.rotation * turnRotation);
		float targetSteeringAngle = steerVal * maxTurn;
		// Smoothly rotate towards target angle at a limited speed (degrees/sec)
		currentSteeringAngle = Mathf.MoveTowards(
		    currentSteeringAngle,
		    targetSteeringAngle,                        // Negative to match wheel direction
		    steerSpeed * Time.fixedDeltaTime             // Max step per frame
		);
		frontWheelCollider.steerAngle = -currentSteeringAngle;

		//Debug.Log($"motorTorque={rearWheelCollider.motorTorque}, brakeTorque={rearWheelCollider.brakeTorque}, steerAngle={rb.rotation * turnRotation}");//frontWheelCollider.steerAngle
		Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);              // <--- NEW
	    speed = horizontalVelocity.magnitude;
	    Vector3 lastHorizontalVelocity = new Vector3(lastVelocity.x, 0f, lastVelocity.z);        // <--- NEW
	    float acceleration = (horizontalVelocity - lastHorizontalVelocity).magnitude / Time.fixedDeltaTime; // <--- CHANGED
		//EscooterSpeedText.text = "Speed: " + speed.ToString("F1") + " m/s";
		lastVelocity = rb.velocity;
		EscooterDataText.text = 
            		"Speed: " + speed.ToString("F1") + " m/s\n" +
            		"Acc:   " + acceleration.ToString("F1") + " m/s²\n" +
    				"Steer: " + targetSteeringAngle.ToString("F1") + "°";		
		Debug.Log($"motorTorque={rearWheelCollider.motorTorque}, brakeTorque={rearWheelCollider.brakeTorque}, steerAngle={frontWheelCollider.steerAngle}");
		Debug.DrawRay(transform.position, rb.velocity.normalized * 2f, Color.green); //vibrate
		Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
		// Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);  // Forward direction
		Debug.Log("Steering Angle: " + frontWheelCollider.steerAngle + " | Velocity: " + rb.velocity.magnitude);

		//Debug.Log($"Force: {force}, Drag: {rb.drag}, Steering Turn: {turnAmount}");
    	}

    	void OnApplicationQuit()
    	{
        	if (arduinoStream != null && arduinoStream.IsOpen)
        	{
            	arduinoStream.Close();
		arduinoStream.Dispose();
        	}
    	}
    	//Displays the Speed that the ego-vehicle is currently travelling at
		private void OnGUI()
		{
			float mstokmh = (int)( speed * 3.6F);
			
			GUI.Box (new Rect (0,0,100,50), mstokmh.ToString() + " km/h");
		}
}

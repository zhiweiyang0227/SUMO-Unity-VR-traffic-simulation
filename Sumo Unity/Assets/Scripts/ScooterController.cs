using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; 
using TMPro;

public class ScooterController : MonoBehaviour {
	//SerialPort serialPort = new SerialPort("COM4", 115200); // Replace with your port
	public string portName = "COM4"; // Replace with your Arduino port
    	private SerialPort arduinoStream;
	
	public float maxAcc = 4f;  // Maximum scooter speed (based on real e-scooter speed ~36km/h)
    	public float maxBrake = 6f; // How quickly the scooter slows down
    	public float maxTurn = 45f; // Turning speed 
    	float steerSpeed = 45f;        // How fast the scooter can turn handlebars (degrees/sec)
    	float currentSteeringAngle = 0f;

	
	private float accVal = 0f;   // Acceleration input
    	private float brakeVal = 0; // Brake input
    	private float steerVal = 0; // Steering input
    	private float speed;

	public TextMeshProUGUI EscooterDataText;  // Assign this in Inspector

    	private Rigidbody rb;
	private Vector3 lastVelocity;
	//public WheelCollider frontWheelCollider;
    	//public WheelCollider rearWheelCollider;

    	private string serialBuffer = ""; // New buffer for serial parsing
    	private string latestData = "";   // Holds the latest valid line of data

	// Use this for initialization
	void Start ()
	{
        	rb = GetComponent<Rigidbody>();
		lastVelocity = rb.velocity;

    		// Rigidbody setup
    		//rb.mass = 30f;
		rb.centerOfMass = new Vector3(0, -0.5f, 0); // lower and centered
    		rb.drag = 0f;//1
    		rb.angularDrag = 0f;//2
		rb.useGravity = true;
		rb.WakeUp();
    		//rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Optional for scooter stability
    		// Ensure proper setup of rear wheel collider
    		//JointSpring spring = rearWheelCollider.suspensionSpring;
    		//spring.spring = 20000f;
    		//spring.damper = 4500f;
    		//spring.targetPosition = 0.5f;
    		//rearWheelCollider.suspensionSpring = spring;

    		//rearWheelCollider.suspensionDistance = 0.1f;
    		//rearWheelCollider.radius = 0.2f; // Make sure this matches your visual wheel height / 2

    		//WheelFrictionCurve forwardFriction = rearWheelCollider.forwardFriction;
    		//forwardFriction.extremumSlip = 0.4f;
    		//forwardFriction.extremumValue = 1f;
    		//forwardFriction.asymptoteSlip = 0.8f;
    		//forwardFriction.asymptoteValue = 0.5f;
    		//forwardFriction.stiffness = 1.5f;
    		//rearWheelCollider.forwardFriction = forwardFriction;

    		//WheelFrictionCurve sidewaysFriction = rearWheelCollider.sidewaysFriction;
    		//sidewaysFriction.stiffness = 1.5f;
    		//rearWheelCollider.sidewaysFriction = sidewaysFriction;

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
		//rearWheelCollider.motorTorque = 0f;
		//rearWheelCollider.brakeTorque = 0f;
        float moveAcc = accVal * maxAcc; // Convert 0 to 1 input into speed
        // Vector3 desiredForward = rb.transform.forward.normalized; // NEW
	    // Vector3 force = desiredForward * moveAcc;
	    Vector3 force = transform.forward * moveAcc * 2;
	    Vector3 applyPoint = transform.position + transform.up * -0.5f; // below center
		//float brakeAmount = brakeVal * maxBrake; // Convert brake input into force
		//Vector3 forward = transform.forward.normalized;
		//Debug.DrawRay(transform.position, forward * 5, Color.red, 1.0f);

    	if (accVal > 0.001f) // If accelerating
    	{
        	//rb.AddForce(force, ForceMode.Force);
			rb.drag = 0f; // Reset drag while moving
			rb.AddForceAtPosition(force, applyPoint, ForceMode.Acceleration);
			//rearWheelCollider.brakeTorque = 0f;
			//rearWheelCollider.motorTorque = moveAcc;
			

		//Debug.Log($"Applying force: {moveAcc}");
    	}
		else

    	{
        //rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeAmount * Time.fixedDeltaTime);
		rb.drag = brakeVal * maxBrake;
		//rearWheelCollider.motorTorque = 0f;
		//rearWheelCollider.brakeTorque = brakeAmount;
    	}
        	

    	// Adjust steering based on normalized input
		// float steeringAngle = steerVal * maxTurn;  // in degrees
		// 🧮 Get target steering angle from input
		float targetSteeringAngle = steerVal * maxTurn;
		// ⏱️ Smoothly move current steering angle toward target
		currentSteeringAngle = Mathf.MoveTowards(currentSteeringAngle, targetSteeringAngle, steerSpeed * Time.fixedDeltaTime);
    	// float turnAmount = currentSteeringAngle * Time.fixedDeltaTime;

    	// float turnAmount = steerVal * maxTurn * Time.fixedDeltaTime;
		Quaternion turnRotation = Quaternion.Euler(0f, -currentSteeringAngle * Time.fixedDeltaTime, 0f);
		rb.MoveRotation(rb.rotation * turnRotation);
		
		//frontWheelCollider.steerAngle = steerVal * maxTurn;
		//Debug.Log($"motorTorque={rearWheelCollider.motorTorque}, brakeTorque={rearWheelCollider.brakeTorque}, steerAngle={rb.rotation * turnRotation}");//frontWheelCollider.steerAngle
		// speed = rb.velocity.magnitude; 
		// float acceleration = (rb.velocity - lastVelocity).magnitude / Time.deltaTime;

		// === DISPLAY SPEED AND ACCELERATION ===
		Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);              // <--- NEW
	    speed = horizontalVelocity.magnitude;
	    Vector3 lastHorizontalVelocity = new Vector3(lastVelocity.x, 0f, lastVelocity.z);        // <--- NEW
	    float acceleration = (horizontalVelocity - lastHorizontalVelocity).magnitude / Time.deltaTime; // <--- CHANGED
		//EscooterSpeedText.text = "Speed: " + speed.ToString("F1") + " m/s";
		lastVelocity = rb.velocity;
		EscooterDataText.text = 
            		"Speed: " + speed.ToString("F1") + " m/s\n" +
            		"Acc:   " + acceleration.ToString("F1") + " m/s²\n" +
    				"Steer: " + targetSteeringAngle.ToString("F1") + "°";
		Debug.Log($"Velocity: {horizontalVelocity}, Speed: {speed}, Force: {force}, Drag: {rb.drag}, Steering Turn: {turnRotation}");
		
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

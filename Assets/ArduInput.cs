using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class ArduInput : MonoBehaviour {
	
	public float speed; 

	private Rigidbody rb;
	private Transform tf; 

	private float fyaw = 0.0f;
	private float fpitch = 0.0f;
	private float froll = 0.0f;
	private float yawOffset = 0.0f;

	public float AmbientSpeed = 100.0f;
	public float RotationSpeed = 200.0f;

	//private string port = "/dev/cu.usbserial-A900GTFE"; 
	//private int baud = 57600; 
	
	SerialPort sp = new SerialPort("/dev/cu.usbserial-A900GTFE", 57600);
	
	// Use this for initialization
	void Start () {
		sp.Open();
		sp.ReadTimeout = 1; 
		rb = GetComponent<Rigidbody>();
		tf = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		//amountToMove = speed * Time.deltaTime; 
		
		if (sp.IsOpen) {
			try {
				readFloat(sp);
				//yaw = readFloat(sp);
				//pitch = readFloat(sp);
				//roll = readFloat(sp);
				//print ("YPR:" + yaw + " | " + pitch + " | " + roll); //Yaw



				//tf.rotation.x = yaw;

			} catch {

			}
		}
		
	}
	void FixedUpdate(){
		moveObject(); 
	}

	//hello world
	
	void readFloat(SerialPort serial) {
		// Convert from little endian (Razor) to big endian (Java) and interpret as float
		//return Float.intBitsToFloat(sp.read() + (sp.read() << 8) + (sp.read() << 16) + (sp.read() << 24));
		char[] delimiterChars = { ' ', ',', '.', ':', '\t', '=' };
		string text = serial.ReadLine(); 
		//print (text);
		string[] words = text.Split(delimiterChars);
		fyaw = Convert.ToSingle (words [1]);
		fpitch = Convert.ToSingle (words [2]); 
		froll = Convert.ToSingle (words [3]); 


	}

	void moveObject()
	{

		//rb.rotation = Quaternion.Euler( pitch, yaw, roll );

		// For relative orientation
		//tf.rotation *= Quaternion.Euler( 0, yaw, 0 );
		// Rotation. Makes the platform moves smoothly towards a target (the value from the arduino card)
		int ipitch = (int)fpitch; 
		int iroll = (int)froll; 
		int iyaw = (int)fyaw;

		Quaternion target = Quaternion.Euler(ipitch, iroll, iyaw);
		rb.rotation = Quaternion.Slerp(rb.rotation, target, Time.deltaTime * 2.0f);

		Debug.Log (ipitch + " | " + iroll + " | " + iyaw);

	}
}

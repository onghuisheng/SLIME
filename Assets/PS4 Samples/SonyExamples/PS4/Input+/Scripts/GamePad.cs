using UnityEngine;
using UnityEngine.PS4;
using System;
using System.Collections;

public class GamePad : MonoBehaviour
{
	// Custom class for holding all the gamepad sprites
	[System.Serializable]
	public class PS4GamePad
	{
		public SpriteRenderer thumbstick_left;
		public SpriteRenderer thumbstick_right;

		public SpriteRenderer cross;
		public SpriteRenderer circle;
		public SpriteRenderer triangle;
		public SpriteRenderer square;

		public SpriteRenderer dpad_down;
		public SpriteRenderer dpad_right;
		public SpriteRenderer dpad_up;
		public SpriteRenderer dpad_left;

		public SpriteRenderer L1;
		public SpriteRenderer L2;
		public SpriteRenderer R1;
		public SpriteRenderer R2;

		public SpriteRenderer lightbar;
		public SpriteRenderer options;
		public SpriteRenderer speaker;
		public SpriteRenderer touchpad;
		public Transform gyro;
		public TextMesh text;
		public Light light;
	}
	public PS4GamePad gamePad;

	public int playerId = -1;
	public Transform[] touches;
	public Color inputOn = Color.white;
	public Color inputOff = Color.grey;

	private int stickID;
	private Color lightbarColour;
	private bool hasSetupGamepad = false;
	private PS4Input.LoggedInUser loggedInUser;
	private PS4Input.ConnectionType connectionType;

	// Touchpad variables
	private int touchNum, touch0x, touch0y, touch0id, touch1x, touch1y, touch1id;
	private int touchResolutionX, touchResolutionY, analogDeadZoneLeft, analogDeadZoneRight;
	private float touchPixelDensity;

	// Volume sampling variables
	private int qSamples = 1024; // array size
	private float rmsValue = 0f; // sound level - RMS
	private float[] samples = new float[1024]; // audio samples

	void Start()
	{
		// Stick ID is the player ID + 1
		stickID = playerId + 1;

		ToggleGamePad(false);
	}

	void Update()
	{
		if(PS4Input.PadIsConnected(playerId))
		{
			// Set the gamepad to the start values for the player
			if(!hasSetupGamepad)
				ToggleGamePad(true);

			// Handle each part individually
			Touchpad();
			Thumbsticks();
			InputButtons();
			DPadButtons();
			TriggerShoulderButtons();
			Lightbar();
			Speaker();

			// Options button is on its own, so we'll do it here
			if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button7", true)))
			{
				
			
				gamePad.options.color = inputOn;

				// Reset the gyro orientation and lightbar to default
				PS4Input.PadResetOrientation(playerId);
				PS4Input.PadResetLightBar(playerId);
				lightbarColour = GetPlayerColor(PS4Input.GetUsersDetails(playerId).color);
			}
			else
				gamePad.options.color = inputOff;

			// Make the gyro rotate to match the physical controller
			gamePad.gyro.localEulerAngles = new Vector3(-PS4Input.PadGetLastOrientation(playerId).x,
			                                            -PS4Input.PadGetLastOrientation(playerId).y,
			                                            PS4Input.PadGetLastOrientation(playerId).z) * 100;
														
			// rebuild the username everyframe, in case it's changed due to PSN access
			gamePad.text.text = PS4Input.RefreshUsersDetails(playerId).userName + "\n(" + connectionType + ")";
														
		}
		else if(hasSetupGamepad)
			ToggleGamePad(false);
	}

	// Toggle the gamepad between connected and disconnected states
	void ToggleGamePad(bool active)
	{
		if(active)
		{
			// Set the lightbar colour to the start/default value
			lightbarColour = GetPlayerColor(PS4Input.GetUsersDetails(playerId).color);

			// Set 3D Text to whoever's using the pad
			loggedInUser = PS4Input.RefreshUsersDetails(playerId);
			gamePad.text.text = loggedInUser.userName + "\n(" + connectionType + ")";

			// Reset and show the gyro
			gamePad.gyro.localRotation = Quaternion.identity;
			gamePad.gyro.gameObject.SetActive(true);

			hasSetupGamepad = true;
		}
		else
		{
			// Hide the touches
			touches[0].gameObject.SetActive(false);
			touches[1].gameObject.SetActive(false);
			
			// Set the lightbar to a default colour
			lightbarColour = Color.gray;
			gamePad.lightbar.color = lightbarColour;
			gamePad.light.color = Color.black;

			// Set the 3D Text to show the pad is disconnected
			gamePad.text.text = "Disconnected";

			// Hide the gyro
			gamePad.gyro.gameObject.SetActive(false);
			
			hasSetupGamepad = false;
		}
	}
	
	void Touchpad()
	{
		PS4Input.GetPadControllerInformation(playerId, out touchPixelDensity, out touchResolutionX, out touchResolutionY, out analogDeadZoneLeft, out analogDeadZoneRight, out connectionType);
		PS4Input.GetLastTouchData(playerId, out touchNum, out touch0x, out touch0y, out touch0id, out touch1x, out touch1y, out touch1id);

		// Show and move around up to 2 touch inputs
		if (touchNum > 0)
		{
			float xPos = 0;
			float yPos = 0;

			// Touch 1
			if (touch0x > 0 || touch0y > 0)
			{
				if (!touches[0].gameObject.activeSelf)
					touches[0].gameObject.SetActive(true);

				xPos = (3.57f / touchResolutionX) * touch0x;
				yPos = (1.35f / touchResolutionY) * touch0y;

				touches[0].localPosition = new Vector3(xPos, -yPos, 1);
			}
			else if (touches[0].gameObject.activeSelf)
				touches[0].gameObject.SetActive(false);

			//Touch 2
			if (touchNum > 1 && (touch1x > 0 || touch1y > 0))
			{
				if (!touches[1].gameObject.activeSelf)
					touches[1].gameObject.SetActive(true);

				xPos = (3.57f / touchResolutionX) * touch1x;
				yPos = (1.35f / touchResolutionY) * touch1y;

				touches[1].localPosition = new Vector3(xPos, -yPos, 1);
			}
			else if (touches[1].gameObject.activeSelf)
				touches[1].gameObject.SetActive(false);
		}
		else if (touches[0].gameObject.activeSelf || touches[1].gameObject.activeSelf)
		{
			touches[0].gameObject.SetActive(false);
			touches[1].gameObject.SetActive(false);
		}

		// Make the touchpad light up and play audio if it's pushed down
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button6", true)))
		{
			gamePad.touchpad.color = inputOn;
			TouchpadAudio(touchResolutionX, touchResolutionY, touch0x, touch0y);
		}
		else
		{
			gamePad.touchpad.color = inputOff;
			GetComponent<AudioSource>().Stop();
		}
	}

	// Change the pitch and volume of an audio source, via the inputs of 
	// the touchpad, and play it through the controller speaker
	void TouchpadAudio(int maxX, int maxY, int posX, int posY)
	{
		Rect touchInput = new Rect();
		touchInput.width = maxX;
		touchInput.height = maxY;
		touchInput.x = posX;
		touchInput.y = posY;

		float xMod = touchInput.x / touchInput.width;
		float yMod = touchInput.y / touchInput.height;

		GetComponent<AudioSource>().pitch = xMod + 0.5f;
		GetComponent<AudioSource>().volume = 1f - yMod;

		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().PlayOnDualShock4(loggedInUser.userId);
	}

	void Thumbsticks()
	{
		// Move the thumbsticks around
		gamePad.thumbstick_left.transform.localPosition = new Vector3(0.4f*Input.GetAxis("leftstick" + stickID + "horizontal"),
		                                                              -0.4f*Input.GetAxis("leftstick" + stickID + "vertical"),
		                                                              0);

		gamePad.thumbstick_right.transform.localPosition = new Vector3(0.4f*Input.GetAxis("rightstick" + stickID + "horizontal"),
		                                                               -0.4f*Input.GetAxis("rightstick" + stickID + "vertical"),
		                                                               0);

		// Make the thumbsticks light up when pressed
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button8", true)))
			gamePad.thumbstick_left.color = inputOn;
		else
			gamePad.thumbstick_left.color = inputOff;
		
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button9", true)))
			gamePad.thumbstick_right.color = inputOn;
		else
			gamePad.thumbstick_right.color = inputOff;
	}

	// Make the Cross, Circle, Triangle and Square buttons light up when pressed
	void InputButtons()
	{
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button0", true)))
			gamePad.cross.color = inputOn;
		else
			gamePad.cross.color = inputOff;
		
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button1", true)))
			gamePad.circle.color = inputOn;
		else
			gamePad.circle.color = inputOff;

		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button2", true)))
			gamePad.square.color = inputOn;
		else
			gamePad.square.color = inputOff;

		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button3", true)))
			gamePad.triangle.color = inputOn;
		else
			gamePad.triangle.color = inputOff;
	}

	// Make the DPad directions light up when pressed
	void DPadButtons()
	{
		if(Input.GetAxis("dpad" + stickID + "_horizontal") > 0)
			gamePad.dpad_right.color = inputOn;
		else
			gamePad.dpad_right.color = inputOff;

		if(Input.GetAxis("dpad" + stickID + "_horizontal") < 0)
			gamePad.dpad_left.color = inputOn;
		else
			gamePad.dpad_left.color = inputOff;

		if(Input.GetAxis("dpad" + stickID + "_vertical") > 0)
			gamePad.dpad_up.color = inputOn;
		else
			gamePad.dpad_up.color = inputOff;

		if(Input.GetAxis("dpad" + stickID + "_vertical") < 0)
			gamePad.dpad_down.color = inputOn;
		else
			gamePad.dpad_down.color = inputOff;
	}
	
	void TriggerShoulderButtons()
	{
		// Make the triggers light up based on how "pulled" they are
		if(Input.GetAxis("joystick" + stickID + "_left_trigger") != 0)
			gamePad.L2.color = inputOff+(inputOn*Input.GetAxis("joystick" + stickID + "_left_trigger"));
		else
			gamePad.L2.color = inputOff;
		
		if(Input.GetAxis("joystick" + stickID + "_right_trigger") != 0)
			gamePad.R2.color = inputOff+(inputOn*-Input.GetAxis("joystick" + stickID + "_right_trigger"));
		else
			gamePad.R2.color = inputOff;

		// Make the shoulders light up when pressed
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button4", true)))
			gamePad.L1.color = inputOn;
		else
			gamePad.L1.color = inputOff;
		
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button5", true)))
			gamePad.R1.color = inputOn;
		else
			gamePad.R1.color = inputOff;
	}

	void Lightbar()
	{
		// Make the lightbar change colour when we hold down buttons
		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button0", true)))
			lightbarColour = Color.Lerp (lightbarColour, Color.blue, Time.deltaTime * 4f);

		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button1", true)))
			lightbarColour = Color.Lerp (lightbarColour, Color.red, Time.deltaTime * 4f);

		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button2", true)))
			lightbarColour = Color.Lerp (lightbarColour, Color.magenta, Time.deltaTime * 4f);

		if(Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + stickID + "Button3", true)))
			lightbarColour = Color.Lerp (lightbarColour, Color.green, Time.deltaTime * 4f);

		// Set the lightbar sprite and the physical lightbar change to the current colour
		gamePad.lightbar.color = lightbarColour;
		gamePad.light.color = lightbarColour;
		PS4Input.PadSetLightBar(playerId,
		                        Mathf.RoundToInt(lightbarColour.r * 255),
		                        Mathf.RoundToInt(lightbarColour.g * 255),
		                        Mathf.RoundToInt(lightbarColour.b * 255));
	}

	// Get the volume being played in-game, and make the speaker light up based on the volume
	void Speaker()
	{
		GetVolume();
		gamePad.speaker.color = (Color.white*rmsValue) + (Color.white*0.25f);
	}

	string PadConnectionType(int connectionType)
	{
		switch(connectionType)
		{
		case 0:
			return "Local";
		case 1:
			return "Remote Vita";
		case 2:
			return "Remote DS4";
		default:
			return "Invalid connection type";
		}
	}

	// Get a usable Color from an int
	Color GetPlayerColor(int colorId)
	{
		switch (colorId)
		{
		case 0:
			return Color.blue;
		case 1:
			return Color.red;
		case 2:
			return Color.green;
		case 3:
			return Color.magenta;
		default:
			return Color.black;
		}
	}

	//Get the volume from an attached audio source component
	void GetVolume()
	{
		if(GetComponent<AudioSource>().time>0f)
		{
			GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
			int i;
			float sum = 0f;
			
			for(i=0; i < qSamples; i++)
				sum += samples[i]*samples[i]; // sum squared samples
			
			rmsValue = Mathf.Sqrt(sum/qSamples); // rms = square root of average

			rmsValue *= GetComponent<AudioSource>().volume;
		}
		else
			rmsValue = 0f;
	}
}

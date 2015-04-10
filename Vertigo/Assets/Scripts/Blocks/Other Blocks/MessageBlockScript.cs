using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBlockScript : MonoBehaviour {
	
	// Use this for initialization
	private bool viewed, generate, finishedMessage;
	private double messageBoxLength;
	private double messageBoxHeight;
	private string message;
	public float DEFAULTTEXTSPEED = .05f;
	public float FASTERTEXTSPEED = .001f;
	public string messageTotal = "Default message, change this in the scene editor plz.";
	public string messageTitle = "Default Title";
	private float letterPause;

	Rect windowRect = new Rect(80,80,200,200);

	void Start () 
	{
		letterPause = DEFAULTTEXTSPEED;
		viewed = generate = finishedMessage = false;
		messageBoxLength = Screen.width / 2;
		messageBoxHeight = Screen.height / 2;
		//StartCoroutine (typeText ());
	}

	IEnumerator typeText()
	{
		foreach (char c in messageTotal.ToCharArray ()) 
		{
			message += c;
			//yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
		finishedMessage = true;
	}

	void OnGUI()
	{
		if (generate == true) 
		{
			GUI.backgroundColor = Color.black;
			GUI.contentColor = Color.white;
			windowRect = GUI.Window (1, new Rect (130, 35, (float)messageBoxLength, (float)messageBoxHeight), generateWindow, messageTitle);
		}
	}

	void generateWindow(int windowID)
	{
		GUILayout.Label (message);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!viewed) 
		{
			StartCoroutine (typeText ());
			generate = !generate;
			//Disable movement
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.disableMovement ();
			viewed = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other)
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1") || (Input.touchCount != 0))
		{
			if(finishedMessage)
			{
				generate = false;
				GameObject character = GameObject.Find ("Character");
				CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
				script.enableMovement ();
			}
			else
			{
				if(letterPause == FASTERTEXTSPEED)
				{
					StopAllCoroutines();
					message = messageTotal;
					finishedMessage = true;
				}
				else
				{
					letterPause = FASTERTEXTSPEED;
				}
			}
		}	
	}
}

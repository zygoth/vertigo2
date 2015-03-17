using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBlockScript : MonoBehaviour {
	
	// Use this for initialization
	private bool viewed, generate, finishedMessage;
	private double messageBoxLength;
	private double messageBoxHeight;
	private string message, messageTotal, messageTitle;
	private float letterPause = 0.05f;

	Rect windowRect = new Rect(80,80,200,200);

	void Start () 
	{
		viewed = generate = finishedMessage = false;
		messageBoxLength = Screen.width / 2;
		messageBoxHeight = Screen.height / 2;
		messageTitle = "Incoming transmission...";
		messageTotal = "This is a test message to see what happens when I have a really long sentence as the message. Will it stay all on the same line in the header or will it go into the body of the window?";
		StartCoroutine (typeText ());
	}

	IEnumerator typeText()
	{
		foreach (char c in messageTotal.ToCharArray ()) 
		{
			message += c;
			yield return 0;
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
		if(Input.GetKeyDown (KeyCode.Return))
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
				StopAllCoroutines();
				message = messageTotal;
				finishedMessage = true;
			}
		}	
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBlockScript : MonoBehaviour {
	
	// Use this for initialization
	private bool viewed;
	private Dialogue dialogue;

	void Start () 
	{
		
		dialogue = GetComponent<Dialogue> ();
		
		//Binding to start event, you can trigger code logic when a chat starts.
		dialogue.OnStart += () => {
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.disableMovement ();
		};

		//Binding to end event, you can trigger code logic when chat ends.
		dialogue.OnEnd += () => {
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.enableMovement ();
		};

		//binding to Event event, you can triger code on certain conditions i.e "screenshake", "playsound", "takedamage"
		dialogue.OnEvent += (EventName) => {		

			if(EventName == "ShakeScreen"){
				//Shake my screen using my own code here....
			}
			//Sample usage of the event system
			//if(EventName == "ScreenShake") { ScreenShake() }
		};

		//binding to OnDialogueChange, you can trigger code when the player changes the message.
		dialogue.OnDialogueChanged += (DialogueItem) => {
			
		};
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!viewed) 
		{
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.disableMovement ();
			viewed = true;
			dialogue.Play();
		}
	}
	
	void OnTriggerStay2D(Collider2D other)
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
		{
			dialogue.Next();			
		}	
	}
}

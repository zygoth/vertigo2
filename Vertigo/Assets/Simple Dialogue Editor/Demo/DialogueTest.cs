using UnityEngine;
using System.Collections;

public class DialogueTest : MonoBehaviour {

	private Dialogue myDialogue;
	private bool _canplay = true;

	void Start(){

		myDialogue = GetComponent<Dialogue> ();

		//Binding to start event, you can trigger code logic when a chat starts.
		myDialogue.OnStart += () => {
			_canplay = false;
		};

		//Binding to end event, you can trigger code logic when chat ends.
		myDialogue.OnEnd += () => {
			_canplay = true;
		};

		//binding to Event event, you can triger code on certain conditions i.e "screenshake", "playsound", "takedamage"
		myDialogue.OnEvent += (EventName) => {		
			Debug.Log("Respond To Event: " + EventName);

			if(EventName == "ShakeScreen"){
				//Shake my screen using my own code here....
			}
			//Sample usage of the event system
			//if(EventName == "ScreenShake") { ScreenShake() }
		};

		//binding to OnDialogueChange, you can trigger code when the player changes the message.
		myDialogue.OnDialogueChanged += (DialogueItem) => {
			Debug.Log("Message Changed: " + DialogueItem.name);
		};

	}

	void OnGUI(){

		if (_canplay) {
			if (GUI.Button (new Rect (10, 10, 100, 42), "Start Chat")) {
				myDialogue.Play ();
			}
		}

		if (!_canplay) {
			if (GUI.Button (new Rect (120, 10, 70, 42), "Next")) {
				myDialogue.Next ();
			}
		}

	}

}

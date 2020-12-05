using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	The purpose of this class is to receive requests from other objects that want story actions to occur. 
	Then it emits events to let callers know when the requested action has completed.
	Most requests will come from the dialogue system, but other things may want to cause these actions as well.

	Some examples of things that this class handles:

		Displaying a screen-filling cutscene image
		Moving NPCs and the player mid cutscene
		Emoting or story-related animations of NPCs or the player
		Screen shake
		Cutscene-related music changes and sound effects
		Waiting for a set amount of time
*/
public class CutsceneManager : MonoBehaviour
{
	public GameObject explosionPrefab;

	public event OnEventHandler OnEvent;
	public delegate void OnEventHandler(string EventName);

	public void doAction(string action) {
		Debug.Log("whee we did an action: " + action);

		if(action.Equals("explode")) { // just for testing
			Instantiate (explosionPrefab, GameObject.Find("Character").transform.position + new Vector3(0, 3, 0), Quaternion.identity);
			Instantiate (explosionPrefab, GameObject.Find("Character").transform.position + new Vector3(2, 3, 0), Quaternion.identity);
			Instantiate (explosionPrefab, GameObject.Find("Character").transform.position + new Vector3(-2, 3, 0), Quaternion.identity);
			Instantiate (explosionPrefab, GameObject.Find("Character").transform.position + new Vector3(0, 5, 0), Quaternion.identity);
			SoundManager.playSound("Block Break");
		}

		if(action.Equals("disablePlayerMovement")) {
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.disableMovement ();
		}

		if(action.Equals("enablePlayerMovement")){
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.enableMovement ();
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

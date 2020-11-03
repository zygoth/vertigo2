using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewDialogue : MonoBehaviour {

	//List of Serializeable dialogue entities
	public float MessageSpeed = 0.06f;
	public float MinimumWaitSpeed = 0.1f;
	public List<string> dialogueLines = new List<string>();
	public TextAsset cutsceneScript;

	//Trigger for message progression
	private bool _next = false;

	//Events for hooking
	public event OnStartHandler OnStart;
	public delegate void OnStartHandler();
	public event OnEndHandler OnEnd;
	public delegate void OnEndHandler();
	public event OnEventHandler OnEvent;
	public delegate void OnEventHandler(string EventName);
	public event OnDialogueChangedHandler OnDialogueChanged;
	public delegate void OnDialogueChangedHandler(string DialogueItem);
	

	//Initiate the playback engine
	public void Play(){

		string[] lines = cutsceneScript.text.Split('\n');

		foreach(string line in lines){
			if(line.Length > 1) {
				dialogueLines.Add(line);
			}			
		}
		

		GameObject UI_Hud = GameObject.Find("SDH_UI_GROUP");
		if (UI_Hud) {
			UI_Hud.GetComponent<Animator> ().SetTrigger ("IN");

			OnStart ();
			StartCoroutine (RunDialogue ());
		} else {
			Debug.LogError("ERROR: SDE Reqires HUD Prefab In Scene");
		}
	}

	public void Next(){
		_next = true;
	}

	//Coroutine entrypoint for dialogue
	IEnumerator RunDialogue(){

		//Iterate each Dialogue Entity and yield to the display function.
		foreach(string DI in dialogueLines){

			OnDialogueChanged(DI);
			yield return StartCoroutine(DisplayDialogue(DI));

		};

		OnEnd ();
		GameObject UI_Hud = GameObject.Find("SDH_UI_GROUP");
		UI_Hud.GetComponent<Animator> ().SetTrigger ("OUT");

	}

	IEnumerator DisplayDialogue(string DI){

		//Flip the trigger for progression
		_next = false;

		Text txtName = GameObject.Find ("SDH_txtName").GetComponent<Text>();
		Text txtMessage = GameObject.Find ("SDH_txtMessage").GetComponent<Text>();
		AudioSource sfx = GameObject.Find ("SDH_UI_GROUP").GetComponent<AudioSource>();

		txtName.text = "testname";
		for (int i = 0; i <= DI.Length; i++) {
			txtMessage.text = DI.Substring(0, i);
			sfx.Play();
			yield return new WaitForSeconds(MessageSpeed);
		};

		//if (DI.event_key.Length > 0)
		//	OnEvent (DI.event_key);

		if (_next) //if the player pressed next before the last message was finished rendering.... wait a bit before progressing
			yield return new WaitForSeconds (MinimumWaitSpeed);

		yield return StartCoroutine(WaitForNext());

	}

	IEnumerator WaitForNext(){
		while (! _next)
			yield return null;
	}

}

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

	//Special characters that can be in a line of dialogue.
	private char[] specialChars = {'[', ']'};

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

	IEnumerator DisplayDialogue(string line){

		//Flip the trigger for progression
		_next = false;

		yield return StartCoroutine(displayLine(line));		

		//if (DI.event_key.Length > 0)
		//	OnEvent (DI.event_key);

		if (_next) //if the player pressed next before the last message was finished rendering.... wait a bit before progressing
			yield return new WaitForSeconds (MinimumWaitSpeed);

		yield return StartCoroutine(WaitForNext());

	}

	IEnumerator displayLine(string line) {


		Text txtName = GameObject.Find ("SDH_txtName").GetComponent<Text>();
		txtName.supportRichText = true;
		Text txtMessage = GameObject.Find ("SDH_txtMessage").GetComponent<Text>();
		AudioSource sfx = GameObject.Find ("SDH_UI_GROUP").GetComponent<AudioSource>();
		
		ParsingContext context = new ParsingContext(line, "", 0, txtName, txtMessage);

		while(context.index < context.rawLine.Length) {			
			parseUntilNextCharacter(context);
			sfx.Play();
			yield return new WaitForSeconds(MessageSpeed);
		};
	}

	/*
		Parses the line until a new character is added to the displayed string. This can mean just adding a single character
		(when displaying text normally), or parsing the speaker (beginning of the line), or parsing commands to do things
		like speed up or slow down the text, emit an event (for some cutscene action to occur for example), or to handle
		rich text elements like bold or colors.
	*/
	void parseUntilNextCharacter(ParsingContext context) {
		
		bool characterParsed = false;
		while(characterParsed == false) {
			characterParsed = parseToken(context);
		}
	}

	/*
		The core of the parsing algorithm. Parses the next token available at context.index.
	*/
	bool parseToken(ParsingContext context) {

		// We just started the line, so parse out the speaker's name and put it in the Name spot
		if(context.index == 0) {
			parseNameAndEmotion(context);
			return false;;	
		}

		// check for special chars that signal rich text, events, or speed changes
		foreach(char c in specialChars) {
			if(context.rawLine[context.index] == c) {
				parseSpecialChar(context);
				return false;
			}
		}

		// Finally if we made it here then we reached a displayable character
		context.displayedText += context.rawLine[context.index];
		context.index++;
		context.txtMessage.text = context.displayedText;
		return true;
	}

	void parseSpecialChar(ParsingContext context) {
		//TODO deal with custom commands and rich text
	}

	void parseNameAndEmotion(ParsingContext context) {
		int colonIndex = context.rawLine.IndexOf(":");
		int parenIndex = context.rawLine.IndexOf("(");
		
		string name;
		if(parenIndex != -1 && parenIndex < colonIndex) { // an emotion was included, so strip that out of the name
			name = context.rawLine.Substring(0, parenIndex);
		}
		else {
			name = context.rawLine.Substring(0, colonIndex);
		}

		context.txtName.text = name;
		
		context.index = colonIndex + 2;

		// change the display picture based on the name and emotion of the character.
		changePicture(context.rawLine.Substring(0, colonIndex));
	}

	void changePicture(string nameAndEmotion) {
		// TODO figure out how to put a picture in the right place. the text position will need to be changed to give space for this.
	}

	IEnumerator WaitForNext(){
		while (! _next)
			yield return null;
	}

	private class ParsingContext {
		public string rawLine;
		public string displayedText;
		public int index;
		public Text txtName; 
		public Text txtMessage;

		public ParsingContext(string rawLine, string displayedText, int index, Text txtName, Text txtMessage) {
			this.rawLine = rawLine;
			this.displayedText = displayedText;
			this.index = index;
			this.txtName = txtName;
			this.txtMessage = txtMessage;
		}
	}
}



using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewDialogue : MonoBehaviour, ICutsceneEventListener{

	//List of Serializeable dialogue entities
	public float MessageSpeed = 0.05f;
	public float MinimumWaitSpeed = 0.1f;
	public List<string> dialogueLines = new List<string>();
	public TextAsset cutsceneScript;

	//Trigger for message progression
	private bool _next = false;

	// Keep track of if the hud is visible or not right now
	private bool hudVisible = false;

	//Special characters that can be in a line of dialogue.
	private char[] specialChars = {'[', '<',};

	private string waitForEvent = "";

	private const float DEFAULT_MESSAGE_SPEED = 0.06f;

	private ProfilePictureMap profileMap = new ProfilePictureMap();
	private List<string> currentRichTextTags = new List<string>();
	private CutsceneManager cutsceneManager;

	//Events for hooking
	//public event OnStartHandler OnStart;
	//public delegate void OnStartHandler();
	//public event OnEndHandler OnEnd;
	//public delegate void OnEndHandler();

	//kept to remember how to bind to events
		/*dialogue.OnStart += () => {
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.disableMovement ();
		};*/
	

	//Initiate the playback engine
	public void Play(){

		cutsceneManager = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
		cutsceneManager.doAction("disablePlayerMovement", this);

		string[] lines = cutsceneScript.text.Split('\n');

		if(lines.Length > 0 && lines[0] != "[hidehud]") {
			showHUD();
		}
		foreach(string line in lines){
			if(line.Length > 1) {
				dialogueLines.Add(line);
			}			
		}		

		//OnStart ();
		StartCoroutine (RunDialogue ());
	}

	public void Next(){
		_next = true;
	}

	private void showHUD() {
		GameObject UI_Hud = GameObject.Find("SDH_UI_GROUP");
		if (UI_Hud) {
			UI_Hud.GetComponent<Animator> ().SetTrigger ("IN");
			hudVisible = true;
		} else {
			Debug.LogError("ERROR: SDE Reqires HUD Prefab In Scene");
			throw new System.ArgumentException("ERROR: SDE Reqires HUD Prefab In Scene");
		}
	}

	public void OnEvent(string eventName) {
		if(waitForEvent.Equals(eventName)) {
			Debug.Log(waitForEvent + " =? " + eventName);
			waitForEvent = "";
		}
	}

	private void hideHUD() {
		GameObject UI_Hud = GameObject.Find("SDH_UI_GROUP");
		if (UI_Hud) {
			UI_Hud.GetComponent<Animator> ().SetTrigger ("OUT");
			hudVisible = false;
		} else {
			Debug.LogError("ERROR: SDE Reqires HUD Prefab In Scene");
			throw new System.ArgumentException("ERROR: SDE Reqires HUD Prefab In Scene");
		}		
	}

	//Coroutine entrypoint for dialogue
	IEnumerator RunDialogue(){

		//Iterate each Dialogue Entity and yield to the display function.
		foreach(string DI in dialogueLines){
			yield return StartCoroutine(DisplayDialogue(DI));
		};

		cutsceneManager.doAction("enablePlayerMovement", this);
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

		while(context.index < context.rawLine.Length | !waitForEvent.Equals("")) {			

			if(waitForEvent.Equals("")) {
				parseUntilNextCharacter(context);
				sfx.Play();	
			}
			
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
		Returns true if we parsed a displayable character, false if we parsed something else.
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

		int richTextEndTagsLength = 0;
		foreach(string s in currentRichTextTags) {
			richTextEndTagsLength += 3 + s.Length;
		}

		string textWithoutClosingTags = context.displayedText.Substring(0, context.displayedText.Length - richTextEndTagsLength);
		string closingTags = context.displayedText.Substring(context.displayedText.Length - richTextEndTagsLength);

		// we must insert the new character before the rich text closing tags
		context.displayedText = textWithoutClosingTags + context.rawLine[context.index] + closingTags;
		context.index++;
		context.txtMessage.text = context.displayedText;
		return true;
	}

	void parseSpecialChar(ParsingContext context) {
		if(context.rawLine[context.index] == '[') {
			parseCommand(context);
		}
		else if(context.rawLine[context.index] == '<') {
			parseRichText(context);
		}
	}

	/*
		Parses a rich text element. These are not displayed to the user but are included in the displayed string
		(Unity parses them and uses them to modify text style). We need to edit the displayed string such that
		the style is applied correctly as the string is typed, and there is never an orphaned tag.
	*/
	void parseRichText(ParsingContext context) {
		int closingBracketIndex = context.rawLine.IndexOf(">", context.index);
		string tagName = context.rawLine.Substring(context.index + 1, (closingBracketIndex - context.index) - 1);

		if(context.rawLine[context.index + 1] == '/') { // this is a closing tag, so stop applying this style
			int richTextEndTagsLength = 0;
			foreach(string s in currentRichTextTags) {
				richTextEndTagsLength += 3 + s.Length;
			}

			string textWithoutClosingTags = context.displayedText.Substring(0, context.displayedText.Length - richTextEndTagsLength);
			currentRichTextTags.Remove(tagName.Substring(1)); // chop off the "/" since this is a closing tag

			foreach(string s in currentRichTextTags) {
				textWithoutClosingTags += "</" + s + ">";
				context.displayedText = textWithoutClosingTags;
			}
		}
		else { // this is an opening tag, so add the tags to the displayed text
			currentRichTextTags.Add(tagName);

			context.displayedText += "<" + tagName + ">" + "</" + tagName + ">";	
		}		

		context.index = closingBracketIndex + 1;
	}

	void parseCommand(ParsingContext context) {
		// grab the part of the string starting with the command character and ending at the end of the string
		string lastPart = context.rawLine.Substring(context.index, context.rawLine.Length - context.index);
		// get the index of where the command ends
		int endIndex = lastPart.IndexOf(']');

		if(endIndex == -1) {
			throw new System.ArgumentException("Command in script had opening but no ending bracket");
		}
		// grab the command without the "[]"
		string commandString = lastPart.Substring(1, endIndex - 1);

		// update the index so that we don't display this command to the user
		context.index = context.index + endIndex + 1;

		// Now check if it's a command that we recognize and if so do the proper thing

		if(commandString.Contains("textspeed=")) {
			MessageSpeed = (1/float.Parse(commandString.Split('=')[1])) * DEFAULT_MESSAGE_SPEED;
			return;
		}

		if(commandString.Contains("action=")) {
			cutsceneManager.doAction(commandString.Split('=')[1], this);
			return;
		}

		if(commandString.Contains("forceproceed")) {
			Next();
			return;
		}		

		if(commandString.Contains("waitforevent=")) {
			Debug.Log(commandString);
			waitForEvent = commandString.Split('=')[1];
			return;
		}
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

	/* 
		Change the profile picture based on who is talking. Also move the text to be centered
		if this is flavor text and has no speaker.
	*/
	void changePicture(string nameAndEmotion) {
		GameObject UI_Profile = GameObject.Find("SDH_profile");

		if(nameAndEmotion.Equals("nobody")) { // check to see if it's an object (no speaker)
			
			// make profile pic invisible
			UI_Profile.GetComponent<RawImage>().enabled = false;

			// move text over to fill the box
			Text txtMessage = GameObject.Find ("SDH_txtMessage").GetComponent<Text>();
			Vector2 anchorMin = txtMessage.GetComponent<RectTransform>().anchorMin;
			txtMessage.GetComponent<RectTransform>().anchorMin = new Vector2(-.133f,anchorMin.y);
		}
		else { // otherwise, update the profile picture based on who is talking.
			
			// make profile pic visible
			UI_Profile.GetComponent<RawImage>().enabled = true;

			// move text over to give space for profile
			Text txtMessage = GameObject.Find ("SDH_txtMessage").GetComponent<Text>();
			Vector2 anchorMin = txtMessage.GetComponent<RectTransform>().anchorMin;
			txtMessage.GetComponent<RectTransform>().anchorMin = new Vector2(0,anchorMin.y);
			
			// change the profile picture based off the mapping in the profile map
			try {
				UI_Profile.GetComponent<RawImage>().texture = Resources.Load<Texture>(profileMap.getProfilePath(nameAndEmotion));
			}
			catch (KeyNotFoundException) // display a default image if there is no mapping for this person
			{
				UI_Profile.GetComponent<RawImage>().texture = Resources.Load<Texture>(profileMap.getProfilePath("default"));
			    Debug.LogError("changePicture: unknown name + emotion read from script: " + nameAndEmotion);
			}	
		}
		
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



using UnityEngine;
using System.Collections;

[System.Serializable]
public class DialogueModel {

	public string event_key = "";
	public string name = "";

	[MultilineAttribute]
	public string message = "";

}

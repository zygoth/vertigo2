using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBlockScript : MonoBehaviour {
	
	// Use this for initialization
	private bool viewed;
	private NewDialogue dialogue;

	void Start () 
	{		
		dialogue = GetComponent<NewDialogue> ();		
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

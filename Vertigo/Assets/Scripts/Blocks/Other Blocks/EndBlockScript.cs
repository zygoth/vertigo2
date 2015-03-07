using UnityEngine;
using System.Collections;

public class EndBlockScript : MonoBehaviour 
{
	// To be set in the scene editor
	public string levelToLoad;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Character") 
		{
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.endLevel (levelToLoad);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

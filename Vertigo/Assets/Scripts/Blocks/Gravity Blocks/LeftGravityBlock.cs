using UnityEngine;
using System.Collections;

public class LeftGravityBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Character")
		{
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");
			script.switchGravity (CharacterControllerScript.gravityDirection.LEFT);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

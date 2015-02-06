using UnityEngine;
using System.Collections;

public class UpGravityBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");
		script.switchGravity (CharacterControllerScript.gravityDirection.UP);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

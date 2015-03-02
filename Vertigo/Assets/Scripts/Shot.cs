using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	private Vector2 velocityVector;
	// Use this for initialization
	void Start () 
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");
		switch (script.gravity)
		{
			case (CharacterControllerScript.gravityDirection.DOWN):
			return;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

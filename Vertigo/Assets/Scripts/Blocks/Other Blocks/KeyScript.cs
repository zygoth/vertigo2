using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject character = GameObject.Find ("Character");
		if (other == character.GetComponent<Collider2D>()) {
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.addKey ();
			Component.Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

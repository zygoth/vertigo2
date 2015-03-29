using UnityEngine;
using System.Collections;

public class BlockLockedScript : MonoBehaviour {

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
			if (script.key) {
				SoundManager.playSound ("Block Unlock");
				Component.Destroy (gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

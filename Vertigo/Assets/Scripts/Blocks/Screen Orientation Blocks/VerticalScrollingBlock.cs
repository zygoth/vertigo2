using UnityEngine;
using System.Collections;

/*
 * Please note:  When placing these blocks in the editor, you must set the public class variables inside the editor.
 * This is because each block will have different values, and they need to be put in by hand.
 */
public class VerticalScrollingBlock : MonoBehaviour 
{
	public float topStoppingPoint;
	public float bottomStoppingPoint;
	public float startX;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Character") {
			GameObject character = GameObject.Find ("Main Camera");
			CameraController script = (CameraController)character.GetComponent ("CameraController");
			script.setVertical (startX, topStoppingPoint, bottomStoppingPoint);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}

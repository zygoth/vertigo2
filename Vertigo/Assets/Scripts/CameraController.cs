using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public enum ScreenMode {HORIZONTAL, VERTICAL, FREE};
	public ScreenMode currentMode;

	// Use this for initialization
	void Start () 
	{
		currentMode = ScreenMode.FREE;
	}

	void FixedUpdate () 
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");

		float newX = script.transform.position.x;
		float newY = script.transform.position.y;
		float newZ = script.transform.position.z;

		if (currentMode == ScreenMode.HORIZONTAL) 
		{
			transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
		}
		else
		if (currentMode == ScreenMode.VERTICAL) 
		{
			transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
		}
		else
		if (currentMode == ScreenMode.FREE) 
		{
			transform.position = new Vector3 (newX, newY, transform.position.z);
		}
	}
}

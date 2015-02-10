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
		float newY = transform.position.y;
		float newZ = transform.position.z;

		transform.position = new Vector3 (newX, newY, newZ);
	}
}

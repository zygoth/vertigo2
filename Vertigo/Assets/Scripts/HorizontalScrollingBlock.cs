using UnityEngine;
using System.Collections;

public class HorizontalScrollingBlock : MonoBehaviour 
{
	public float rightStoppingPoint;
	public float leftStoppingPoint;
	public float snapHeight;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject character = GameObject.Find ("Main Camera");
		CameraController script = (CameraController) character.GetComponent ("CameraController");
		script.setHorizontal (snapHeight, leftStoppingPoint, rightStoppingPoint);
	}

	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class HorizontalScrollingBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject character = GameObject.Find ("Main Camera");
		CameraController script = (CameraController) character.GetComponent ("CameraController");
		script.setOrientation (CameraController.ScreenMode.HORIZONTAL);
	}

	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cameraScript = (CameraController) camera.GetComponent ("CameraController");
		float newX = transform.position.x;
		float newY = transform.position.y;
		float newZ = transform.position.z;

		if(transform.position.x > camera.transform.position.x + 16)
		{
			newX = camera.transform.position.x - 16;
		}

		if(transform.position.x < camera.transform.position.x - 16)
		{
			newX = camera.transform.position.x + 16;
		}

		if(transform.position.y > camera.transform.position.y + 12)
		{
			newY = camera.transform.position.y - 12;
		}

		if(transform.position.y < camera.transform.position.y - 12)
		{
			newY = camera.transform.position.y + 12;
		}

		transform.position = new Vector3 (newX, newY, newZ);
	}
}

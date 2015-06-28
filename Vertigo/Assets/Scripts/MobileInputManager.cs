using UnityEngine;
using System.Collections;

public class MobileInputManager : MonoBehaviour {

	private int SCREENWIDTH = 32;
	private int SCREENHEIGHT = 24;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		/*
		if(Input.GetMouseButtonDown (0))
		{
			Debug.LogError ("touch at screen position: " + Input.mousePosition.x + " " + Input.mousePosition.y);
			Vector3 worldClickPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			Debug.LogError ("touch at world position: " + worldClickPoint.x + " " + worldClickPoint.y);
			return;
		}*/

		if (Input.touchCount == 0)
		{
			return;
		}

		float XAxis = 0f;
		float YAxis = 0f;
		bool jump = false;
		bool shoot = false;
		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cameraScript = (CameraController) camera.GetComponent ("CameraController");
		Vector2 bottomLeftCorner = new Vector2 (cameraScript.transform.position.x - (SCREENWIDTH / 2), cameraScript.transform.position.y - (SCREENHEIGHT / 2));

		// Look for all touches
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			Vector3 worldPoint = cameraScript.camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,0));

			if (worldPoint.x < bottomLeftCorner.x + SCREENWIDTH / 8)
			{
				XAxis = -1;
			}
			if (worldPoint.x > bottomLeftCorner.x + SCREENWIDTH / 8 && worldPoint.x < bottomLeftCorner.x + SCREENWIDTH / 4)
			{
				XAxis = 1;
			}
			if (worldPoint.y > bottomLeftCorner.y && worldPoint.y < bottomLeftCorner.y + SCREENHEIGHT / 2 &&
			    worldPoint.x < bottomLeftCorner.y + SCREENWIDTH * 1 / 4)
			{
				YAxis = -1;
			}
			if (worldPoint.y > bottomLeftCorner.y + SCREENHEIGHT / 2 && worldPoint.y < bottomLeftCorner.y + SCREENHEIGHT &&
			    worldPoint.x < bottomLeftCorner.y + SCREENWIDTH * 1 / 4)
			{
				YAxis = 1;
			}

			// only jump or shoot at the beginning of the touch
			if(touch.phase != TouchPhase.Began)
			{
				continue;
			}

			// Jump check
			if (worldPoint.x > bottomLeftCorner.x + SCREENWIDTH * 7 / 8)
			{
				jump = true;
			}

			// Shoot
			if (worldPoint.x < bottomLeftCorner.x + SCREENWIDTH * 7 / 8 && worldPoint.x > bottomLeftCorner.x + SCREENWIDTH * 3 / 4)
			{
				shoot = true;
			}
		}

		GameObject character = GameObject.Find("Character");
		CharacterControllerScript script = (CharacterControllerScript)character.GetComponent("CharacterControllerScript");
		script.doWalking (XAxis, YAxis);
		script.doJumpCheck (jump);
		script.doShootCheck (shoot);
	}
}

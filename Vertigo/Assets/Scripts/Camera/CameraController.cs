using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public enum ScreenMode {HORIZONTAL, VERTICAL, FREE};
	public ScreenMode currentMode;
	private float leftWall;
	private float rightWall;
	private float topWall;
	private float bottomWall;

	// Use this for initialization
	void Start () 
	{
		currentMode = ScreenMode.FREE;
		leftWall = -int.MaxValue;
		rightWall = int.MaxValue;
		topWall = int.MaxValue;
		bottomWall = -int.MaxValue;
		Screen.SetResolution (640, 480, true);
	}

	public void setOrientation(ScreenMode newMode)
	{
		currentMode = newMode;
	}

	public void setHorizontal(float centerY, float leftWall, float rightWall)
	{
		currentMode = ScreenMode.HORIZONTAL;
		transform.position = new Vector3 (transform.position.x, centerY, transform.position.z);
		this.leftWall = leftWall;
		this.rightWall = rightWall;
	}

	public void setVertical(float centerX, float topWall, float bottomWall)
	{
		currentMode = ScreenMode.VERTICAL;
		transform.position = new Vector3 (centerX, transform.position.y, transform.position.z);
		this.topWall = topWall;
		this.bottomWall = bottomWall;
	}

	public void setFree(float leftWall, float bottomWall, float rightWall, float topWall)
	{
		currentMode = ScreenMode.FREE;
		this.leftWall = leftWall;
		this.bottomWall = bottomWall;
		this.rightWall = rightWall;
		this.topWall = topWall;
	}

	private float enforceHorizontalBorders(float possibleXPosition)
	{
		float newX = possibleXPosition;

		if(newX >= rightWall)
		{
			newX = rightWall;
		}
		
		if(newX <= leftWall)
		{
			newX = leftWall;
		}

		return newX;
	}

	private float enforceVerticalBorders(float possibleYPosition)
	{
		float newY = possibleYPosition;
		
		if(newY >= topWall)
		{
			newY = topWall;
		}
		
		if(newY <= bottomWall)
		{
			newY = bottomWall;
		}
		
		return newY;
	}

	void Update () 
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");

		float newX = (transform.position.x + script.transform.position.x) / 2;
		float newY = (transform.position.y + script.transform.position.y) / 2;
		float newZ = script.transform.position.z;

		if (currentMode == ScreenMode.HORIZONTAL) 
		{
			newX = enforceHorizontalBorders (newX);
			transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
		}
		else
		if (currentMode == ScreenMode.VERTICAL) 
		{
			newY = enforceVerticalBorders (newY);
			transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
		}
		else
		if (currentMode == ScreenMode.FREE) 
		{
			newX = enforceHorizontalBorders (newX);
			newY = enforceVerticalBorders (newY);
			transform.position = new Vector3 (newX, newY, transform.position.z);
		}
	}
}

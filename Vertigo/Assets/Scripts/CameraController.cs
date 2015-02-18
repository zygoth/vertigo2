﻿using UnityEngine;
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
	}

	public void setOrientation(ScreenMode newMode)
	{
		currentMode = newMode;
	}

	public void setHorizontal(float centerY, float leftWall, float rightWall)
	{
		currentMode = ScreenMode.HORIZONTAL;
		this.leftWall = leftWall;
		this.rightWall = rightWall;
	}

	public void setVertical(float centerY, float topWall, float bottomWall)
	{
		currentMode = ScreenMode.VERTICAL;
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

	void FixedUpdate () 
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");

		float newX = script.transform.position.x;
		float newY = script.transform.position.y;
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

using UnityEngine;
using System.Collections;

/*
 * This object determines the starting behavior of the screen.
 * Like the orientation blocks, this object is meant to be initialized through the scene editor.
 */
public class ScreenOrientationInitializer : MonoBehaviour 
{
	public CameraController.ScreenMode startMode;
	public float topStoppingPoint;
	public float bottomStoppingPoint;
	public float rightStoppingPoint;
	public float leftStoppingPoint;
	public float startY;
	public float startX;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		CameraController script = (CameraController) GetComponent ("CameraController");
		
		switch(startMode)
		{
		case CameraController.ScreenMode.HORIZONTAL:
			script.setHorizontal (startY, leftStoppingPoint, rightStoppingPoint);
			break;
		case CameraController.ScreenMode.VERTICAL:
			script.setVertical (startX, topStoppingPoint, bottomStoppingPoint);
			break;
		case CameraController.ScreenMode.FREE:
			script.setFree (leftStoppingPoint, bottomStoppingPoint, rightStoppingPoint, topStoppingPoint);
			break;
		}

		Destroy (this);
	}
}

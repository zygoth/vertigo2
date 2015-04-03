using UnityEngine;
using System.Collections;

public class MobileInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		float XAxis = 0f;
		float YAxis = 0f;
		bool jump = false;
		bool shoot = false;

		// Look for all fingers
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			


			if (touch.position.x < AspectUtility.xOffset + AspectUtility.screenWidth / 8)
			{
				XAxis = -1;
			}
			if (touch.position.x > AspectUtility.xOffset + AspectUtility.screenWidth / 8 && touch.position.x < AspectUtility.xOffset + AspectUtility.screenWidth / 4)
			{
				XAxis = 1;
			}
			if (touch.position.y > AspectUtility.yOffset && touch.position.y < AspectUtility.yOffset + AspectUtility.screenHeight / 8)
			{
				YAxis = -1;
			}
			if (touch.position.y > AspectUtility.yOffset + AspectUtility.screenHeight / 8 && touch.position.y < AspectUtility.yOffset + AspectUtility.screenHeight / 4)
			{
				YAxis = 1;
			}

			// Jump check
			if (touch.position.x > AspectUtility.xOffset + AspectUtility.screenWidth * 7 / 8)
			{
				jump = true;
			}

			// Shoot
			if (touch.position.x < AspectUtility.xOffset + AspectUtility.screenWidth * 7 / 8 && touch.position.x > AspectUtility.xOffset + AspectUtility.screenWidth * 3 / 4)
			{
				shoot = true;
			}
		}
		GameObject character = GameObject.Find("Character");
		CharacterControllerScript script = (CharacterControllerScript)character.GetComponent("CharacterControllerScript");
		script.doMovement (XAxis, YAxis);
		script.doJumpCheck (jump);
		script.doShootCheck (shoot);
	}
}

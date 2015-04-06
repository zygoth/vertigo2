using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevelBlock : MonoBehaviour {
	public static string levelToLoad = "Intro Level";
	public static string currentLevelNum;
	public Text levelTextNumber;

	public static void updateLoadLevelBlock(string levelToUpdateTo){
		LoadLevelBlock.levelToLoad = levelToUpdateTo;
		switch (levelToUpdateTo) {
		case("Intro Level"):	currentLevelNum = "1";
			break;
		case("Spiral Spikes"):	currentLevelNum = "2";
			break;
		default:				currentLevelNum = "1";
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Character") 
		{
			GameObject character = GameObject.Find ("Character");
			CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
			script.endLevel (LoadLevelBlock.levelToLoad);
		}
	}

	// Use this for initialization
	void Start () {
		updateLoadLevelBlock (levelToLoad);
		levelTextNumber.text = currentLevelNum;
	}
	
	// Update is called once per frame
	void Update () {
	}
}

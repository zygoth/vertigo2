using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}

	public static void playSound(string soundName)
	{
		GameObject sound = GameObject.Find (soundName);

		if(sound == null)
		{
			Debug.Log ("tried to play sound: " + soundName + ", but doesn't exist");
			return;
		}

		sound.audio.Play ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GameObject introMusic = GameObject.Find ("Intro Music");
		introMusic.audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

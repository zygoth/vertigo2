using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{
	public enum Song {INTRO, SONG1, SONG2, SONG3, SONG4, CREDITS};

	public Song musicToPlay = Song.INTRO;

	// Use this for initialization
	void Start () 
	{
		GameObject introMusic;

		switch (musicToPlay)
		{
		case Song.INTRO:
			introMusic = GameObject.Find ("Intro Music");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		case Song.SONG1:
			introMusic = GameObject.Find ("Music 1");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		case Song.SONG2:
			introMusic = GameObject.Find ("Music 2");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		case Song.SONG3:
			introMusic = GameObject.Find ("Music 3");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		case Song.SONG4:
			introMusic = GameObject.Find ("Music 4");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		case Song.CREDITS:
			introMusic = GameObject.Find ("Credits Music");
			introMusic.GetComponent<AudioSource>().Play ();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

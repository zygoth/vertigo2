using UnityEngine;
using System.Collections;

public class EndBlockScript : MonoBehaviour 
{
	public string levelToLoad;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Application.LoadLevel (levelToLoad);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

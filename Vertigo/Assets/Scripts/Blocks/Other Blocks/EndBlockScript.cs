using UnityEngine;
using System.Collections;

public class EndBlockScript : MonoBehaviour 
{
	// To be set in the scene editor
	public string levelToLoad;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		StartCoroutine ("loadNewLevel");
	}

	private IEnumerator loadNewLevel()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f); // wait half a second
			// do things
			Application.LoadLevel (levelToLoad);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

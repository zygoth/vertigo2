using UnityEngine;
using System.Collections;

public class BreakableBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.name == "Shot") 
		{
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	private Vector2 velocityVector;
	// Use this for initialization
	void Start () 
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("collided with a block!");
		if(other.gameObject.layer == 0) // default layer, so it's a collidable block
		{

			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

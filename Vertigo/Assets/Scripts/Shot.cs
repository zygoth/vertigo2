using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public GameObject explosionPrefab;
	private Vector2 velocityVector;
	// Use this for initialization
	void Start () 
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Breakable Block")
		{
			Destroy (other.gameObject);
		}
		if(other.gameObject.layer == 0) // default layer, so it's a collidable block
		{
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

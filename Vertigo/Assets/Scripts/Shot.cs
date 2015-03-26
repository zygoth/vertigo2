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
			Instantiate (explosionPrefab, other.transform.position, Quaternion.identity);
			Instantiate (explosionPrefab, new Vector3(other.transform.position.x - 1, other.transform.position.y + 1, other.transform.position.z), Quaternion.identity);
			Instantiate (explosionPrefab, new Vector3(other.transform.position.x + 1, other.transform.position.y + 1, other.transform.position.z), Quaternion.identity);
			Instantiate (explosionPrefab, new Vector3(other.transform.position.x - 1, other.transform.position.y - 1, other.transform.position.z), Quaternion.identity);
			Instantiate (explosionPrefab, new Vector3(other.transform.position.x + 1, other.transform.position.y - 1, other.transform.position.z), Quaternion.identity);

			Destroy (other.gameObject);
			SoundManager.playSound("Block Break");
			Destroy (gameObject);
			return;
		}
		if(other.gameObject.layer == 0) // default layer, so it's a collidable block
		{
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			SoundManager.playSound("Shot Explode");
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}

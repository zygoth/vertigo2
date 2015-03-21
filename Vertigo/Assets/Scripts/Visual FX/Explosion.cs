using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void destroySelf()
	{
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

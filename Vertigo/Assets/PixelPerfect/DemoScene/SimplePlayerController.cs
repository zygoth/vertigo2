using UnityEngine;
using System.Collections;

public class SimplePlayerController : MonoBehaviour {

	public float playerSpeed=1;

	void Start () {
	}
	
	void Update () {
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position-=playerSpeed*Time.deltaTime*Vector3.right;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position+=playerSpeed*Time.deltaTime*Vector3.right;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position+=playerSpeed*Time.deltaTime*Vector3.up;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position-=playerSpeed*Time.deltaTime*Vector3.up;
		}
	}
	
}

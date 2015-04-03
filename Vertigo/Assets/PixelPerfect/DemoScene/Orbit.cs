using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public float speed=1; 

	void Update () {
		transform.position+=new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time),0)*speed*Time.deltaTime;
	}
}

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectPhysics : MonoBehaviour {

	void Update () {
		if (transform.localPosition!=Vector3.zero) {
			transform.localPosition=Vector3.zero;
		}
	}
}

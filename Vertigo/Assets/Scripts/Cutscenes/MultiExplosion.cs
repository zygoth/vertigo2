using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiExplosion : CutsceneAction
{
	public GameObject explosionPrefab;

	public int numExplosions = 20;
	private int timesExploded = 0;
	public float secondsBetweenExplosions = .1f;
	System.Random rnd = new System.Random();

	public MultiExplosion() : base() {

	}

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (doAction ());
    }

    // Update is called once per frame
    void Update()
    {
    	
    }

    IEnumerator doAction() {    	

        while(timesExploded < numExplosions) {
        	Debug.Log("doing action");
        	int xPos = rnd.Next(0,16);
	    	int yPos = rnd.Next(0,12);
	        Instantiate (explosionPrefab, GameObject.Find("Character").transform.position + new Vector3(xPos, yPos, 0), Quaternion.identity);
	        timesExploded += 1;
        	yield return new WaitForSeconds(secondsBetweenExplosions);
        }

        if(base.eventName != null && base.eventListener != null) {
        	base.eventListener.OnEvent(eventName + ":end");
        }
       	Object.Destroy(this.gameObject);
    }
}

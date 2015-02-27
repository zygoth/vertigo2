using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int maxHealth = 100;
	public double curHealth = 100;
	public double healthBarLength;

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 2;
	}
	
	// Update is called once per frame
	void Update () {
		adjustCurHealth (0);
	}

	void OnGUI(){
		GUI.color = Color.red;
		GUI.Box (new Rect(125, 10, (float)healthBarLength, 20), curHealth + "/" + maxHealth);
	}

	public void adjustCurHealth(int adj){
		curHealth += adj;
		if (curHealth < 0) {//Check for going below 0 health
			curHealth = 0;
		}

		if (curHealth > maxHealth) {//Check for going above max health
			curHealth = maxHealth;
		}

		if (maxHealth < 1) {//Check for divide by 0
			maxHealth = 1;
		}
		healthBarLength = (Screen.width / 2) * (curHealth / maxHealth);

	}
}

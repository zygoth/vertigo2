using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	public Slider healthBarSlider;//reference for slider
	public double maxHealth;
	public double curHealth;

	// Use this for initialization
	void Start () {
		maxHealth = 1;
		curHealth = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
	}

	/*
	 * Used to adjust the health bar by some value, either positive or negative. Example: Adjusting by -.25 will lower the player's health by one quarter.
	 */
	public void adjustCurHealth(double adj){
		if (adj > 0 && healthBarSlider.value < maxHealth) {
			SoundManager.playSound("Heal Sound");
		}

		healthBarSlider.value += (float)adj;//adjust health value
		if (healthBarSlider.value < 0) {//Check for going below 0 health
			healthBarSlider.value = 0;
		}

		if (healthBarSlider.value > maxHealth) {//Check for going above max health
			healthBarSlider.value = (float)maxHealth;
		}
		curHealth = healthBarSlider.value;
	}
}

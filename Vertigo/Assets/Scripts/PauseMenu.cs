using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

	private float startTime = 0.1f;
	
	public Material mat;

	private float savedTimeScale;
	
	private bool showfps;
	
	public Color lowFPSColor = Color.red;
	public Color highFPSColor = Color.green;
	public Image pauseFilter;
	public int lowFPS = 29;
	public int highFPS = 50;
	public string currentLevel;
	public GameObject start;
	
	public string url = "https://github.com/colindt/unity-platformer";
	
	public Color statColor = Color.white;
	
	public string[] credits= {
		"A BYU CS 428 Production",
		"Programming by Ben Coffman, Colin Thompson, Joe Lyon, Schuyler Goodman, Jeff Angell, & Joe Eklund",
		"For more info, check out https://github.com/colindt/unity-platformer",
		"Updated 4.4.2015"} ;
	
	public enum Page {
		None,Main,Options,Credits
	}
	
	private Page currentPage;

	private float fps;
	
	private int toolbarInt = 0;
	private string[]  toolbarstrings =  {"Audio","Graphics", "Stats","System"};
	
	
	void Start() {
		Time.timeScale = 1;
	}
	
	void LateUpdate () {
		if (showfps) {
			FPSUpdate();
		}
		
		if (Input.GetKeyDown("escape")) 
		{
			switch (currentPage) 
			{
			case Page.None: 
				PauseGame(); 
				break;
				
			case Page.Main: 
				if (!IsBeginning()) 
					UnPauseGame(); 
				break;
				
			default: 
				currentPage = Page.Main;
				break;
			}
		}
	}
	
	void OnGUI () {
		ShowStatNums();
		if (IsGamePaused()) {
			Color temp = Color.black;
			temp.a = 200 / 255.0f;
			pauseFilter.color = temp;
			GUI.color = statColor;
			switch (currentPage) {
			case Page.Main: MainPauseMenu(); break;
			case Page.Options: ShowToolbar(); break;
			case Page.Credits: ShowCredits(); break;
			}
		}   
	}

	void ShowToolbar() {
		BeginPage(300,300);
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarstrings);
		switch (toolbarInt) {
		case 0: VolumeControl(); break;
		case 3: ShowDevice(); break;
		case 1: showGraphics(); break;
		case 2: StatControl(); break;
		}
		EndPage();
	}


	void showGraphics(){
		GUILayout.Label("There are currently no changeable graphics options.");
	}

	void ShowCredits() {
		BeginPage(300,300);
		foreach(string credit in credits) {
			GUILayout.Label(credit);
		}
		EndPage();
	}
	
	void ShowBackButton() {
		if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) {
			currentPage = Page.Main;
		}
	}
	
	void ShowDevice() {
		GUILayout.Label("Unity player version "+Application.unityVersion);
		GUILayout.Label("Graphics: "+SystemInfo.graphicsDeviceName+" "+
		                SystemInfo.graphicsMemorySize+"MB\n"+
		                SystemInfo.graphicsDeviceVersion+"\n"+
		                SystemInfo.graphicsDeviceVendor);
		GUILayout.Label("OS: "+SystemInfo.operatingSystem);
	}
	
	void VolumeControl() {
		GUILayout.Label("Volume");
		AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 1);
	}
	
	void StatControl() {
		GUILayout.BeginHorizontal();
		showfps = GUILayout.Toggle(showfps,"FPS");
		GUILayout.EndHorizontal();
	}
	
	void FPSUpdate() {
		float delta = Time.smoothDeltaTime;
		if (!IsGamePaused() && delta !=0.0) {
			fps = 1 / delta;
		}
	}
	
	void ShowStatNums() {
		GUILayout.BeginArea( new Rect(Screen.width - 100, 10, 100, 200));
		if (showfps) {
			string fpsstring= fps.ToString ("#,##0 fps");
			GUI.color = Color.Lerp(lowFPSColor, highFPSColor,(fps-lowFPS)/(highFPS-lowFPS));
			GUILayout.Label ("<size=20>" + fpsstring + "</size>");
		}
		GUILayout.EndArea();
	}
	
	void BeginPage(int width, int height) {
		GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
	}
	
	void EndPage() {
		GUILayout.EndArea();
		if (currentPage != Page.Main) {
			ShowBackButton();
		}
	}
	
	bool IsBeginning() {
		return (Time.time < startTime);
	}
	
	
	void MainPauseMenu() {
		BeginPage(200,200);
		if (GUILayout.Button ("Continue")) {
			UnPauseGame();
			
		}
		if (GUILayout.Button ("Restart")) {
			loadLevel(currentLevel);
			
		}
		if (GUILayout.Button ("Main Menu")) {
			loadLevel("Main Menu");
			
		}
		if (GUILayout.Button ("Options")) {
			currentPage = Page.Options;
		}
		if (GUILayout.Button ("Credits")) {
			currentPage = Page.Credits;
		}
		if (GUILayout.Button ("Quit")) {
			Application.Quit();
		}
		EndPage();
	}
	
	void PauseGame() {
		savedTimeScale = Time.timeScale;
		Time.timeScale = 0;
		AudioListener.pause = true;

		currentPage = Page.Main;
	}
	
	void UnPauseGame() {
		Time.timeScale = savedTimeScale;
		AudioListener.pause = false;

		Color temp = Color.black;
		temp.a = 0 / 255.0f;
		pauseFilter.color = temp;
		currentPage = Page.None;
		if (IsBeginning() && start != null) {
			start.SetActive(true);
		}
	}

	void loadLevel(string levelToLoad){
		Time.timeScale = savedTimeScale;
		AudioListener.pause = false;
		if (IsBeginning() && start != null) {
			start.SetActive(true);
		}
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript script = (CharacterControllerScript)character.GetComponent ("CharacterControllerScript");
		script.loadLevel (levelToLoad);
		
	}

	bool IsGamePaused() {
		return (Time.timeScale == 0);
	}
	
	void OnApplicationPause(bool pause) {
		if (IsGamePaused()) {
			AudioListener.pause = true;
		}
	}
}
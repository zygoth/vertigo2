using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePictureMap
{
	private Dictionary<string, string> profilePicMap = new Dictionary<string, string>();

	public ProfilePictureMap() {
		profilePicMap.Add("default", "Profiles/crackblock1");
		profilePicMap.Add("Mazeman", "Profiles/MAZEMANHEAD");
        profilePicMap.Add("Mazeman(annoyed)", "Profiles/MAZEMANHEADANNOYED");
        profilePicMap.Add("Mazeman(surprised)", "Profiles/MAZEMANHEADSURPRISED");
	}

	public string getProfilePath(string nameAndEmotion) {
		return profilePicMap[nameAndEmotion];
	}    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneAction : MonoBehaviour
{
	public ICutsceneEventListener eventListener { get; set; }
	public string eventName { get; set; }

}

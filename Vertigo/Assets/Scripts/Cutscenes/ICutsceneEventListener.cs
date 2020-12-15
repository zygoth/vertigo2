using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICutsceneEventListener
{
    void OnEvent(string eventString);
}

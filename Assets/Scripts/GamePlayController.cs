using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCOntroller : MonoBehaviour
{
    public void LoadOtherLevels()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        SceneLoader.instance.LoadLevel(name);
    }
}

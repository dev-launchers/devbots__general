using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void ChangeToScene(int sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void SaveCustomizationChanges(GameObject bot) {
        //Should be changed to proper serialization method
        PrefabUtility.SaveAsPrefabAsset(bot, "Assets/Prefabs/CurrentPlayer.prefab");
    }


}    

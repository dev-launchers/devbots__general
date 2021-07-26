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
        GameObject playerBot = GameObject.FindGameObjectsWithTag("Bot")[0]; //Gets bot from menu screen
        foreach(BotPart botPart in playerBot.GetComponentsInChildren<BotPart>()) {
            //Activate components on bot, readying it for battle
            botPart.SetState(new State(true));
        }
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void SaveCustomizationChanges(GameObject bot) {
        //Should be changed to proper serialization method
        //PrefabUtility.SaveAsPrefabAsset(bot, "Assets/Prefabs/CurrentPlayer.prefab");
    }


}    

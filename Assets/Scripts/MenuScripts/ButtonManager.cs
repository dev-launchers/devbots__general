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
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot"); //Gets all bots in combat scene
        foreach(GameObject bot in bots) {
            foreach(BotPart botPart in bot.GetComponentsInChildren<BotPart>()) {
            //Activate components on bot, readying it for battle
            botPart.SetState(new State(true));
            }
        }
    }

    public void SaveCustomizationChanges(GameObject bot) {
        //Should be changed to proper serialization method
        //PrefabUtility.SaveAsPrefabAsset(bot, "Assets/Prefabs/CurrentPlayer.prefab");
    }


}    

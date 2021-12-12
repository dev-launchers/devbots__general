using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains static methods that handle scene loading.
/// </summary>
public class SceneHandler : MonoBehaviour
{
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu Scene");
        GameObject.FindObjectOfType<SoundManager>().PlayMusic("Menu Music");
    }

    public static void LoadCustomizeBotScene()
    {
        SceneManager.LoadScene("Bot Customize Scene");
    } 
    public static void LoadMarketplaceScene()
    {
        SceneManager.LoadScene("Marketplace Scene");
    }
    public static void LoadCombatScene()
    {
        SceneManager.LoadScene("Combat");
    } 

    public static void LoadVictoryScene()
    {
        SceneManager.LoadScene("Victory Scene");
    }
    public static void LoadLoseScene()
    {
        SceneManager.LoadScene("Lose Scene");
    }
    public static void LoadSettingsScene()
    {
        SceneManager.LoadScene("Settings Scene");
    }
}

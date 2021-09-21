using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains static methods that handle scene loading.
/// </summary>
public class SceneHandler : MonoBehaviour
{
public static void LoadCustomizeBotScene()
    {
        SceneManager.LoadScene(0);
    }
    public static void LoadCombatScene()
    {
        SceneManager.LoadScene(1);
    }
    public static void LoadVictoryScene()
    {
        SceneManager.LoadScene(2);
    }
    public static void LoadLoseScene()
    {
        SceneManager.LoadScene(3);
    }
}

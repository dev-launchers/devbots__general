using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;
using UnityEngine.Audio;
public enum SoundType
{
    Sfx, Music, Reactive 
};

public enum MusicType
{
    BattleMountainTheme,
    BattleFactoryTheme,
    BattleCityTheme,
    MenuTheme
};

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        
    }
}

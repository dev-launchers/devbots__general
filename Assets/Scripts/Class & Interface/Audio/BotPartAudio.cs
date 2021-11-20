using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPartAudio : MonoBehaviour
{
    private BotAudioManager manager;

    [SerializeField] private  PartScriptableObject[] botPartSounds;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<BotAudioManager>();
    }

    public void PlaySound(string soundName = " ")
    {
        if (soundName == " ")
        {
            manager.PerformSound(botPartSounds[0]);
            
        }
        else
        {
            PartScriptableObject s = Array.Find(botPartSounds, sound => sound.soundName == soundName);
            manager.PerformSound(s);
        }
        
    }

}

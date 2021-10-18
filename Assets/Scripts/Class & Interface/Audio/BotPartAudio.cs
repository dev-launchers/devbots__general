using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class BotPartSound
{
    public string nameOfSound;
    public PartScriptableObject partSound;
};

public class BotPartAudio : MonoBehaviour
{
    private BotAudioManager manager;

    [SerializeField] private BotPartSound[] botPartSounds;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<BotAudioManager>();
    }

    public void PlaySound(string soundName = " ")
    {
        if (soundName == " ")
        {
            manager.PerformSound(botPartSounds[0].partSound);
        }
        else
        {
            BotPartSound s = Array.Find(botPartSounds, sound => sound.nameOfSound == soundName);
            manager.PerformSound(s.partSound);
        }
        
    }

}

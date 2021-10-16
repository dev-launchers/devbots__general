using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAudioManager : MonoBehaviour
{
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformSound(PartScriptableObject sound)
    {
        source.pitch = sound.pitch;
        source.PlayOneShot(sound.clip);
    }
}

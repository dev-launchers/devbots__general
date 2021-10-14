using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPartAudio : MonoBehaviour
{
    private BotAudioManager manager;

    [SerializeField] private PartScriptableObject partSound;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<BotAudioManager>();
    }

    public void PlaySound()
    {
        manager.PerformSound(partSound);
    }

}

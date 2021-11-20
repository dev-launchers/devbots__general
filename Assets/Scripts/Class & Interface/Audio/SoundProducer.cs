using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//meant to be put on objects which have sounds to play at an animation, NOT MEANT FOR BOT PARTS
public class SoundProducer : MonoBehaviour
{
    [SerializeField]private ReactiveAudioScriptableObject soundObject;
    public void PlaySound()
    {
        SoundManager.main.PlayReactionarySound(soundObject);
    }
}

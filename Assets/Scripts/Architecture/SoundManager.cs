using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;
using UnityEngine.Audio;
/*public enum SoundType
{
    Part, Music, Reactive 
};*/
/*
 *
 *
 * for bot parts, why not create an audio manager to apply onto bot prefabs?
 */
public class SoundManager : MonoBehaviour//this only manages one off sounds, UI sounds, music
{
    public static SoundManager main;

    [SerializeField] private AudioSource musicSource;
   // [SerializeField] private AudioSource partSoundSource;
    [SerializeField] private AudioSource reactiveAudioSource;

    public MusicScriptableObject[] musicList;//meant to be filled in the editor
    
    
    private void Awake()
    {
        main = this;
    }


    public void PlayMusic(string music)
    {
        MusicScriptableObject s = Array.Find(musicList, sound => sound.musicName == music);
        musicSource.PlayOneShot(s.clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PlayBotPartSound(PartScriptableObject sound)//temporary
    {
      //  partSoundSource.pitch = sound.pitch;
      //  partSoundSource.PlayOneShot(sound.clip);
    }

    public void PlayReactionarySound(ReactiveAudioScriptableObject sound)//UI sounds, click effects, one time effects for things that don't need their own audio maager
    {
        reactiveAudioSource.PlayOneShot(sound.clip);
    }
    
}

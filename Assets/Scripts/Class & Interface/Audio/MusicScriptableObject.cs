using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Audio",menuName = "Music")]
public class MusicScriptableObject : SoundScriptableObjectScript
{
    [Tooltip("spaces to be marked with an underscore. \nEg:  Battle_Music_Mountain")]
    public string musicName;
}

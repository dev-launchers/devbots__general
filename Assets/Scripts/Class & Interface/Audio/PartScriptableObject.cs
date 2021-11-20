using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Audio",menuName = "Bot Part Sound")]
public class PartScriptableObject : SoundScriptableObjectScript
{
    [Range(0.1f,100f)]public float pitch;
    public string soundName;
}

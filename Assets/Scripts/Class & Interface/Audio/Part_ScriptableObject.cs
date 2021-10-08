using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Audio",menuName = "Bot Part")]
public class PartScriptableObject : SoundScriptableObjectScript
{
    [Range(0.1f,3f)]public float pitch;
    
}

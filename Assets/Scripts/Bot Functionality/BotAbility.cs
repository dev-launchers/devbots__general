using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAbility : ScriptableObject
{
    public string abilityName;
    public float coolDownTime;
    public float activeTime;
    
    public virtual void Activate(GameObject parent)
    {
        Debug.Log(name+" activated");
    }
}

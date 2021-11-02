using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAbility : ScriptableObject
{
    public string name;
    public float coolDownTime;
    
    public virtual void Activate(GameObject parent)
    {
    
    }
}

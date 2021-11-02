using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AbilityState
{
    Ready,
    Active,
    Cooldown
};
public class BotAbilityHolder : MonoBehaviour
{
    

    private AbilityState state = AbilityState.Ready;
    
    public BotAbility ability;

    private float coolDownTime;

    private float activeTime;
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum AbilityState
{
    Ready,
    Active,
    Cooldown
};
public class BotAbilityHolder : MonoBehaviour
{
    private AbilityState state = AbilityState.Active;
    
    public BotAbility ability;

    private float coolDownTime;

    private float activeTime = 0;

    [SerializeField] private bool isRunning;
    // Update is called once per frame
    void Update()
    {
        activeTime += Time.deltaTime;
        switch(state)
        {
            case AbilityState.Ready:
                if (isRunning/*condition for any move*/)
                {
                    //ability.Activate(gameObject);
                    //state = AbilityState.Active;
                    //activeTime = ability.activeTime;
                }
                break;
            
            case AbilityState.Active:
                if (activeTime > 0) activeTime -= Time.deltaTime;
                else
                {
                    state = AbilityState.Cooldown;
                    coolDownTime = ability.coolDownTime;
                    Debug.Log("Cooldown");
                }
                break;
            
            case AbilityState.Cooldown: 
                if (coolDownTime > 0) coolDownTime -= Time.deltaTime;
                else
                {
                    Debug.Log("Ready");
                    state = AbilityState.Ready;
                }
                break;
        }
    }
}

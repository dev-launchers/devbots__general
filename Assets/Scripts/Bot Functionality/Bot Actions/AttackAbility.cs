using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack Ability", menuName = "Ability Objects/Attack Ability", order = 1)]
public class AttackAbility : BotAbility
{
    public Vector2 dir;
    public float power;
    
    
    private Animator animator;//Animator used for sword rotation
    private Rigidbody2D rb;
    private BotSensor sensor;
    private int enemyLayer;
    public override void Activate(GameObject parent)//we may split this up further if necessary with more layers of abstraction
    {
        base.Activate(parent);
        
        //set up
        Animator = parent.GetComponent<Animator>();
        rb = parent.GetComponentInParent<Rigidbody2D>();
        sensor = parent.GetComponentInParent<BotSensor>();
        enemyLayer = sensor.GetEnemyLayer();
        
        Debug.Log(parent.name+" performed attack ability");
        
    }
}

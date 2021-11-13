using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Gun Ability", menuName = "Ability Objects/Attack Ability/Gun", order = 1)]

public class GunAbility : BotAbility
{
    public GameObject projectile;

    public Vector2 direction;

    public float speed;

    public override void Activate(GameObject parent)
    {
        Debug.Log("Shoot");
        Transform shootPos=parent.GetComponent<GunPosition>().shootPosition;
        GameObject b= Instantiate(projectile, shootPos.position, shootPos.rotation);
        b.GetComponent<Rigidbody2D>().AddForce(direction*speed,ForceMode2D.Impulse);
    }
}

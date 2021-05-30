using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default(Rigidbody2D);
    
    private float movespeed = 5.0f;

    public void MoveStep(float enemyPos)
    {
        //Should depend on specific movement part
        rb.velocity = new Vector2(enemyPos * movespeed, 1);
    }

}

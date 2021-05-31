﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = default(Rigidbody2D);
    [SerializeField] private float movespeed = default(float);
    [SerializeField] private float jumpSize = default(float);

    public void MoveStep(float enemyPos) {
        //Should depend on specific movement part
        rb.velocity = new Vector2(enemyPos * movespeed, jumpSize);
    }

    public void TakeKnockback(float enemyPos, float knockback) {
        //Should depend on specific movement part
        rb.velocity = new Vector2(enemyPos * knockback, knockback);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sprite = default(SpriteRenderer);

    public void FlipSprite(float enemyPos) {
        if (enemyPos < 0f) {
            sprite.flipX = false;
        }
        else {
            sprite.flipX = true;
        }
    }
}

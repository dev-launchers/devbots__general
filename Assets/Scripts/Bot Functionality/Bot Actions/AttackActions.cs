using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActions : MonoBehaviour
{
    // TO-DO add ability pass and use Audio

    public static void ProjectileAttack(
        int _dir,
        Transform _startPos,
        float _dmg,
        float _speed,
        int _enemyLayer,
        Animator _anim,
        GameObject _projectile,
        Vector2 _size
        )
    {
        //Faces attack at enemy, handled as local position to bot part

        GameObject projectileInstance = Instantiate(_projectile, _startPos.position, Quaternion.identity);
        //Create a projectile at the start position

        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        //Fetch script/data for projectile

        projectileScript.SetValues(_dir, _dmg, _speed, _size, _enemyLayer);

        // TO-DO incoorperate audio

        // TO-DO knockback?

    }

    public static void MeleeAttack(
        Transform _attackPoint,
        float _attackRange,
        int damage, 
        Vector2 _knockback,
        LayerMask _enemyLayers,
        Animator _anim,
        string _animTrigger
        )
    {
        Collider2D enemy = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _enemyLayers);
    }
}

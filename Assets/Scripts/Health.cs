using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    //[SerializeField] private GameObject damageFX = default(GameObject);

    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = 1.0f;
    }

    public void TakeDamage(float damage) {
        HP -= damage;
        //Instantiate(damageFX, transform.position, Quaternion.identity);
    }
}

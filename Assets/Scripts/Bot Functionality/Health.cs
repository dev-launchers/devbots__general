using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] Animator animator = default(Animator);

    private AudioManager audioManager;
    //[SerializeField] private GameObject damageFX = default(GameObject);

    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        HP = 1.0f;
    }

    public void TakeDamage(float damage) {
        HP -= damage;
        if (HP <= 0.0f) {
           // audioManager.Play("Death");
          //  animator.Play("death");
            //Make a new gameObject for dead hull, or disable scripts?
            //gameObject.SetActive(false);
            //Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        else {
           // audioManager.Play("Hit");
            //Instantiate(damageFX, transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    Rigidbody2D rb;

    public Slider healthSlider;

    private float knockBack;

    private Scene scene;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        scene = SceneManager.GetActiveScene();
        rb = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        healthSlider.value = 1.0f;
        
    }
    public void Update() {
     if (healthSlider.value <= 0.0f) {
            //Reset
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }

    public void TakeDamage(float damage) {
        Debug.Log(damage);
        healthSlider.value = healthSlider.value - damage;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSlider : MonoBehaviour
{

    [SerializeField] private Health hp = default(Health);

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
        healthSlider.value = hp.HP;
        if (healthSlider.value <= 0.0f) { //Can just be in TakeDamage?
            //Reset
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }

}

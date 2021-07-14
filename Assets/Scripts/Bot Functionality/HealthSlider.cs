using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSlider : MonoBehaviour
{
    private IEnumerator coroutine;

    [SerializeField] private Health hp = default(Health);

    public Slider healthSlider;
    private Scene scene;

    void Start()
    {     
        scene = SceneManager.GetActiveScene();
        Slider healthSlider = FindObjectOfType<Slider>();
    }
    public void Update() {
        healthSlider.value = hp.HP;
    }

    IEnumerator Death(float Death)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(scene.name);


        yield break;
    }

}

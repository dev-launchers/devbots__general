using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPart : MonoBehaviour, IBotPart
{

    [SerializeField] private LayerMask enemyLayer = default(LayerMask);
    [SerializeField] private float attackDistance = default(float);
    [SerializeField] private float damage = default(float);
    [SerializeField] private GameObject projectile = default(GameObject); //Object to be fired by gun part
    [SerializeField] private float projectileSpeed = default(float);
    [SerializeField] private GameObject projectileStartPos;
    [SerializeField] private Vector2 projectileSize = default(Vector2);

    private BotSensor sensor;
    private BotController controller;

    private bool isRunning;
    private float timer;

    private const float COOLDOWN = 2.0f;

        // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        AttackStep();
    }

    public void SetState(State state)
    {
        return;
    }

    public void AttackStep()
    {

        if (isRunning) {

            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                timer = COOLDOWN; //Reset Timer
                int enemyDirection = sensor.GetNearestSensedBotDirection();
    
                projectileStartPos.transform.localPosition = new Vector3(enemyDirection * attackDistance, 0, 0);
                //Faces attack at enemy, handled as local position to bot part

                GameObject projectileInstance = Instantiate(projectile, projectileStartPos.transform.position, Quaternion.identity);
                //Create a projectile at the start position

                Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
                //Fetch script/data for projectile

                projectileScript.enemyDirection = enemyDirection; //Set the direction of the projectile
                projectileScript.damage = damage; //Set the damage of the projectile
                projectileScript.speed = projectileSpeed; //Set the speed of the projectile
                projectileScript.enemyLayer = enemyLayer;
                //Set the target of the projectile, so it only hits the enemy bot

                projectileInstance.gameObject.transform.localScale = projectileSize;
                //Set the projectile size
                //TODO: Set projectile knockback


                controller.PlayAudio("Hit");
            }
        }
    }
}

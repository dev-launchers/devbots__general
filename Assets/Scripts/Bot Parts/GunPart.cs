using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPart : BotPart
{

    [SerializeField] private int enemyLayer = default(int);
    [SerializeField] private float damage = default(float);
    [SerializeField] private GameObject projectile = default(GameObject); //Object to be fired by gun part
    [SerializeField] private float projectileSpeed = default(float);
    [SerializeField] private GameObject projectileStartPos;
    [SerializeField] private Vector2 projectileSize = default(Vector2);

    private BotSensor sensor;
    private BotController controller;

    [SerializeField] private bool isRunning;
        // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponentInParent<BotSensor>();
        controller = GetComponentInParent<BotController>();
        enemyLayer = sensor.GetEnemyLayer();
        timer = GetCoolDown();
    }

    // Update is called once per frame
    void Update()
    {
        AttackStep();
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public void AttackStep()
    {

        if (isRunning) {

            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                timer = GetCoolDown(); //Reset Timer
                int enemyDirection = sensor.GetNearestSensedBotDirection();
    
                //Faces attack at enemy, handled as local position to bot part

                GameObject projectileInstance = Instantiate(projectile, projectileStartPos.transform.position, Quaternion.identity);
                //Create a projectile at the start position

                Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
                //Fetch script/data for projectile

                projectileScript.SetValues(enemyDirection, damage, projectileSpeed, projectileSize, enemyLayer);
                //Tells projectile values

                //TODO: Set projectile knockback

                controller.PlayAudio("Hit");
            }
        }
    }
}

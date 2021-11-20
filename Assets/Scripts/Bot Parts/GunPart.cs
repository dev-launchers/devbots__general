using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPart : BotPart
{

    private int enemyLayer;
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



    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public void AttackStep()
    {

        if (isRunning) {

            if (!IsPartCoolingDown()){
                ResetCooldownTimer();

                int enemyDirection = sensor.GetNearestSensedBotDirection();

                //Faces attack at enemy, handled as local position to bot part
                AttackActions.ProjectileAttack(
                    enemyDirection, 
                    projectileStartPos.transform, 
                    damage, 
                    projectileSpeed, 
                    enemyLayer, 
                    null, 
                    projectile, 
                    new Vector2(1, 1)
                    );

                GameObject projectileInstance = Instantiate(projectile, projectileStartPos.transform.position, Quaternion.identity);
                ////Create a projectile at the start position

                //Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
                ////Fetch script/data for projectile

                //projectileScript.SetValues(enemyDirection, damage, projectileSpeed, projectileSize, enemyLayer);
                ////Tells projectile values

                //TODO: Set projectile knockback

                //controller.PlayAudio("Hit");
            }
        }
    }

    public override void BotPartUpdate()
    {
        AttackStep();
    }
}

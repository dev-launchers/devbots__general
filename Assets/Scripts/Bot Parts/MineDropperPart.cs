using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDropperPart : BotPart
{

    private int enemyLayer;
    [SerializeField] private float damage = default(float);
    [SerializeField] private GameObject landmine; // gameobject to be dropped
    [SerializeField] private GameObject projectileStartPos;
    [SerializeField] private Vector3 projectileSize = default(Vector3);

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

    public void BackStep()
    {
        // drop landmines
        if(isRunning)
        {
            if (!IsPartCoolingDown())
            {
                ResetCooldownTimer();

                //Create a landmine at the start position
                GameObject landmineInstance = Instantiate(landmine, projectileStartPos.transform.position, Quaternion.identity);

                //Fetch script/data for landmine
                Landmine projectileScript = landmineInstance.GetComponent<Landmine>();

                //Tells landmine values
                projectileScript.SetValues(damage, projectileSize, enemyLayer);
                

                //TODO: Set landmine knockback

                controller.PlayAudio("Hit");
            }
        }
    }

    public override void SetState(State state)
    {
        isRunning = state.isActive;
    }

    public override void BotPartUpdate()
    {
        BackStep();
    }
}

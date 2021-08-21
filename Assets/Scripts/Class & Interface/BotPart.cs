using UnityEngine;

public abstract class BotPart : MonoBehaviour
{
    [SerializeField] private float coolDown; 
    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;
    private float timer = 0;

    public float GetCoolDown() {
        return coolDown;
    }

    public bool IsPartCoolingDown() {
        return timer > 0;
    }

    public void AdvanceCooldownTimer() {
        timer -= Time.deltaTime;
    }

    public void ResetCooldownTimer() {
        timer = coolDown;
    }

    public void Update() {
        AdvanceCooldownTimer();
    }

}

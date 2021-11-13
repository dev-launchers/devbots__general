using UnityEngine;

public abstract class BotPart : MonoBehaviour
{
    [SerializeField] private float coolDown;
    protected float timer;

    public abstract void BotPartUpdate();
    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;

    // These should be protected?
    public float GetCoolDownTimer(){ 
        return timer;
    }

    public float GetCoolDown() {
        return coolDown;
    }

    public bool IsPartCoolingDown() {
        return timer > 0;
    }

    private void AdvanceCooldownTimer() {
        timer -= Time.deltaTime;
    }

    public void ResetCooldownTimer() {
        timer = coolDown;
    }

    private void Update() {
        AdvanceCooldownTimer();
        BotPartUpdate();
    }


}

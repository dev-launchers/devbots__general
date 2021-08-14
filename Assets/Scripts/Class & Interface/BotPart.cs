using UnityEngine;

public abstract class BotPart : MonoBehaviour
{
    [SerializeField] private float coolDown;
    internal float timer;
    public float GetTimer { get { return timer; } }
    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;

    public float GetCoolDown(){
        return coolDown;
    }
}

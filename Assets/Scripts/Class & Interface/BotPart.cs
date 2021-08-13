using UnityEngine;

public abstract class BotPart : MonoBehaviour
{
    [SerializeField] private float coolDown; 
    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;

    public float GetCoolDown(){
        return coolDown;
    }
}

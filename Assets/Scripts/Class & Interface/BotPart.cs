using UnityEngine;

public abstract class BotPart : MonoBehaviour
{
    [SerializeField] public float coolDown; 
    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;
}

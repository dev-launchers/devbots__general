using UnityEngine;

public abstract class BotPart : MonoBehaviour
{

    abstract public void SetState(State state);
    //[SerializeField] abstract private bool isRunning;

}

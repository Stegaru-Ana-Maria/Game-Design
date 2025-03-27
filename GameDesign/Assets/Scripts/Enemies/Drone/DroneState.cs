using UnityEngine;

public abstract class DroneState
{
    protected DroneFSM enemy;

    public DroneState(DroneFSM enemy)
    {
        this.enemy = enemy;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

}

using UnityEngine;

public class DoorTriggerButton : MonoBehaviour
{
    [SerializeField] private DoorSetActive door;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            door.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            door.CloseDoor();
        }
    }


}

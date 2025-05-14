using UnityEngine;

public class FirstBoosRoomDoorController : MonoBehaviour
{
    [SerializeField] GameObject door;

    private void Awake()
    {

    }

    private void Start()
    {
        door.SetActive(false);
    }

    public void OpenDoor()
    {
        door.SetActive(false);
    }

    public void CloseDoor()
    {
        door.SetActive(true);
    }
}

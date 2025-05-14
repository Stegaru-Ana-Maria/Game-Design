using UnityEngine;

public class FirstBoosRoomDoorController : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject invisibleWall;
   
    private void Awake()
    {

    }

    private void Start()
    {
        door.SetActive(true);
        invisibleWall.SetActive(true);
    }

    public void OpenDoor()
    {
        door.SetActive(false);
        invisibleWall.SetActive(true);
    }

    public void CloseDoor()
    {
        door.SetActive(true);
        invisibleWall.SetActive(false);
    }
}

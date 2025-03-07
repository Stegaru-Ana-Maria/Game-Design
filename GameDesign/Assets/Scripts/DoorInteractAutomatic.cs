using System.Xml.Serialization;
using UnityEngine;

public class DoorInteractAutomatic : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;
    private IDoor door;

    private void Awake()
    {
        door = doorGameObject.GetComponent<IDoor>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerMovement>() != null)
        {
            door.OpenDoor();
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerMovement>() != null)
        {
            door.OpenDoor();
        }
    }

}

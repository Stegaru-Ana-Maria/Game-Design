using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private Boss1AI bossAI;
    [SerializeField] FirstBoosRoomDoorController door;
    Health playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player-ul a intrat in camera boss-ului!");
            playerHealth = other.gameObject.GetComponent<Health>();
            door.CloseDoor();
            bossAI.SetPlayerInRoom(true);
        }
    }
    private void Update()
    {
        if (playerHealth != null && playerHealth.isPlayerDead() == true)
        {
            door.OpenDoor();
        }
    }
}

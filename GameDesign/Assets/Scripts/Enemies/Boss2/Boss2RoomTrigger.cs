using UnityEngine;

public class Boss2RoomTrigger : MonoBehaviour
{
    [SerializeField] private Boss2AI bossAI;
    [SerializeField] private AudioClip bossMusic;
    private bool hasTriggered = false;
    public DoorController door;
    Health playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player-ul a intrat in camera boss-ului!");
            playerHealth = other.gameObject.GetComponent<Health>();
            bossAI.SetPlayerInRoom(true);
            MusicManager.ChangeMusic(bossMusic);
            hasTriggered = true;
            door.CloseDoor();
        }
    }

    private void Update()
    {
        if(hasTriggered == true && playerHealth != null && playerHealth.isPlayerDead() == true)
        {
            hasTriggered = false;
            door.SetDoorToOpen();
        }
    }
}

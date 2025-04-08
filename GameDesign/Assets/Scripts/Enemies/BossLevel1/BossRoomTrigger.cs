using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private Boss1AI bossAI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player-ul a intrat in camera boss-ului!");
            bossAI.SetPlayerInRoom(true);
        }
    }
}

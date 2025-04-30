using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            Transform destination = collision.GetComponent<Teleporter>().GetDestination();
            if (destination != null)
            {
                SoundEffectManager.Play("Teleport");
                transform.position = destination.position;
            }
        }
    }
}

using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool playerNear = false;
    private bool isActivated = false;
    private Animator animator;
    public DoorController door;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            isActivated = true;
            animator.SetBool("isActivated", true); 
            door.SetDoorToOpen();
        }
    }
}

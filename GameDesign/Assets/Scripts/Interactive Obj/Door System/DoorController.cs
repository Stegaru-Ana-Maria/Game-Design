using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    public bool isOpen = false;
    public bool doorShouldOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool("open", true);
    }

    public void CloseDoor() 
    {
        animator.SetBool("open", false);
        SoundEffectManager.Play("CloseDoor");
    }

    public void SetDoorToOpen()
    {
        doorShouldOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (doorShouldOpen && other.CompareTag("Player"))
        {
            isOpen = true;
            animator.SetBool("open", isOpen);
            doorShouldOpen = false;
        }
    }
}

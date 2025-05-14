using UnityEngine;

public class NextLevelDoorController : MonoBehaviour
{
    [SerializeField] BossHealth boss;
    [SerializeField] GameObject door;

    private void Start()
    {
        door.SetActive(true);
    }

    private void Update()
    {
        if(boss.GetHealth() <= 0)
        {
            door.SetActive(false);
        }
    }
}

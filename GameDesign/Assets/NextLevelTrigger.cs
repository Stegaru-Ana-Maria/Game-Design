using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{

    private bool hasTriggered = false;
    [SerializeField] int nextLevelScene = 2;

    private void Awake()
    {
        //SaveProgressManager.instance.AssignNextLevelTrigger(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            SaveProgressManager.instance.SaveGame();
            SaveProgressManager.instance.StartLevel(nextLevelScene);
        }
    }
}

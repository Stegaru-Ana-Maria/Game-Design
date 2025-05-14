using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneChange : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

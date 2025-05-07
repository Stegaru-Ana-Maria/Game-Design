using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    [Header("MENU OBJECTS")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;
    [SerializeField] GameObject titleScreenLoadOptions;

    [Header("BUTTONS")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button loadMenuIntroductionButton;
    [SerializeField] Button mainMenuLoadGameButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenLoadGameMenu()
    {
        // close main menu
        titleScreenMainMenu.SetActive(false);
        // open load menu
        titleScreenLoadMenu.SetActive(true);
        loadMenuIntroductionButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        // close load menu
        titleScreenLoadMenu.SetActive(false);
        // open main menu
        titleScreenMainMenu.SetActive(true);

    }

    public void OpenLoadGameOptions()
    {
        // close main menu
        titleScreenMainMenu.SetActive(false);
        // open load menu
        titleScreenLoadOptions.SetActive(true);
       // loadMenuIntroductionButton.Select();
    }

    public void CloseLoadGameOptions()
    {
        // close load menu
        titleScreenLoadOptions.SetActive(false);
        // open main menu
        titleScreenMainMenu.SetActive(true);

    }
}

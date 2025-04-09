using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level_Load : MonoBehaviour
{
    [SerializeField] Button loadLevelButton;

    [Header("GAME STATUS")]
    public Levels level;
    private bool unlocked;

    private void Awake()
    {
        GetCurrentProgression();

        loadLevelButton.interactable = unlocked;

        if (loadLevelButton.interactable)
        {
            loadLevelButton.onClick.AddListener(LoadLevel);
        }
    }

    private void GetCurrentProgression()
    {
        if (level == Levels.Testing)
        {
            unlocked = SaveProgressManager.instance.currentProgression.testing_unlocked;
        }
        else if (level == Levels.Protoype)
        {
            unlocked = SaveProgressManager.instance.currentProgression.prototype_unlocked;
        }
        else if (level == Levels.Level_00)
        {
            unlocked = SaveProgressManager.instance.currentProgression.level_00_unlocked;
        }
        else if (level == Levels.Level_01)
        {
            unlocked = SaveProgressManager.instance.currentProgression.level_01_unlocked;
        }
        else if (level == Levels.Level_02)
        {
            unlocked = SaveProgressManager.instance.currentProgression.level_02_unlocked;
        }
    }

    private void OnEnable()
    {

    }

    private void LoadLevel(){

        if(level == Levels.Testing)
        {
            SaveProgressManager.instance.StartLevel(SaveProgressManager.instance.testing_scene_index);
        }
        else if (level == Levels.Protoype)
        {
            SaveProgressManager.instance.StartLevel(SaveProgressManager.instance.prototype_scene_index);
        }
        else if (level == Levels.Level_00)
        {
            SaveProgressManager.instance.StartLevel(SaveProgressManager.instance.level_00_scene_index);
        }
        else if (level == Levels.Level_01)
        {
            SaveProgressManager.instance.StartLevel(SaveProgressManager.instance.level_01_scene_index);
        }
        else if (level == Levels.Level_02)
        {
            SaveProgressManager.instance.StartLevel(SaveProgressManager.instance.level_02_scene_index);
        }
    }
}

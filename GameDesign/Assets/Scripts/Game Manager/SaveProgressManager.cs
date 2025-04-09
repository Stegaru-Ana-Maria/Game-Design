using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveProgressManager : MonoBehaviour
{
    public static SaveProgressManager instance;

    public NextLevelTrigger nextLevelTrigger;

    [Header("SAVE GAME")]
    [SerializeField] bool saveGame;

    [Header("SAVE DATA WRITER")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("CURRENT LEVEL PROGRESSION")]
    public LevelProgressionData currentProgression;

    [Header("SCENE INDEX")]
    public int currentSceneIndex = 0;

    [Header("LEVEL SCENE INDEX")]
    public int testing_scene_index;
    public int prototype_scene_index;
    public int level_00_scene_index;
    public int level_01_scene_index;
    public int level_02_scene_index;

    private void Awake()
    {
        // there can only be one instance of this script at one time, if another one exists, destroy it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public void AssignNextLevelTrigger(NextLevelTrigger newTrigger)
    //{
    //    if (newTrigger == null)
    //    {
    //        Debug.LogError("Failed to assign the trigger to SaveProgressManager!");
    //        return;
    //    }

    //    nextLevelTrigger = newTrigger;
    //    Debug.Log("Trigger assigned successfully to the SaveProgressManager.");
    //}

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadLevelProgression();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }
    }

    public void StartLevel(int newSceneIndex)
    {
        StartCoroutine(LoadLevelScene(newSceneIndex));
    }

    private void LoadLevelProgression()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // if this save file was not created yet, create one now
            currentProgression = new LevelProgressionData();
            saveFileDataWriter.CreateSaveFile(currentProgression);
        }
        else
        {
            currentProgression = saveFileDataWriter.LoadSaveFile();
        }
    }

    public void SaveGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        switch (currentSceneIndex)
        {
            case 1:
                currentProgression.level_01_unlocked = true;
                break;
            case 2:
                currentProgression.level_02_unlocked = true;
                break;
            default:
                break;
        }
        //write that info onto a json file, saved to this machine
        saveFileDataWriter.UpdateProgresisonFile(currentProgression);
    }

    public IEnumerator LoadLevelScene(int newSceneIndex)
    {
        currentSceneIndex = newSceneIndex;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneIndex);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
    public int GetSceneIndex()
    {
        return currentSceneIndex;
    }
}

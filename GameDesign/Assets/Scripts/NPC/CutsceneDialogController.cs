using UnityEngine;
using TMPro;

public class CutsceneDialogController : MonoBehaviour
{
    public TextMeshProUGUI playerDialogText;
    public TextMeshProUGUI bossDialogText;

    public void ShowPlayerDialog(string text)
    {
        playerDialogText.text = text;
    }

    public void ShowBossDialog(string text)
    {
        bossDialogText.text = text;
    }
}

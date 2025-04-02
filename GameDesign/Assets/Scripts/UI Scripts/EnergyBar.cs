using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Image totalEnergyBar;
    [SerializeField] private Image currentEnergyBar;

    private void Start()
    {
        totalEnergyBar.fillAmount = playerAttack.maxEnergy/50;
    }
    private void Update()
    {
        currentEnergyBar.fillAmount = ((float)(playerAttack.currentEnergy)/50);
    }
}
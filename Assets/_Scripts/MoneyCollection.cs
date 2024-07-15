using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MoneyCollection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private int currentMoney = 0;
    [SerializeField] private int upgradeCost = 20;
    [SerializeField] private UnityEvent upgradeEvent;

    public void AddMoney(int value)
    {
        currentMoney += value;

        UpdateText();
    }

    public void BuyStackUpgrade()
    {
        if (currentMoney < upgradeCost)
        {
            return;
        }

        currentMoney -= upgradeCost;

        upgradeEvent.Invoke();

        UpdateText();
    }

    private void UpdateText()
    {
        textUI.text = currentMoney.ToString();
    }
}

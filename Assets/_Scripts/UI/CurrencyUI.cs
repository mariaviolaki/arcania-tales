using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
	[SerializeField] TMP_Text currencyText;
	[SerializeField] GameplaySettingsSO settings;

    void Start()
    {
		UpdateCurrencyText(settings.StartGold);
    }

    void UpdateCurrencyText(int newAmount)
	{
		currencyText.text = newAmount.ToString();
	}
}

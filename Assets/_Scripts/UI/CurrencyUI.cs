using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
	[SerializeField] TMP_Text currencyText;
	[SerializeField] GameplaySettingsSO settings;
	[SerializeField] PlayerWallet playerWallet;

    void Start()
    {
		playerWallet.OnGoldChange += UpdateCurrencyText;

		UpdateCurrencyText();
    }

	void OnDestroy()
	{
		playerWallet.OnGoldChange -= UpdateCurrencyText;
	}

	void UpdateCurrencyText()
	{
		currencyText.text = playerWallet.Gold.ToString();
	}
}

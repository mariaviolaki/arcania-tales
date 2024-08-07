using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[SerializeField] GameObject imageContainer;
	[SerializeField] GameObject textContainer;

	Image itemImage;
	TMP_Text quantityText;

	void Awake()
	{
		itemImage = imageContainer.GetComponent<Image>();
		quantityText = textContainer.GetComponent<TMP_Text>();
	}

	public void Fill(Sprite sprite, int quantity)
	{
		itemImage.sprite = sprite;
		quantityText.text = quantity.ToString();

		imageContainer.SetActive(true);
		textContainer.SetActive(true);
	}

	public void Empty()
	{
		itemImage.sprite = null;
		quantityText.text = "";

		imageContainer.SetActive(false);
		textContainer.SetActive(false);
	}
}

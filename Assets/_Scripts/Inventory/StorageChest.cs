using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour, IInteractable
{
	[SerializeField] SpriteRenderer storageImage;
	[SerializeField] Sprite closedStorageSprite;
	[SerializeField] Sprite openStorageSprite;
	
	UIManager uiManager;

	bool isOpen;

    void Awake()
    {
		isOpen = false;
    }

	void Start()
	{
		uiManager = FindObjectOfType<UIManager>();
	}

	public void Interact(Transform player)
	{
		ToggleStorage(true);

		if (uiManager != null)
		{
			uiManager.OpenStorage(this);
		}
	}
	
	public void ToggleStorage(bool isOpen)
	{
		this.isOpen = isOpen;
		storageImage.sprite = this.isOpen ? openStorageSprite : closedStorageSprite;
	}
}

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
		if (uiManager != null)
		{
			uiManager.OnCloseStorage += () => ToggleStorage(false);
		}
	}

	void OnDestroy()
	{
		if (uiManager != null)
		{
			uiManager.OnCloseStorage -= () => ToggleStorage(false);
		}
	}

	public void Interact(Transform player)
	{
		ToggleStorage(true);

		if (uiManager != null)
		{
			uiManager.ChangeUIState(GameEnums.UIState.Storage);
		}
	}

	void ToggleStorage(bool isOpen)
	{
		this.isOpen = isOpen;
		storageImage.sprite = this.isOpen ? openStorageSprite : closedStorageSprite;

		if (uiManager != null && uiManager.StorageUI != null)
		{
			uiManager.StorageUI.CurrentStorage = isOpen ? this : null;
		}
	}
}

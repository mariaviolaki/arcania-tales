using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] GameManagerSO gameManager;
	[SerializeField] WorldFadeUI worldFadeUI;
	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] StorageUI storageUI;
	[SerializeField] SelectedItemUI selectedItemUI;
	[SerializeField] ShopUI shopUI;
	[SerializeField] DialogueUI dialogueUI;
	[SerializeField] HintUI hintUI;

	GameEnums.UIState previousState;
	GameEnums.UIState currentState;

	public GameEnums.UIState CurrentState { get { return currentState; } }
	public GameEnums.UIState PreviousState { get { return previousState; } }

	// Provide a way to get the different UI components because they might be currently disabled in the scene
	public ToolbarUI Toolbar { get { return toolbarUI; } }
	public InventoryUI Inventory { get { return inventoryUI; } }
	public StorageUI Storage { get { return storageUI; } }
	public SelectedItemUI SelectedItem { get { return selectedItemUI; } }
	public ShopUI Shop { get { return shopUI; } }
	public DialogueUI Dialogue { get { return dialogueUI; } }
	public HintUI Hint { get { return hintUI; } }

	void Awake()
	{
		previousState = GameEnums.UIState.Toolbar;
		currentState = GameEnums.UIState.None;
	}

	void Start()
	{
		toolbarUI.SetUIManager(this);

		InitListeners();
		
		ChangeUIState(GameEnums.UIState.Toolbar);
	}

	void OnDestroy()
	{
		shopUI.OnCloseShopUI -= () => ChangeUIState(GameEnums.UIState.Toolbar);

		selectedItemUI.OnShowSelectedItem -= () => ChangeUIState(GameEnums.UIState.ItemSelection);
		selectedItemUI.OnReleaseItem -= () => ChangeUIState(GameEnums.UIState.None);

		dialogueUI.OnOpenDialogueUI -= () => ChangeUIState(GameEnums.UIState.Dialogue);
		dialogueUI.OnCloseDialogueUI -= () => ChangeUIState(GameEnums.UIState.Toolbar);

		hintUI.OnOpenHintUI -= () => ChangeUIState(GameEnums.UIState.Hint);
		hintUI.OnCloseHintUI -= () => ChangeUIState(GameEnums.UIState.Toolbar);
	}

	void InitListeners()
	{
		// Setup the toolbar button to open or close the inventory
		EventTrigger eventTrigger = toolbarUI.InventoryButton.GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => ToggleInventory((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);

		shopUI.OnCloseShopUI += () => ChangeUIState(GameEnums.UIState.Toolbar);

		selectedItemUI.OnShowSelectedItem += () => ChangeUIState(GameEnums.UIState.ItemSelection);
		selectedItemUI.OnReleaseItem += () => ChangeUIState(GameEnums.UIState.None); // revert to the previous state

		dialogueUI.OnOpenDialogueUI += () => ChangeUIState(GameEnums.UIState.Dialogue);
		dialogueUI.OnCloseDialogueUI += () => ChangeUIState(GameEnums.UIState.Toolbar);

		hintUI.OnOpenHintUI += () => ChangeUIState(GameEnums.UIState.Hint);
		hintUI.OnCloseHintUI += () => ChangeUIState(GameEnums.UIState.Toolbar);
	}

	void ToggleInventory(PointerEventData eventData)
	{
		if (currentState == GameEnums.UIState.Toolbar)
		{
			ChangeUIState(GameEnums.UIState.Inventory);
		}
		else if (currentState == GameEnums.UIState.Inventory)
		{
			ChangeUIState(GameEnums.UIState.Toolbar);
		}
		else if (currentState == GameEnums.UIState.Storage)
		{
			storageUI.SetCurrentStorage(null);
			ChangeUIState(GameEnums.UIState.Toolbar);
		}
	}

	public void OpenStorage(StorageChest storageChest)
	{
		storageUI.SetCurrentStorage(storageChest);
		ChangeUIState(GameEnums.UIState.Storage);
	}

	public void OpenShop(ShopCounter shopCounter)
	{
		shopUI.SetCurrentShop(shopCounter);
		ChangeUIState(GameEnums.UIState.Shop);
	}

	void ChangeUIState(GameEnums.UIState newState)
	{
		if (newState == currentState) return;

		// The current state can never be -None-
		newState = (newState == GameEnums.UIState.None) ? previousState : newState;

		// The previous state can never be transitional
		previousState = IsTransitionalState(currentState) ? previousState : currentState;
		currentState = newState;

		worldFadeUI.SetActive(IsWorldFadeState(currentState, previousState));
		gameManager.SetGamePaused(newState != GameEnums.UIState.Toolbar);

		if (newState == GameEnums.UIState.ItemSelection)
		{
			selectedItemUI.SetActive(true);
		}
		else if (newState == GameEnums.UIState.Toolbar)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(false);
			storageUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
			shopUI.SetActive(false);
			hintUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Inventory)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(true);
			storageUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
			shopUI.SetActive(false);
			hintUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Storage)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(true);
			storageUI.SetActive(true);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
			shopUI.SetActive(false);
			hintUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Dialogue)
		{
			toolbarUI.SetActive(false);
			inventoryUI.SetActive(false);
			storageUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(true);
			shopUI.SetActive(false);
			hintUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Shop)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(true);
			storageUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
			shopUI.SetActive(true);
			hintUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Hint)
		{
			toolbarUI.SetActive(false);
			inventoryUI.SetActive(false);
			storageUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
			shopUI.SetActive(false);
			hintUI.SetActive(true);
		}
	}

	bool IsTransitionalState(GameEnums.UIState uiState)
	{
		return uiState == GameEnums.UIState.None || uiState == GameEnums.UIState.ItemSelection;
	}

	bool IsWorldFadeState(GameEnums.UIState uiState, GameEnums.UIState oldState)
	{
		if (uiState == GameEnums.UIState.ItemSelection) return oldState != GameEnums.UIState.Toolbar;

		return uiState != GameEnums.UIState.Toolbar && uiState != GameEnums.UIState.Dialogue;
	}
}

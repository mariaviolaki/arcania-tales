using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager: MonoBehaviour
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] GameManagerSO gameManager;
	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] SelectedItemUI selectedItemUI;
	[SerializeField] DialogueUI dialogueUI;

	InventoryUIManager inventoryUIManager;
	GameEnums.UIState previousState;
	GameEnums.UIState currentState;

	public GameEnums.UIState CurrentState { get { return currentState; } }
	public GameEnums.UIState PreviousState { get { return previousState; } }

	void Awake()
	{
		previousState = GameEnums.UIState.Toolbar;
		currentState = GameEnums.UIState.None;
	}

	void Start()
	{
		inventoryUIManager = new InventoryUIManager(inventorySettings, toolbarUI, inventoryUI, selectedItemUI);
		toolbarUI.SetUIManager(this);

		InitListeners();
		
		ChangeUIState(GameEnums.UIState.Toolbar);
	}

	void OnDestroy()
	{
		dialogueUI.OnOpenDialogueUI -= () => ChangeUIState(GameEnums.UIState.Dialogue);
		dialogueUI.OnCloseDialogueUI -= () => ChangeUIState(GameEnums.UIState.Toolbar);

		selectedItemUI.OnShowSelectedItem -= () => ChangeUIState(GameEnums.UIState.ItemSelection);
		selectedItemUI.OnReleaseSelectedItem -= () => ChangeUIState(GameEnums.UIState.None);
	}

	void InitListeners()
	{
		// Setup the toolbar button to open or close the inventory
		EventTrigger eventTrigger = toolbarUI.InventoryButton.GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => ToggleInventory((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);

		dialogueUI.OnOpenDialogueUI += () => ChangeUIState(GameEnums.UIState.Dialogue);
		dialogueUI.OnCloseDialogueUI += () => ChangeUIState(GameEnums.UIState.Toolbar);

		selectedItemUI.OnShowSelectedItem += () => ChangeUIState(GameEnums.UIState.ItemSelection);
		selectedItemUI.OnReleaseSelectedItem += () => ChangeUIState(GameEnums.UIState.None); // revert to the previous state
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
	}

	void ChangeUIState(GameEnums.UIState newState)
	{
		if (newState == currentState) return;

		// The current state can never be -None-
		newState = (newState == GameEnums.UIState.None) ? previousState : newState;

		// The previous state can never be transitional
		previousState = IsTransitionalState(currentState) ? previousState : currentState;
		currentState = newState;

		gameManager.SetGamePaused(newState != GameEnums.UIState.Toolbar);

		if (newState == GameEnums.UIState.ItemSelection)
		{
			selectedItemUI.SetActive(true);
		}
		else if (newState == GameEnums.UIState.Toolbar)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Inventory)
		{
			toolbarUI.SetActive(true);
			inventoryUI.SetActive(true);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(false);
		}
		else if (newState == GameEnums.UIState.Dialogue)
		{
			toolbarUI.SetActive(false);
			inventoryUI.SetActive(false);
			selectedItemUI.SetActive(false);
			dialogueUI.SetActive(true);
		}
	}

	bool IsTransitionalState(GameEnums.UIState uiState)
	{
		return uiState == GameEnums.UIState.None || uiState == GameEnums.UIState.ItemSelection;
	}
}

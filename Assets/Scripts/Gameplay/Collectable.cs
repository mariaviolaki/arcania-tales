using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
	void IInteractable.Interact()
	{
		Debug.Log("Interacting with: " + name);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectManager : MonoBehaviour
{
	[Tooltip("A container of objects which will persist across scenes")]
	[SerializeField] GameObject objectContainer;

	static bool isCreated = false;

    void Awake()
    {
		if (isCreated) return;

		InitPersistentObject();
	}

	void InitPersistentObject()
	{
		GameObject persistentObject = Instantiate(objectContainer);
		DontDestroyOnLoad(persistentObject);
		isCreated = true;
	}
}

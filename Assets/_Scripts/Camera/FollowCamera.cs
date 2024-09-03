using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	void Start()
	{
		CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
		PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
		GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();

		sceneManager.OnEndChangeScene += UpdateBounds;

		if (virtualCamera != null && playerMovement != null)
		{
			UpdateBounds(Vector2.zero);
		}
	}

	void UpdateBounds(Vector2 sceneEntryPoint)
	{
		GetComponent<CinemachineConfiner>().m_BoundingShape2D = FindObjectOfType<TerrainGrid>().GridCollider;
	}
}

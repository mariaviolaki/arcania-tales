using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineConfiner))]
public class FollowCamera : MonoBehaviour
{
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] PlayerMovement playerMovement;

	CinemachineConfiner cameraConfiner;

	void Start()
	{
		cameraConfiner = GetComponent<CinemachineConfiner>();

		sceneManager.OnEndChangeScene += UpdateBounds;
		UpdateBounds(Vector2.zero);
	}

	void OnDestroy()
	{
		sceneManager.OnEndChangeScene -= UpdateBounds;
	}

	void UpdateBounds(Vector2 sceneEntryPoint)
	{
		cameraConfiner.m_BoundingShape2D = FindObjectOfType<TerrainGrid>().GridCollider;
	}
}

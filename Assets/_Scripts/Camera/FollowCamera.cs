using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
[RequireComponent(typeof(CinemachineConfiner))]
public class FollowCamera : MonoBehaviour
{
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] PlayerMovement playerMovement;

	CinemachineVirtualCamera virtualCamera;
	CinemachineConfiner cameraConfiner;

	void Awake()
	{
		virtualCamera = GetComponent<CinemachineVirtualCamera>();
		cameraConfiner = GetComponent<CinemachineConfiner>();
	}

	void Start()
	{
		sceneManager.OnEndChangeScene += ChangeSceneView;
		UpdateBounds();
	}

	void OnDestroy()
	{
		sceneManager.OnEndChangeScene -= ChangeSceneView;
	}

	void ChangeSceneView(Vector2 sceneEntryPoint)
	{
		UpdateBounds();
		virtualCamera.ForceCameraPosition(sceneEntryPoint, Quaternion.identity);
	}

	void UpdateBounds()
	{
		cameraConfiner.m_BoundingShape2D = FindObjectOfType<TerrainGrid>().GridCollider;
	}
}

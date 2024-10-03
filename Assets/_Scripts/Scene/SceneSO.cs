using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene", menuName = "Scriptable Objects/Scene")]
public class SceneSO : ScriptableObject
{
	[SerializeField] GameEnums.Scene scene;
	[SerializeField] List<GamePosition> adjacentScenes;

	public GameEnums.Scene Scene { get { return scene; } }
	public List<GamePosition> AdjacentScenes { get { return adjacentScenes; } }
}

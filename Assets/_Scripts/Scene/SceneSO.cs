using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene", menuName = "Scriptable Objects/Scene")]
public class SceneSO : ScriptableObject
{
	[SerializeField] GameEnums.Scene scene;
	[SerializeField] List<SceneEntrySO> entryPoints;

	public GameEnums.Scene Scene { get { return scene; } }
	public List<SceneEntrySO> EntryPoints { get { return entryPoints; } }
}

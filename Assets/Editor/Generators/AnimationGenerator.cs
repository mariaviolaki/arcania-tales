using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AnimationGenerator : MonoBehaviour
{
	void Start()
	{
		// This path must be below a folder named "Resources"
		const string spritePath = "Sprites/Characters/Body";
		const string spritePart = "Body";

		string[] animDirs =
		{
			"Left", "Front", "Back", "Right",
			"Left", "Front", "Back", "Right",
			"Left", "Front", "Back", "Right"
		};

		Sprite[] allSprites = Resources.LoadAll<Sprite>(spritePath);
		Dictionary<string, List<Sprite>> animations = new Dictionary<string, List<Sprite>>();

		for (int i = 0; i < allSprites.Length; i++)
		{
			// Don't include sprite sheets in animations
			Sprite sprite = allSprites[i];
			if (!sprite.name.Contains("_")) continue;

			// Only create a default animation for the first skin
			int spriteNum = Convert.ToInt32(sprite.name.Split("_")[0].Substring(spritePart.Length));
			if (spriteNum != 0) continue;

			int spriteSliceNum = Convert.ToInt32(sprite.name.Split("_")[1]);
			string spriteDirection = animDirs[spriteSliceNum];

			// All sprites in the spritesheet will be used in walk animations
			string animName = spritePart + "Walk" + spriteDirection;
			if (animations.ContainsKey(animName))
			{
				animations[animName].Add(sprite);
			}
			else
			{
				animations.Add(animName, new List<Sprite> { sprite });
			}

			// The first 4 sprites are also used for idle animations
			if ((new[] { 0, 1, 2, 3 }).Contains(spriteSliceNum))
			{
				animName = spritePart + "Idle" + spriteDirection;
				animations.Add(animName, new List<Sprite> { sprite });
			}
			// Create an idle frame between walking frames
			else if ((new[] { 4, 5, 6, 7 }).Contains(spriteSliceNum))
			{
				animName = spritePart + "Walk" + spriteDirection;
				animations[animName].Add(animations[animName][0]);
			}
		}

		// Create animations for all the sprites for this body part
		foreach (KeyValuePair<string, List<Sprite>> animData in animations)
		{
			string animName = animData.Key;
			List<Sprite> spriteFrames = animData.Value;

			AnimationClip newClip = new AnimationClip();

			// Create and apply clip settings
			AnimationClipSettings newSettings = new AnimationClipSettings();
			newSettings.loopTime = true;
			AnimationUtility.SetAnimationClipSettings(newClip, newSettings);

			// Create initial binding
			EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");

			// Make the actual clip itself
			float interval = 0.25f;
			ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[spriteFrames.Count + 1];
			for (int i = 0; i < keyframes.Length; i++)
			{
				if (i == keyframes.Length - 1)
				{
					keyframes[i].time = i * interval;
					keyframes[i].value = spriteFrames[0];
				}
				else
				{
					keyframes[i].time = i * interval;
					keyframes[i].value = spriteFrames[i];
				}
			}

			// Save it
			AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyframes);

			string savePath = "Assets/Animations/" + spritePart;
			AssetDatabase.CreateAsset(newClip, $"{savePath}/" + animName + ".anim");
			AssetDatabase.SaveAssets();
		}
	}
}

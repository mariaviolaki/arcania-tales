using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] Image transitionImage;
	[SerializeField] float fadeInSpeed = 1f;

	Color imageColor;
	Coroutine currentTransition;

    void Awake()
    {
		sceneManager.OnBeginChangeScene += FadeOut;
		sceneManager.OnEndChangeScene += FadeIn;

		imageColor = transitionImage.color;
    }

	public void FadeOut()
	{
		StopFadeTransition();		
		currentTransition = StartCoroutine(BeginFadeTransition(Color.clear, imageColor, gameplaySettings.SceneTransitionDelay));
	}

	public void FadeIn(Vector2 entryPoint)
	{
		StopFadeTransition();
		currentTransition = StartCoroutine(BeginFadeTransition(imageColor, Color.clear, fadeInSpeed));
	}

	IEnumerator BeginFadeTransition(Color startColor, Color endColor, float fadeSpeed)
	{
		transitionImage.enabled = true;

		float percentage = 0f;
		while (percentage < 1f)
		{
			percentage += Time.deltaTime * fadeSpeed;
			Color currentColor = Color.Lerp(startColor, endColor, percentage);
			transitionImage.color = currentColor;

			yield return null;
		}
	}

	void StopFadeTransition()
	{
		if (currentTransition != null)
		{
			StopCoroutine(currentTransition);
		}

		transitionImage.enabled = false;
	}
}

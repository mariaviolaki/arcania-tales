using System.Collections;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;
	Transform targetPlayer;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

		targetPlayer = collision.gameObject.transform;
		StartCoroutine(FollowPlayer());
	}

	IEnumerator FollowPlayer()
	{
		while (Vector2.Distance(transform.root.position, targetPlayer.position) > gameplaySettings.MagnetDistance)
		{
			float moveSpeed = gameplaySettings.MagnetForce * Time.deltaTime;
			transform.root.position = Vector2.MoveTowards(transform.root.position, targetPlayer.position, moveSpeed);

			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}
}

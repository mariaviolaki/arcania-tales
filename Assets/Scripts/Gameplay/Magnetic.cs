using System.Collections;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
	const float PULL_SPEED = 5f;
	const float MIN_DISTANCE = 0.1f;

	Transform targetPlayer;
	Collider2D cCollider;

	void Awake()
	{
		cCollider = GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

		targetPlayer = collision.gameObject.transform;
		StartCoroutine(FollowPlayer());
	}

	IEnumerator FollowPlayer()
	{
		while (Vector2.Distance(transform.root.position, targetPlayer.position) > MIN_DISTANCE)
		{
			float moveSpeed = PULL_SPEED * Time.deltaTime;
			transform.root.position = Vector2.MoveTowards(transform.root.position, targetPlayer.position, moveSpeed);

			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}
}

using UnityEngine;
using System.Collections;

public class NetworkSync : MonoBehaviour {
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 startSyncPosition;
	private Vector3 endSyncPosition;
	private Quaternion startSyncRotation;
	private Quaternion endSyncRotation;
	private float syncHealth = 0f;

	private PlayerDefense playerDefense;

	private bool initialized = false;

	void Start () {
		playerDefense = GetComponent<PlayerDefense>();
		endSyncRotation = Quaternion.identity;
		initialized = true;
	}

	void Update() {
		if (!networkView.isMine) {
			syncTime += Time.deltaTime;
			transform.position = Vector3.Lerp(startSyncPosition, endSyncPosition, syncTime / syncDelay);
			transform.rotation = Quaternion.Lerp(startSyncRotation, endSyncRotation, syncTime / syncDelay);
			playerDefense.health = syncHealth;
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (initialized) {
			Vector3 syncPosition = Vector3.zero;
			Quaternion syncRotation = Quaternion.identity;
			Vector3 syncVelocity = Vector3.zero;
			float syncHealth = 0f;

			if (stream.isWriting) {
				syncPosition = transform.position;
				syncRotation = transform.rotation;
				syncVelocity = rigidbody.velocity;
				syncHealth = playerDefense.health;
			}

			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncHealth);

			if (stream.isReading) {
				syncTime = 0f;
				float currentTime = Time.time;
				syncDelay = currentTime - lastSynchronizationTime;
				lastSynchronizationTime = currentTime;
				
				startSyncPosition = transform.position;
				startSyncRotation = transform.rotation;
				endSyncPosition = syncPosition + syncVelocity * syncDelay;
				endSyncRotation = syncRotation;
				this.syncHealth = syncHealth;
			}
		}
	}
}
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

	void Start () {
		endSyncRotation = Quaternion.identity;
	}

	void Update() {
		if (!networkView.isMine) {
			syncTime += Time.deltaTime;
			transform.position = Vector3.Lerp(startSyncPosition, endSyncPosition, syncTime / syncDelay);
			transform.rotation = Quaternion.Lerp(startSyncRotation, endSyncRotation, syncTime / syncDelay);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = transform.position;
			syncRotation = transform.rotation;
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			float currentTime = Time.time;
			syncDelay = currentTime - lastSynchronizationTime;
			lastSynchronizationTime = currentTime;
			
			startSyncPosition = transform.position;
			startSyncRotation = transform.rotation;
			endSyncPosition = syncPosition + syncVelocity * syncDelay;
			endSyncRotation = syncRotation;
		}
	}
}
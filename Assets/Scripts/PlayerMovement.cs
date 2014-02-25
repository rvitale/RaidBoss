using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	PlayerManager PMC_PlayerManagerClass;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 startSyncPosition;
	private Vector3 endSyncPosition;
	private Quaternion startSyncRotation;
	private Quaternion endSyncRotation;
	
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		if(networkView.isMine){
			gameObject.AddComponent<AudioListener>();
		}
	}

	void Update() {

		rigidbody.WakeUp();
		//movement and lookAt
		if (networkView.isMine) {
			CharacterController controller = GetComponent<CharacterController> ();
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), moveDirection.y, Input.GetAxis ("Vertical"));
			moveDirection.x *= speed;
			moveDirection.z *= speed;
			if (controller.isGrounded) {
					moveDirection.y = 0;
					if (Input.GetButton ("Jump")) {
							moveDirection.y = jumpSpeed;
							PMC_PlayerManagerClass.PlaySound ("jump");
					}

			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
			Vector3 faceDirection = new Vector3 (moveDirection.x, 0, moveDirection.z);
			if (faceDirection != Vector3.zero)
					transform.forward = Vector3.Normalize (faceDirection);
			PMC_PlayerManagerClass.lookDirection = faceDirection;
		} else {
			syncTime += Time.deltaTime;
			rigidbody.position = Vector3.Lerp(startSyncPosition, endSyncPosition, syncTime / syncDelay);
			rigidbody.rotation = Quaternion.Lerp(startSyncRotation, endSyncRotation, syncTime / syncDelay);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			syncRotation = rigidbody.rotation;
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
			
			startSyncPosition = rigidbody.position;
			startSyncRotation = rigidbody.rotation;
			endSyncPosition = syncPosition + syncVelocity * syncDelay;
			endSyncRotation = syncRotation;
		}
	}
}

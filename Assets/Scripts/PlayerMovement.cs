using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	private const float BaseSpeed = 6.0F;

	public float speed = BaseSpeed;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public bool canRotate = true;

	PlayerManager PMC_PlayerManagerClass;
	PlayerDefense PD_PlayerDefense;

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
		PD_PlayerDefense = GetComponent<PlayerDefense>();
		if(networkView.isMine){
			gameObject.AddComponent<AudioListener>();
		}
		endSyncRotation = Quaternion.identity;
	}

	void Update() {

		rigidbody.WakeUp();
		//movement and lookAt
		if (networkView.isMine ) {
			CharacterController controller = GetComponent<CharacterController> ();
			if(!PD_PlayerDefense.isDead){
				moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), moveDirection.y, Input.GetAxis ("Vertical"));
			}
			else{
				moveDirection = new Vector3 (0, moveDirection.y, 0);
			}
			float y = moveDirection.y;
			moveDirection.x *= speed;
			moveDirection.z *= speed;
			moveDirection.y = 0;
			moveDirection.Normalize();
			moveDirection *= speed;
			moveDirection = new Vector3(moveDirection.x, y, moveDirection.z);
			if (controller.isGrounded) {
					moveDirection.y = 0;
					if (Input.GetButton ("Jump")&& !PD_PlayerDefense.isDead) {
							moveDirection.y = jumpSpeed;
							PMC_PlayerManagerClass.PlaySound ("jump");
					}

			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
			if (canRotate) {
				Vector3 faceDirection = new Vector3 (moveDirection.x, 0, moveDirection.z);
				if (faceDirection != Vector3.zero)
						transform.forward = Vector3.Normalize (faceDirection);
				PMC_PlayerManagerClass.lookDirection = faceDirection;
			}
		} else {
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

	public void resetSpeed() {
		speed = BaseSpeed;
	}
}

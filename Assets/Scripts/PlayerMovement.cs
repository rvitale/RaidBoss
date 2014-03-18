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
	
	private float lastX = 0f;
	private float lastZ = 0f;

	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		PD_PlayerDefense = GetComponent<PlayerDefense>();
		if(networkView.isMine){
			gameObject.AddComponent<AudioListener>();
		}
	}

	void Update() {

		rigidbody.WakeUp();
		//movement and lookAt
		if (networkView.isMine ) {
			CharacterController controller = GetComponent<CharacterController> ();
			if(!PD_PlayerDefense.isDead){
				float x = Input.GetAxis ("Horizontal");
				float z = Input.GetAxis ("Vertical");
				if (Mathf.Abs(x) != 0) {
					if (Mathf.Abs(x) < Mathf.Abs(lastX)) {
						x = 0;
					} else {
						x = x / Mathf.Abs(x);
					}
				}
				if (Mathf.Abs(z) != 0) {
					if (Mathf.Abs(z) < Mathf.Abs(lastZ)) {
						z = 0;
					} else {
						z = z / Mathf.Abs(z);
					}
				}
				lastX = Input.GetAxis ("Horizontal");
				lastZ = Input.GetAxis ("Vertical");
				moveDirection = new Vector3 (x, moveDirection.y, z);

			}
			else{
				moveDirection = new Vector3 (0, moveDirection.y, 0);
			}
//			print (moveDirection);
			float y = moveDirection.y;
			moveDirection.x *= speed;
			moveDirection.z *= speed;
			moveDirection.y = 0;
			moveDirection.Normalize();
			moveDirection *= speed;
			moveDirection = new Vector3(moveDirection.x, y, moveDirection.z);
		//	print (moveDirection);
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
		}
	}

	public void ResetSpeed() {
		speed = BaseSpeed;
	}
}

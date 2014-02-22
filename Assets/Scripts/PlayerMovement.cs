using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	PlayerManager PMC_PlayerManagerClass;
	
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
		if(networkView.isMine){
			CharacterController controller = GetComponent<CharacterController>();
			moveDirection = new Vector3(Input.GetAxis("Horizontal"),moveDirection.y, Input.GetAxis("Vertical"));
			moveDirection.x *= speed;
			moveDirection.z *= speed;
			if (controller.isGrounded) {
				moveDirection.y = 0;
				if (Input.GetButton("Jump"))
					moveDirection.y = jumpSpeed;			
			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
			Vector3 faceDirection = new Vector3(moveDirection.x,0,moveDirection.z);
			if(faceDirection!= Vector3.zero)
				transform.forward = Vector3.Normalize(faceDirection);
				PMC_PlayerManagerClass.lookDirection = faceDirection;
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public static PlayerMove S;

	public float walkSpeed;
	public float jumpSpeed;
	public float jumpHoldTime;
	float jumpTimer;
	bool jumping;
	bool flipping;

	public float grav;

	float vertMag;

	public LayerMask groundLayerMask;
	public float downCastCheck;
	public float movementCastCheck;

	// Use this for initialization
	void Start () {
		if(S == null){
			S = this;
		}
		else{
			if(this != S){
				Destroy(this.gameObject);
			}
		}


		jumpTimer = 0;
		jumping = flipping = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.inOverworld){
			return;
		}

		Move ();
		VertMovement();
	}

	IEnumerator QuickFlip(Vector3 dir){
		flipping = true;
		float t = 0;

		Vector3 oldEuler = transform.rotation.eulerAngles;
		Vector3 newEuler = oldEuler;
		newEuler.y += 180;

		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;

			transform.rotation = Quaternion.Euler(Vector3.Lerp (oldEuler, newEuler, t));

			yield return 0;
		}
		transform.right = dir;
		flipping = false;
	}

	void Move(){
		Vector3 pos = transform.position;
		if(Input.GetKey(KeyCode.W)){
			pos.z += Time.deltaTime * walkSpeed;
		}
		else if(Input.GetKey(KeyCode.S)){
			pos.z -= Time.deltaTime * walkSpeed;
		}
		if(Input.GetKey(KeyCode.A)){
			pos.x -= Time.deltaTime * walkSpeed;
			if(transform.right != Vector3.left && !flipping){
				StartCoroutine("QuickFlip", Vector3.left);
			}
		}
		else if(Input.GetKey(KeyCode.D)){
			pos.x += Time.deltaTime * walkSpeed;
			if(transform.right != Vector3.right && !flipping){
				StartCoroutine("QuickFlip", Vector3.right);
			}
		}
		
		Vector3 oldPos = transform.position;

		bool canMoveCheck = false;

		BoxCollider boxCol = gameObject.GetComponent<BoxCollider>();
		Vector3 top = transform.position + Vector3.up * (boxCol.size.y / 2);
		Vector3 bottom = transform.position + Vector3.down * (boxCol.size.y / 2.5f);
		Vector3 topMid = transform.position + Vector3.up * (boxCol.size.y / 4);
		Vector3 bottomMid = transform.position + Vector3.down * (boxCol.size.y / 4);

		Vector3 distFromPoint = (pos - oldPos).normalized * movementCastCheck;

		if(Physics.Linecast(oldPos, oldPos + distFromPoint, groundLayerMask)){
			canMoveCheck = true;
		}
		if(Physics.Linecast(top, top + distFromPoint, groundLayerMask)){
			canMoveCheck = true;
		}
		if(Physics.Linecast(bottom, bottom + distFromPoint, groundLayerMask)){
			canMoveCheck = true;
		}
		if(Physics.Linecast(topMid, topMid + distFromPoint, groundLayerMask)){
			canMoveCheck = true;
		}
		if(Physics.Linecast(bottomMid, bottomMid + distFromPoint, groundLayerMask)){
			canMoveCheck = true;
		}


		if(!canMoveCheck){
			transform.position = pos;
		}


	}

	bool CheckGround(){
		Vector3 downCheck = Vector3.down * downCastCheck;

		bool middle = Physics.Linecast (transform.position, transform.position + downCheck, groundLayerMask);
		bool left = Physics.Linecast (transform.position + Vector3.right / 2.1f, transform.position + Vector3.right / 2.1f + downCheck, groundLayerMask);
		bool right = Physics.Linecast (transform.position + Vector3.left / 2.1f, transform.position + Vector3.left / 2.1f + downCheck, groundLayerMask);


		return middle || left || right;

	}

	void VertMovement(){
		bool grounded = CheckGround();

		Vector3 pos = transform.position;
		if(!grounded){
			if(jumping && Input.GetKey(KeyCode.Space)){
				vertMag = (jumpSpeed * jumpTimer) * Time.deltaTime;
				jumpTimer -= Time.deltaTime;
				if(jumpTimer <= 0){
					jumping = false;
				}
			}
			else{
				jumping = false;
			}
			vertMag -= grav * Time.deltaTime;
		}
		else{
			vertMag = 0;
			if(Input.GetKey(KeyCode.Space)){
				jumping = true;
				vertMag = (jumpSpeed * jumpTimer) * Time.deltaTime;
				jumpTimer = jumpHoldTime;
			}
		}

		pos.y += vertMag;

		transform.position = pos;
	}
}

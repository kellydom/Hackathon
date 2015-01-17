using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	public float chaseDist;
	public float chaseSpeed;

	public LayerMask groundLayerMask;
	public float movementCastCheck;

	public bool shouldStartBattle;

	public bool onGround;
	Vector3 gravVec;
	public float gravity;

	public string textToSay;

	// Use this for initialization
	void Start () {
		onGround = false;
	}

	void MoveDown(){
		if(!onGround){
			gravVec += Vector3.down * gravity * Time.deltaTime;
			Vector3 pos = transform.position;
			pos += gravVec;
			transform.position = pos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.inOverworld){
			return;
		}

		if(shouldStartBattle){
			Vector3 playerPos = PlayerMove.S.transform.position;
			Vector3 pos = transform.position;

			if(Vector3.Distance(playerPos, pos) < chaseDist){
				ChasePlayer();
			}
		}
		MoveDown();
	
	}

	IEnumerator QuickFlip(Vector3 dir){
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
	}

	void ChasePlayer(){
		Vector3 pos = transform.position;
		Vector3 playerPos = PlayerMove.S.transform.position;
		playerPos.y = pos.y;

		Vector3 moveVec = playerPos - pos;
		moveVec = moveVec.normalized * chaseSpeed * Time.deltaTime;
		pos += moveVec;
		
		Vector3 oldPos = transform.position;

		if(pos.x > oldPos.x && transform.right == Vector3.left){
			StartCoroutine("QuickFlip", Vector3.right);
		}
		if(pos.x < oldPos.x && transform.right == Vector3.right){
			StartCoroutine("QuickFlip", Vector3.left);
		}
		
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

	void OnTriggerEnter(Collider col){
		if(col.tag == "Ground"){
			onGround = true;
		}


	}
}

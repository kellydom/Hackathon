using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public GameObject targetObj;
	public float followDist;
	public float camHeight;
	public float followSpeed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.LookAt (targetObj.transform.position);
		Vector3 pos = targetObj.transform.position;
		pos.y += camHeight;
		pos.z -= followDist;

		transform.position = Vector3.Lerp (transform.position, pos, Time.deltaTime * followSpeed);

		transform.LookAt(targetObj.transform.position);
	}
}

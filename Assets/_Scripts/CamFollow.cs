using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public GameObject targetObj;
	public float followDist;
	public float camHeight;
	public float followSpeed;

	public GameObject sun;
	public float sunXOff;
	public float sunYOff;
	public float sunZOff;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		CamMove();
		SunMove();


	}

	void CamMove(){
		
		Vector3 pos = targetObj.transform.position;
		pos.y += camHeight;
		pos.z -= followDist;
		
		transform.position = Vector3.Lerp (transform.position, pos, Time.deltaTime * followSpeed);
		
		transform.LookAt(targetObj.transform.position);
	}


	void SunMove(){
		
		Vector3 sunPos = sun.transform.position;
		sunPos.x = transform.position.x + sunXOff;
		sunPos.y = transform.position.y + sunYOff;
		sunPos.z = transform.position.z + sunZOff;
		sun.transform.position = sunPos;

		sun.transform.RotateAround(sunPos, Vector3.forward, Time.deltaTime * 10);
	}
}

using UnityEngine;
using System.Collections;

public class AssistantFollow : MonoBehaviour {

	public GameObject target;

	Vector3 velocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.inOverworld) return;

		velocity = Vector3.Lerp (velocity, target.transform.position + Vector3.up * 1.5f + Vector3.right - transform.position, Random.Range (0.5f, 1.0f));

		transform.position += velocity * Time.deltaTime * 3;
	}
}

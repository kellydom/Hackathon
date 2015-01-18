using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssistantFollow : MonoBehaviour {

	public GameObject target;

	Vector3 velocity;

	public List<Sprite> animationList;
	public float animationSpeed;
	float animTimer;
	int currSprite = 0;

	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		animTimer = 0;
		sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = animationList[currSprite];
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
	
	// Update is called once per frame
	void Update () {
		if(!GameController.S.inOverworld) return;

		velocity = Vector3.Lerp (velocity, target.transform.position + Vector3.up * 1.5f + Vector3.right - transform.position, Random.Range (0.5f, 1.0f));

		Vector3 pos = transform.position + velocity * Time.deltaTime * 3;
		Vector3 oldPos = transform.position;
		
		if(pos.x > oldPos.x && transform.right == Vector3.right){
			StartCoroutine("QuickFlip", Vector3.left);
		}
		if(pos.x < oldPos.x && transform.right == Vector3.left){
			StartCoroutine("QuickFlip", Vector3.right);
		}

		transform.position = pos;

		animTimer += Time.deltaTime;
		if(animTimer > animationSpeed){
			animTimer = 0;
			currSprite++;
			if(currSprite >= animationList.Count){
				currSprite = 0;
			}
			sr.sprite = animationList[currSprite];
		}

	}
}

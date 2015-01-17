using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController S;

	public bool inOverworld;

	public Camera overWorldCam;
	public Camera battleCam;

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
		GoToOverworld();
	
	}

	public void GoToBattle(){
		inOverworld = false;
		overWorldCam.enabled = false;
		battleCam.enabled = true;
	}

	public void GoToOverworld(){
		inOverworld = true;
		overWorldCam.enabled = true;
		battleCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

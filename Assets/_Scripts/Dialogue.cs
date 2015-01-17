using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

	public enum Speaker{
		Doctor,
		Patient
	};

	Speaker speaker;
	string speech;
	List<Action> unlocks = new List<Action>();
	List<string> keyItems = new List<string>();

	public Dialogue(Speaker speak, string toSay, List<Action> ul, List<string>important){
		speaker = speak;
		speech = toSay;
		unlocks = ul;
		keyItems = important;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

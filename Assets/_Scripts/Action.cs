using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

	public enum Type{
		HISTORY,
		IMAGING,
		LAB,
		PHYSICAL
	};

	Type type;
	string name;
	bool isUnlocked;
	Dialogue dialog;

	public Action(Type t, string n, bool unlocked, Dialogue d){
		type = t;
		name = n;
		isUnlocked = unlocked;
		dialog = d;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

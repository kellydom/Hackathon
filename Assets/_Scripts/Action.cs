using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action : MonoBehaviour {

	public enum Type{
		HISTORY,
		IMAGING,
		LAB,
		PHYSICAL
	};

	public Type type;
	public string name;
	public bool isUnlocked;

	public Action(Type t, string n, bool unlocked){
		type = t;
		name = n;
		isUnlocked = unlocked;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

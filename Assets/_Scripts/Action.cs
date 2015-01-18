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
	public Dialogue question;

	public Action(Type t, string n, bool unlocked, Dialogue q){
		type = t;
		name = n;
		isUnlocked = unlocked;
		question = q;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

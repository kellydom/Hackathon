using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public enum Personality{
		Jock,
		Sweet,
		Default
	};

	Personality personality;
	Demographic dems;
	Disease disease;

	// Use this for initialization
	void Start () {
		
	}

	public void AddDisease(Disease d){
		disease = d;
		personality = Personality.Default;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

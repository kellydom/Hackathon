using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

	public enum Speaker{
		Doctor,
		Patient,
		Assistant
	};

	Speaker speaker;
	public Dictionary<Person.Personality, string> responses = new Dictionary<Person.Personality, string>();
	List<string> unlocks = new List<string>();
	List<string> keyItems = new List<string>();

	public Dialogue(Speaker speak, Dictionary<Person.Personality, string> possibleResponses, List<string> ul, List<string>important){
		speaker = speak;
		responses = possibleResponses;
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

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

	public void AddDem(Demographic dem){
		dems = dem;
	}

	public void AddDisease(Disease d){
		disease = d;
	}

	public void ChoosePersonality(){
		personality = Personality.Default;

		if(dems.sex == Demographic.Sex.Male){
			if(dems.age == Demographic.Age.Young){
				
			}
			if(dems.age == Demographic.Age.Middle){

			}
			if(dems.age == Demographic.Age.Old){
				
			}
		}
		else{

		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

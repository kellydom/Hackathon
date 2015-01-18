using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {

	public enum Personality{
		Jock,
		Sweet,
		Default
	};

	public Personality personality;
	Demographic dems;
	public Disease disease;

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

		//list of probabilities for each personality type
		List<float> probsPerPersonality = new List<float>();

		if(dems.sex == Demographic.Sex.Male){
			if(dems.age == Demographic.Age.Young){
				probsPerPersonality.Add (-1);
				probsPerPersonality.Add (0.5f);
				probsPerPersonality.Add (0.5f);
			}
			if(dems.age == Demographic.Age.Middle){
				probsPerPersonality.Add (0.5f);
				probsPerPersonality.Add (0.25f);
				probsPerPersonality.Add (0.25f);
			}
			if(dems.age == Demographic.Age.Old){
				probsPerPersonality.Add (0.15f);
				probsPerPersonality.Add (0.15f);
				probsPerPersonality.Add (0.7f);
				
			}
		}
		else{
			if(dems.age == Demographic.Age.Young){
				probsPerPersonality.Add (-1);
				probsPerPersonality.Add (0.75f);
				probsPerPersonality.Add (0.25f);
				
			}
			if(dems.age == Demographic.Age.Middle){
				probsPerPersonality.Add (0.1f);
				probsPerPersonality.Add (0.35f);
				probsPerPersonality.Add (0.55f);
				
			}
			if(dems.age == Demographic.Age.Old){
				probsPerPersonality.Add (0.001f);
				probsPerPersonality.Add (0.75f);
				probsPerPersonality.Add (0.24f);
				
			}

		}


		float ran = Random.Range(0.0f, 1.0f);
		for(int i = 0; i < probsPerPersonality.Count; ++i){
			if(ran < probsPerPersonality[i]){
				personality = (Personality)i;
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

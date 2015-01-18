using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {

	public enum SpriteEnum{
		ELDERLY_WOMAN = 0,
		JOCK = 1,
		LITTLE_KID = 2,
		GENERIC_GIRL = 3,
		PARENT = 4,
		POLICE = 5,
		ASIAN_PRINCESS = 6,
		PRINCESS = 7,
		BLACK_SKATER = 8,
		SKATER = 9,
		ELDERLY_MAN = 10,
		GENERIC_GUY = 11
	}

	public enum Personality{
		Jock,
		Sweet,
		Dandy,
		Dad,
		Polics,
		Princess,
		Sports,
		Default
	};

	public SpriteEnum spriteName;

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

		if (dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Old) {
			personality = Personality.Sweet;
			spriteName = SpriteEnum.ELDERLY_WOMAN;
		}

		else if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Middle) {
			float ran = Random.Range(0.0f, 1.0f);
			if(ran <= 0.5f){
				personality = Personality.Jock;
				spriteName = SpriteEnum.JOCK;
			}
			else{
				personality = Personality.Dad;
				spriteName = SpriteEnum.PARENT;
			}
		}

		else if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Young && dems.race == Demographic.Race.Black) {
			personality = Personality.Sports;
			spriteName = SpriteEnum.BLACK_SKATER;
		}

		else if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Young && dems.race != Demographic.Race.Black) {
			float ran = Random.Range (0.0f, 1.0f);
			if(ran <= 0.5f){
				personality = Personality.Default;
				spriteName = SpriteEnum.LITTLE_KID;
			}
			else{
				personality = Personality.Sports;
				spriteName = SpriteEnum.SKATER;
			}
		}

		else if (dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Young && dems.race == Demographic.Race.Asian) {
			personality = Personality.Princess;
			spriteName = SpriteEnum.ASIAN_PRINCESS;
		}

		else if (dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Young && dems.race != Demographic.Race.Asian) {
			personality = Personality.Princess;
			spriteName = SpriteEnum.PRINCESS;
		}

		else if(dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Middle){
			float ran = Random.Range(0.0f, 1.0f);
			if(ran <= 0.5f){
				personality = Personality.Default;
				spriteName = SpriteEnum.GENERIC_GIRL;
			}
			else {
				personality = Personality.Polics;
				spriteName = SpriteEnum.POLICE;
			}
		}

		else if(dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Old){
			personality = Personality.Dandy;
			spriteName = SpriteEnum.ELDERLY_MAN;
		}

		else{
			personality = Personality.Default;
			spriteName = SpriteEnum.GENERIC_GUY;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

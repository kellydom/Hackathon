using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {

	public enum SpriteEnum{
		SWEET_ASIAN = 0,
		SWEET_BLACK = 1,
		SWEET_HISPANIC = 2,
		SWEET_WHITE = 3,
		DANDY_ASIAN = 4,
		DANDY_BLACK = 5,
		DANDY_HISPANIC = 6,
		DANDY_WHITE = 7,
		PRINCESS_ASIAN = 8,
		PRINCESS_BLACK = 9,
		PRINCESS_HISPANIC = 10,
		PRINCESS_WHITE = 11,
		DEFAULT_KID_ASIAN = 12,
		DEFAULT_KID_BLACK = 13,
		DEFAULT_KID_HISPANIC = 14,
		DEFAULT_KID_WHITE = 15,
		DEFAULT_WOMAN_ASIAN = 16,
		DEFAULT_WOMAN_BLACK = 17,
		DEFAULT_WOMAN_HISPANIC = 18,
		DEFAULT_WOMAN_WHITE = 19,
		POLICE_ASIAN = 20,
		POLICE_BLACK = 21,
		POLICE_HISPANIC = 22,
		POLICE_WHITE = 23,
		DAD_ASIAN = 24,
		DAD_BLACK = 25,
		DAD_HISPANIC = 26,
		DAD_WHITE = 27,
		DEFAULT_MALE_ASIAN = 28,
		DEFAULT_MALE_BLACK = 29,
		DEFAULT_MALE_HISPANIC = 30,
		DEFAULT_MALE_WHITE = 31,
		JOCK_ASIAN = 32,
		JOCK_BLACK = 33,
		JOCK_HISPANIC = 34,
		JOCK_WHITE = 35,
		SPORTS_ASIAN = 36,
		SPORTS_BLACK = 37,
		SPORTS_HISPANIC = 38,
		SPORTS_WHITE = 39
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
			switch(dems.race)
			{
			case Demographic.Race.Asian:
				spriteName = SpriteEnum.SWEET_ASIAN;
				break;
			case Demographic.Race.Black:
				spriteName = SpriteEnum.SWEET_BLACK;
				break;
			case Demographic.Race.White:
				spriteName = SpriteEnum.SWEET_WHITE;
				break;
			}
		}

		if (dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Middle) {
			float roll = Random.Range(0.0f, 1.0f);
			if(roll <= 0.5f){
				personality = Personality.Default;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.DEFAULT_WOMAN_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.DEFAULT_WOMAN_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.DEFAULT_WOMAN_WHITE;
					break;
				}
			}
			else {
				personality = Personality.Polics;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.POLICE_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.POLICE_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.POLICE_WHITE;
					break;
				}
			}
		}

		if (dems.sex == Demographic.Sex.Female && dems.age == Demographic.Age.Young) {
			personality = Personality.Princess;
			switch(dems.race)
			{
			case Demographic.Race.Asian:
				spriteName = SpriteEnum.PRINCESS_ASIAN;
				break;
			case Demographic.Race.Black:
				spriteName = SpriteEnum.PRINCESS_BLACK;
				break;
			case Demographic.Race.White:
				spriteName = SpriteEnum.PRINCESS_WHITE;
				break;
			}
		}

		if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Old) {
			personality = Personality.Dandy;
			switch(dems.race)
			{
			case Demographic.Race.Asian:
				spriteName = SpriteEnum.DANDY_ASIAN;
				break;
			case Demographic.Race.Black:
				spriteName = SpriteEnum.DANDY_BLACK;
				break;
			case Demographic.Race.White:
				spriteName = SpriteEnum.DANDY_WHITE;
				break;
			}
		}

		if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Middle) {
			float roll = Random.Range(0.0f, 1.0f);
			if(roll <= 0.33f){
				personality = Personality.Default;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.DEFAULT_MALE_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.DEFAULT_MALE_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.DEFAULT_MALE_WHITE;
					break;
				}
			}
			else if (roll <= 0.66f){
				personality = Personality.Jock;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.JOCK_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.JOCK_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.JOCK_WHITE;
					break;
				}
			}
			else {
				personality = Personality.Dad;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.DAD_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.DAD_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.DAD_WHITE;
					break;
				}
			}
		}

		if (dems.sex == Demographic.Sex.Male && dems.age == Demographic.Age.Young) {
			float roll = Random.Range(0.0f, 1.0f);
			if(roll <= 0.5f){
				personality = Personality.Default;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.DEFAULT_KID_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.DEFAULT_KID_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.DEFAULT_KID_WHITE;
					break;
				}
			}
			else {
				personality = Personality.Sports;
				switch(dems.race)
				{
				case Demographic.Race.Asian:
					spriteName = SpriteEnum.SPORTS_ASIAN;
					break;
				case Demographic.Race.Black:
					spriteName = SpriteEnum.SPORTS_BLACK;
					break;
				case Demographic.Race.White:
					spriteName = SpriteEnum.SPORTS_WHITE;
					break;
				}
			}
		}


		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

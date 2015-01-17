using UnityEngine;
using System.Collections;

public class Demographic
{
	public enum Sex{
		Male,
		Female
	};
	public enum Age{
		Young,
		Middle,
		Old
	};
	public enum Race{
		White,
		Black,
		Asian
	};

	public Sex sex;

	public Age age;

	public Race race;

	public Demographic(string newSex, string newAge, string newRace)
	{
		switch(newSex){
		case "male":
			sex = Sex.Male;
			break;
		case "female":
			sex = Sex.Female;
			break;
		}

		switch(newAge){
		case "young":
			age = Age.Young;
			break;
		case "middle":
			age = Age.Middle;
			break;
		case "old":
			age = Age.Old;
			break;
		}

		switch(newRace){
		case "white":
			race = Race.White;
			break;
		case "black":
			race = Race.Black;
			break;
		case "asian":
			race = Race.Asian;
			break;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Disease
{

	public string name;

	public string complaint;

	public Demographic dems;
	
	public Disease(JSONObject obj)
	{
		double roll;
		for(int i = 0; i < obj.Count; i++)
		{
			if(obj.keys[i] != null)
			{
				switch((string)obj.keys[i])
				{
				case "name":
					name = obj.list[i].str;
					break;
				case "chief_complaint":
					complaint = obj.list[i].str;
					break;		
				case "demographics":
					JSONObject demObj = obj.list[i];
					string sex = "";
					string age = "";
					string race = "";
					for(int demographicIterator = 0; demographicIterator < demObj.Count; demographicIterator++)
					{
						switch((string)demObj.keys[demographicIterator])
						{
						case "sex":
							JSONObject sexObj = demObj.list[demographicIterator];
							double maleProbability = 0.0;
							double femaleProbability = 0.0;
							for(int sexIterator = 0; sexIterator < 2; sexIterator++)
							{
								if((string)sexObj.keys[sexIterator] == "male")
								{
									maleProbability = sexObj.list[sexIterator].f;
								}
								else
								{
									femaleProbability = sexObj.list[sexIterator].f;
								}
							}
							System.Diagnostics.Debug.Assert((maleProbability + femaleProbability) == 1.0);
							roll = Random.Range(0.0F, 1.0F);
							if(roll <= maleProbability)
							{
								sex = "male";
							}
							else
							{
								sex = "female";
							}
							break;
						case "age":
							JSONObject ageObj = demObj.list[demographicIterator];
							double youngProbability = 0;
							double middleProbability = 0;
							double oldProbability = 0;
							for(int ageIterator = 0; ageIterator < ageObj.keys.Count; ageIterator++)
							{
								switch((string)ageObj.keys[ageIterator])
								{
								case "young":
									youngProbability = ageObj.list[ageIterator].f;
									break;
								case "middle":
									middleProbability = ageObj.list[ageIterator].f;
									break;
								case "old":
									oldProbability = ageObj.list[ageIterator].f;
									break;
								}
							}
							System.Diagnostics.Debug.Assert((youngProbability + middleProbability + oldProbability) == 1.0);
							roll = Random.Range(0.0F, 1.0F);
							if(roll <= youngProbability)
							{
								age = "young";
							}
							else if(roll <= (youngProbability + middleProbability))
							{
								age = "middle";
							}
							else
							{
								age = "old";
							}
							break;
						case "race":
							JSONObject raceObj = demObj.list[demographicIterator];
							double blackProbability = 0;
							double whiteProbability = 0;
							double asianProbability = 0;
							double hispanicProbability = 0;
							for(int raceIterator = 0; raceIterator < raceObj.keys.Count; raceIterator++)
							{
								switch((string)raceObj.keys[raceIterator])
								{
								case "black":
									blackProbability = raceObj.list[raceIterator].f;
									break;
								case "white":
									whiteProbability = raceObj.list[raceIterator].f;
									break;
								case "asian":
									asianProbability = raceObj.list[raceIterator].f;
									break;
								case "hispanic":
									hispanicProbability = raceObj.list[raceIterator].f;
									break;
								}
							}
							System.Diagnostics.Debug.Assert((blackProbability + whiteProbability + asianProbability + hispanicProbability) == 1.0);
							roll = Random.Range(0.0F, 1.0F);
							if(roll <= blackProbability)
							{
								race = "black";
							}
							else if(roll <= (blackProbability + whiteProbability))
							{
								race = "white";
							}
							else if(roll <= (blackProbability + whiteProbability + asianProbability))
							{
								race = "asian";
							}
							else
							{
								race = "hispanic";
							}
							break;
						}
					}
					dems = new Demographic(sex, age, race);					break;
				}
			}
		}
	}


	// Use this for initialization
	void Start ()
	{


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class Disease : MonoBehaviour
{

	public string name;

	public string complaint;

	
	public Dictionary<string, float> sexProbs = new Dictionary<string, float>();
	public Dictionary<string, float> ageProbs = new Dictionary<string, float>();
	public Dictionary<string, float> raceProbs = new Dictionary<string, float>();
	public Dictionary<string, Dialogue> responseDictionary = new Dictionary<string, Dialogue>();
	public List<string> successfullTreatments = new List<string> ();

	
	public Disease(JSONObject obj)
	{



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
							for(int sexIterator = 0; sexIterator < sexObj.Count; sexIterator++)
							{
								sexProbs.Add ((string)sexObj.keys[sexIterator], sexObj.list[sexIterator].f);
							}

							break;
						case "age":
							JSONObject ageObj = demObj.list[demographicIterator];
							double youngProbability = 0;
							double middleProbability = 0;
							double oldProbability = 0;
							for(int ageIterator = 0; ageIterator < ageObj.keys.Count; ageIterator++)
							{
								ageProbs.Add ((string)ageObj.keys[ageIterator], ageObj.list[ageIterator].f);
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
								raceProbs.Add ((string)raceObj.keys[raceIterator], raceObj.list[raceIterator].f);
							}
							break;
						}
					}
					break;
				case "history":
					JSONObject historyObj = obj.list[i];
					for(int historyIterator = 0; historyIterator < historyObj.keys.Count; historyIterator++)
					{
						string actionName = (string)historyObj.keys[historyIterator];
						Dictionary<Person.Personality, string> personalityDictionary = new Dictionary<Person.Personality, string>();
						List<string> unlockList = new List<string>();
						List<string> importantList = new List<string>();
						JSONObject actionObj = historyObj.list[historyIterator];
						for(int actionIterator = 0; actionIterator < actionObj.keys.Count; actionIterator++)
						{
							switch((string)actionObj.keys[actionIterator])
							{
							case "speech":
								JSONObject speechObj = actionObj.list[actionIterator];
								for(int speechIterator = 0; speechIterator < speechObj.keys.Count; speechIterator++)
								{
									Person.Personality speechKey = Person.Personality.Default;
									switch((string)speechObj.keys[speechIterator])
									{
									case "jock":
										speechKey = Person.Personality.Jock;
										break;
									case "sweet":
										speechKey = Person.Personality.Sweet;
										break;
									case "dandy":
										speechKey = Person.Personality.Dandy;
										break;
									case "dad":
										speechKey = Person.Personality.Dad;
										break;
									case "police":
										speechKey = Person.Personality.Polics;
										break;
									case "princess":
										speechKey = Person.Personality.Princess;
										break;
									case "sports":
										speechKey = Person.Personality.Sports;
										break;
									case "default":
										speechKey = Person.Personality.Default;
										break;
									}
									personalityDictionary.Add (speechKey, speechObj.list[speechIterator].str);
								}
								break;
							}
						}
						Dialogue responseDialogue = new Dialogue(Dialogue.Speaker.Patient, personalityDictionary, null, null);
						responseDictionary.Add(actionName, responseDialogue);
					}
					break;
				case "physical":
					JSONObject physicalObj = obj.list[i];
					for(int physicalIterator = 0; physicalIterator < physicalObj.keys.Count; physicalIterator++)
					{
						string actionName = (string)physicalObj.keys[physicalIterator];
						Dictionary<Person.Personality, string> personalityDictionary = new Dictionary<Person.Personality, string>();
						List<string> unlockList = new List<string>();
						List<string> importantList = new List<string>();
						personalityDictionary.Add(Person.Personality.Default, physicalObj.list[physicalIterator].str);
						Dialogue responseDialogue = new Dialogue(Dialogue.Speaker.Assistant, personalityDictionary, null, null);
						responseDictionary.Add(actionName, responseDialogue);		                             
					}
					break;
				case "labs":
					JSONObject labObj = obj.list[i];
					for(int labIterator = 0; labIterator < labObj.keys.Count; labIterator++)
					{
						string actionName = (string)labObj.keys[labIterator];
						Dictionary<Person.Personality, string> personalityDictionary = new Dictionary<Person.Personality, string>();
						List<string> unlockList = new List<string>();
						List<string> importantList = new List<string>();
						personalityDictionary.Add(Person.Personality.Default, labObj.list[labIterator].str);
						Dialogue responseDialogue = new Dialogue(Dialogue.Speaker.Assistant, personalityDictionary, unlockList, importantList);
						responseDictionary.Add(actionName, responseDialogue);							                             
					}
					break;
				case "imaging":
					JSONObject imageObj = obj.list[i];
					for(int imageIterator = 0; imageIterator < imageObj.keys.Count; imageIterator++)
					{
						string actionName = (string)imageObj.keys[imageIterator];
						Dictionary<Person.Personality, string> personalityDictionary = new Dictionary<Person.Personality, string>();
						List<string> unlockList = new List<string>();
						List<string> importantList = new List<string>();
						personalityDictionary.Add(Person.Personality.Default, imageObj.list[imageIterator].str);
						Dialogue responseDialogue = new Dialogue(Dialogue.Speaker.Assistant, personalityDictionary, unlockList, importantList);
						responseDictionary.Add(actionName, responseDialogue);	                             
					}
					break;
				case "treatment_response_success":
					JSONObject successObj = obj.list[i];
					string actionName = (string)obj.keys[i];
					Dictionary<Person.Personality, string> successResponseDictionary = new Dictionary<Person.Personality, string>();
					for(int successIterator = 0; successIterator < successObj.keys.Count; successIterator++)
					{
						Person.Personality speechKey = Person.Personality.Default;
						switch((string)successObj.keys[successIterator])
						{
						case "jock":
							speechKey = Person.Personality.Jock;
							break;
						case "sweet":
							speechKey = Person.Personality.Sweet;
							break;
						case "dandy":
							speechKey = Person.Personality.Dandy;
							break;
						case "dad":
							speechKey = Person.Personality.Dad;
							break;
						case "police":
							speechKey = Person.Personality.Polics;
							break;
						case "princess":
							speechKey = Person.Personality.Princess;
							break;
						case "sports":
							speechKey = Person.Personality.Sports;
							break;
						case "default":
							speechKey = Person.Personality.Default;
							break;
						}
						successResponseDictionary.Add(speechKey, successObj.list[successIterator].str);
					}
					Dialogue responseDialogue = new Dialogue(Dialogue.Speaker.Patient, successResponseDictionary, null, null);
					responseDictionary.Add (actionName, responseDialogue);
					break;
				case "treatment_response_failure":
					JSONObject failureObj = obj.list[i];
					string failedActionName = (string)obj.keys[i];
					Dictionary<Person.Personality, string> failedResponseDictionary = new Dictionary<Person.Personality, string>();
					for(int failureIterator = 0; failureIterator < failureObj.keys.Count; failureIterator++)
					{
						Person.Personality speechKey = Person.Personality.Default;
						switch((string)failureObj.keys[failureIterator])
						{
						case "jock":
							speechKey = Person.Personality.Jock;
							break;
						case "sweet":
							speechKey = Person.Personality.Sweet;
							break;
						case "dandy":
							speechKey = Person.Personality.Dandy;
							break;
						case "dad":
							speechKey = Person.Personality.Dad;
							break;
						case "police":
							speechKey = Person.Personality.Polics;
							break;
						case "princess":
							speechKey = Person.Personality.Princess;
							break;
						case "sports":
							speechKey = Person.Personality.Sports;
							break;
						case "default":
							speechKey = Person.Personality.Default;
							break;
						}
						failedResponseDictionary.Add(speechKey, failureObj.list[failureIterator].str);
					}
					Dialogue failedResponseDialogue = new Dialogue(Dialogue.Speaker.Patient, failedResponseDictionary, null, null);
					responseDictionary.Add (failedActionName, failedResponseDialogue);
					break;
				case "treatment":
					JSONObject treatmentObj = obj.list[i];
					for(int treatmentIterator = 0; treatmentIterator < treatmentObj.list.Count; treatmentIterator++)
					{
						successfullTreatments.Add(treatmentObj.list[treatmentIterator].str);
					}
					break;
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
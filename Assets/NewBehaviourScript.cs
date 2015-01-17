using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		string longString = "{\"name\": \"Gastroesophageal Reflux Disease\",\"chief_complaint\": \"Chest pain\",\"demographics\": {\"sex\": {\"male\": 0.5,\"female\": 0.5},\"age\": {\"young\": 0.1,\"middle\": 0.4,\"old\": 0.5},\"race\": {\"black\": 0.3,\"white\": 0.3,\"asian\": 0.4}}}";

		JSONObject emperorJSonOfSpartax = new JSONObject(longString);
		Disease theAIDS = new Disease (emperorJSonOfSpartax);

		print (theAIDS.name);
		print (theAIDS.complaint);
		print (theAIDS.dems.sex);
		print (theAIDS.dems.age);
		print (theAIDS.dems.race);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

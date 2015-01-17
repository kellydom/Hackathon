using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameController : MonoBehaviour {
	public static GameController S;

	public bool inOverworld;

	public Camera overWorldCam;
	public Camera battleCam;

	public GameObject spawnObject;

	public GameObject tempSpawnPrefab;

	List<Disease> diseases = new List<Disease>();

	public string path;


	// Use this for initialization
	void Start () {
		if(S == null){
			S = this;
		}
		else{
			if(this != S){
				Destroy(this.gameObject);
			}
		}
		GoToOverworld();
		CreateDiseases();
	}

	void CreateDiseases(){
		var info = new DirectoryInfo(path);
		var fileInfo = info.GetFiles("*.txt");
		foreach(var file in fileInfo){
			string jsonO = File.ReadAllText(path + "SampleJSON.txt");
			JSONObject emperorJSonOfSpartax = new JSONObject(jsonO);

			Disease newDisease = new Disease(emperorJSonOfSpartax);
			
			diseases.Add (newDisease);
		}
	}

	Demographic MakeDems(Disease thisDisease){

		Demographic newDem;

		float maleProb = thisDisease.sexProbs["male"];
		float roll = Random.Range (0.0f, 1.0f);
		string sex = "";
		if(roll <= maleProb){
			sex = "male";
		}
		else{
			sex = "female";
		}
		
		float youngProb = thisDisease.ageProbs["young"];
		float middleProb = thisDisease.ageProbs["middle"];
		roll = Random.Range (0.0f, 1.0f);
		string age = "";
		if(roll <= youngProb){
			age = "young";
		}
		else if(roll <= youngProb + middleProb){
			age = "middle";
		}
		else{
			age = "old";
		}
		
		float whiteProb = thisDisease.raceProbs["white"];
		float asianProb = thisDisease.raceProbs["asian"];
		roll = Random.Range (0.0f, 1.0f);
		string race = "";
		if(roll <= whiteProb){
			race = "white";
		}
		else if(roll <= whiteProb + asianProb){
			race = "asian";
		}
		else{
			race = "black";
		}

		newDem = new Demographic(sex, age, race);

		return newDem;

	}

	void SpawnEnemy(Vector3 pos){
		GameObject newEn = Instantiate(tempSpawnPrefab, pos, Quaternion.identity) as GameObject;
		AI ai = newEn.GetComponent<AI>();
		Person person = newEn.AddComponent<Person>();

		int ran = Random.Range(0, diseases.Count - 1);
		Disease thisDisease = diseases[ran];

		Demographic newDem = MakeDems();

		person.AddDem(newDem);
		person.AddDisease(thisDisease);
		person.ChoosePersonality();
	}

	void Awake(){
		for(int i = 0; i < spawnObject.transform.childCount; ++i){
			SpawnEnemy(spawnObject.transform.GetChild (i).transform.position);
		}
	}

	public void GoToBattle(){
		inOverworld = false;
		overWorldCam.enabled = false;
		battleCam.enabled = true;
	}

	public void GoToOverworld(){
		inOverworld = true;
		overWorldCam.enabled = true;
		battleCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

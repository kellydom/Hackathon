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
	public List<Action> actions = new List<Action>();
	public List<Treatment> treatments = new List<Treatment>();

	public string path;

	public GameObject blockingPlane;

	public List<Sprite> possibleSprites = new List<Sprite>();


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
		blockingPlane.renderer.enabled = false;
		CreateActionList();
		CreateDiseases();

		for(int i = 0; i < spawnObject.transform.childCount; ++i){
			SpawnEnemy(spawnObject.transform.GetChild (i).transform.position);
		}

		
		GoToOverworld();
	}

	void CreateActionList(){
		string actionList = File.ReadAllText("Assets/ActionJSON.txt");
		JSONObject actionJSON = new JSONObject(actionList);

		for(int i = 0; i < actionJSON.Count; i++)
		{
			if(actionJSON.keys[i] != null)
			{
				Action.Type type;
				JSONObject typeObj;
				switch((string)actionJSON.keys[i])
				{
				case "history":
					type = Action.Type.HISTORY;
					typeObj = actionJSON.list[i];
					for(int typeIterator = 0; typeIterator < typeObj.Count; typeIterator++)
					{	
						JSONObject questionObj = typeObj.list[typeIterator];
						string name = typeObj.keys[typeIterator];
						print (name);
						bool isUnlocked = false;
						Dialogue question = null;
						Dictionary<Person.Personality, string> docsPersonality = new Dictionary<Person.Personality, string>();
						for(int questionIterator = 0; questionIterator < questionObj.keys.Count; questionIterator++)
						{
							switch((string)questionObj.keys[questionIterator])
							{
							case "speech":
								string query = questionObj.list[questionIterator].str;
								print (query);
								docsPersonality.Add(Person.Personality.Default, query);
								question = new Dialogue(Dialogue.Speaker.Doctor, docsPersonality, null, null);
								break;
							case "visibility":
								isUnlocked = questionObj.list[questionIterator].b;
								break;
							}
						}
						Action newAction = new Action(type, name, isUnlocked, question);
						actions.Add (newAction);
					}
					break;
				
				case "physical":
					type = Action.Type.PHYSICAL;
					typeObj = actionJSON.list[i];
					for(int typeIterator = 0; typeIterator < typeObj.Count; typeIterator++)
					{
						string name = typeObj.keys[typeIterator];
						bool isUnlocked = typeObj.list[typeIterator].b;
						
						Action newAction = new Action(type, name, isUnlocked, null);
						actions.Add (newAction);
						
					}
					break;
				
				case "labs":
					type = Action.Type.LAB;
					typeObj = actionJSON.list[i];
					for(int typeIterator = 0; typeIterator < typeObj.Count; typeIterator++)
					{
						string name = typeObj.keys[typeIterator];
						bool isUnlocked = typeObj.list[typeIterator].b;
						
						Action newAction = new Action(type, name, isUnlocked, null);
						actions.Add (newAction);
						
					}
					break;
				
				case "imaging":
					type = Action.Type.IMAGING;
					typeObj = actionJSON.list[i];
					for(int typeIterator = 0; typeIterator < typeObj.Count; typeIterator++)
					{
						string name = typeObj.keys[typeIterator];
						bool isUnlocked = typeObj.list[typeIterator].b;
						
						Action newAction = new Action(type, name, isUnlocked, null);
						actions.Add (newAction);
						
					}
					break;
				
				case "treatment":
					break;
				}
				
			}
		}
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

		Demographic newDem = MakeDems(thisDisease);

		person.AddDem(newDem);
		person.AddDisease(thisDisease);
		person.ChoosePersonality();
		
		Sprite sprite = possibleSprites[(int)person.spriteName];

		newEn.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

	}

	IEnumerator FadeToBattle(GameObject patient){
		blockingPlane.renderer.enabled = true;

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 1.0f;

			Color c = Color.black;
			c.a = Mathf.Lerp (0, 1, t);

			blockingPlane.renderer.material.color = c;

			yield return 0;
		}
		overWorldCam.enabled = false;
		battleCam.enabled = true;
		BattleController.S.BattleStart(patient);

		blockingPlane.transform.position = battleCam.transform.position + battleCam.transform.forward / 2;
		blockingPlane.transform.parent = battleCam.transform;

		Vector3 localEuler = blockingPlane.transform.rotation.eulerAngles;
		localEuler.y += 180;
		blockingPlane.transform.rotation = Quaternion.Euler (localEuler);

		t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 1.0f;
			
			Color c = Color.black;
			c.a = Mathf.Lerp (1, 0, t);
			
			blockingPlane.renderer.material.color = c;
			
			yield return 0;
		}
		
		blockingPlane.transform.position = overWorldCam.transform.position + overWorldCam.transform.forward / 2;
		blockingPlane.transform.parent = overWorldCam.transform;
		
		localEuler = blockingPlane.transform.rotation.eulerAngles;
		localEuler.y += 180;
		blockingPlane.transform.rotation = Quaternion.Euler (localEuler);
	}

	public void GoToBattle(GameObject patient){
		inOverworld = false;
		StartCoroutine(FadeToBattle(patient));
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

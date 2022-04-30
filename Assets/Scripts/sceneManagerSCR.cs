using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class sceneManagerSCR : MonoBehaviour{
	public float screenHeight, screenWidth;
	
	// List of existing obstacles on screen
	public List<GameObject> obstacleList = new List<GameObject>();
	public List<GameObject> knifeList = new List<GameObject>();
	
	// Game state management
	public bool isOnMainScreen = true;
	public bool gameHasStarted, awaitingRestart, awaitingContinue;
	public bool areYouSure, resetGameScreen, knifeShopScreen;
	
	// Score and coin counters
	public int level = 1;
	public int levelScore, coins;
	
	// Player and environment management
	public GameObject Knife;
	public Quaternion originalKnifeRotation;
	public knifeSCR knifeScript;
	
	GameObject knifeStand;
	GameObject gamePlane;
	GameObject finishLine;
	
	void Start(){
		Knife = GameObject.Find("Knife");
		knifeScript = Knife.GetComponent<knifeSCR>();
		originalKnifeRotation = Knife.transform.rotation;
		
		LoadGame();
		
		StartLevel();
	}
	
	void OnGUI(){
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		
		GUI.skin.button.fontSize = (int)screenHeight/25;
		GUI.skin.label.fontSize = (int)screenHeight/25;
		
		// OPTIONS BUTTON
		if(GUI.Button(new Rect(screenWidth/30, screenWidth/30, screenWidth/10, screenWidth/10), "...")){
			isOnMainScreen = !isOnMainScreen;
			areYouSure = false;
			resetGameScreen = false;
			knifeShopScreen = false;
		}
		
		// GAME SCREEN
		if(isOnMainScreen){
			// Player touched the floor (reset level)
			if(awaitingRestart){
				awaitingContinue = false;
				// Reset button
				if(GUI.Button(new Rect(screenWidth*1/5, screenHeight*2/5, screenWidth*3/5, screenHeight*1/5), "RESTART")){
					awaitingRestart = false;
					gameHasStarted = false;
					
					RestartLevel();
				}
			}
			
			// Player touched the finish line (end of level)
			if(awaitingContinue){
				awaitingRestart = false;
				// Next level button
				if(GUI.Button(new Rect(screenWidth*1/5, screenHeight*2/5, screenWidth*3/5, screenHeight*1/5), "NEXT LEVEL")){
					awaitingContinue = false;
					gameHasStarted = false;
					
					ChangeLevel();
				}
			}
			
			// Player hasn't started level
			if(!awaitingContinue && !awaitingRestart && !gameHasStarted){
				GUI.color = new Color(0,0,0,1);
				
				GUI.Label(new Rect(screenWidth*3/7, screenHeight/30, screenWidth*3/7, screenHeight/15), $"Level {level}");
				GUI.Label(new Rect(screenWidth*6/7, screenHeight/40, screenWidth*2/7, screenHeight/20), $"${coins}");
				GUI.Label(new Rect(screenWidth*2/7, screenHeight*3/5, screenWidth*5/7, screenHeight/5), $"TAP TO PLAY");
				
				GUI.color = new Color(1,1,1,0f);
				
				if(GUI.Button(new Rect(screenWidth*1/15, screenHeight*1/25, screenWidth*13/15, screenHeight*23/25), " ")){
					gameHasStarted = true;
					knifeScript.canJump = true;
				}
			}
		}
		// MENU BOX
		else{
			GUI.skin.box.fontSize = (int)screenHeight/25;
			GUI.skin.button.fontSize = (int)screenHeight/45;
			
			GUI.Box(new Rect(screenWidth/15, screenHeight/25, screenWidth*13/15, screenHeight*23/25), "MENU");
			
			// RESET DATA button
			if(!areYouSure && !knifeShopScreen){
				if(GUI.Button(new Rect(screenWidth/5, screenHeight/7, screenWidth*3/5, screenHeight/7), "RESET ALL GAME DATA")){
					areYouSure = true;
				}
			}
			else if(!knifeShopScreen){
				if(GUI.Button(new Rect(screenWidth/5, screenHeight/5, screenWidth/5, screenHeight/5), "RESET")){
					ResetGame();
				}
				if(GUI.Button(new Rect(screenWidth*3/5, screenHeight/5, screenWidth/5, screenHeight/5), "RETURN")){
					areYouSure = false;
				}
			}
			
			// Knife SHOP button
			if(!knifeShopScreen && !areYouSure){
				if(GUI.Button(new Rect(screenWidth/5, screenHeight*3/7, screenWidth*3/5, screenHeight/7), "SHOP")){
					knifeShopScreen = true;
				}
			}
			// Knife SHOP options
			else if(!areYouSure){
				// KNIFE 1
				if(!knifeList[0].GetComponent<knifeSCR>().canEquip){
					if(GUI.Button(new Rect(screenWidth/5, screenHeight/7, screenWidth/5, screenHeight/7), knifeList[0].GetComponent<knifeSCR>().buyingPrice.ToString())){
						knifeList[0].GetComponent<knifeSCR>().BuyKnife();
					}
				}
				else{
					if(GUI.Button(new Rect(screenWidth/5, screenHeight/7, screenWidth/5, screenHeight/7), "EQUIP 1")){
						knifeList[0].GetComponent<knifeSCR>().EquipKnife(0);
					}
				}
				
				// KNIFE 2
				if(!knifeList[1].GetComponent<knifeSCR>().canEquip){
					if(GUI.Button(new Rect(screenWidth*3/5, screenHeight/7, screenWidth/5, screenHeight/7), knifeList[1].GetComponent<knifeSCR>().buyingPrice.ToString())){
						knifeList[1].GetComponent<knifeSCR>().BuyKnife();
					}
				}
				else{
					if(GUI.Button(new Rect(screenWidth*3/5, screenHeight/7, screenWidth/5, screenHeight/7), "EQUIP 2")){
						knifeList[1].GetComponent<knifeSCR>().EquipKnife(1);
					}
				}
				
				// KNIFE 3
				if(!knifeList[2].GetComponent<knifeSCR>().canEquip){
					if(GUI.Button(new Rect(screenWidth/5, screenHeight*3/7, screenWidth/5, screenHeight/7), knifeList[2].GetComponent<knifeSCR>().buyingPrice.ToString())){
						knifeList[2].GetComponent<knifeSCR>().BuyKnife();
					}
				}
				else{
					if(GUI.Button(new Rect(screenWidth/5, screenHeight*3/7, screenWidth/5, screenHeight/7), "EQUIP 3")){
						knifeList[2].GetComponent<knifeSCR>().EquipKnife(2);
					}
				}
				
				// KNIFE 4
				if(!knifeList[3].GetComponent<knifeSCR>().canEquip){
					if(GUI.Button(new Rect(screenWidth*3/5, screenHeight*3/7, screenWidth/5, screenHeight/7), knifeList[3].GetComponent<knifeSCR>().buyingPrice.ToString())){
						knifeList[3].GetComponent<knifeSCR>().BuyKnife();
					}
				}
				else{
					if(GUI.Button(new Rect(screenWidth*3/5, screenHeight*3/7, screenWidth/5, screenHeight/7), "EQUIP 4")){
						knifeList[3].GetComponent<knifeSCR>().EquipKnife(3);
					}
				}
				
				// RETURN
				if(GUI.Button(new Rect(screenWidth/5, screenHeight*5/7, screenWidth*3/5, screenHeight/7), "RETURN")){
					knifeShopScreen = false;
				}
				
				// I'm sure there's a smarter way to do this
			}
		
			// QUIT button
			if(!knifeShopScreen && !areYouSure){
				if(GUI.Button(new Rect(screenWidth*1/5, screenHeight*5/7, screenWidth*3/5, screenHeight*1/7), "QUIT GAME")){
					Application.Quit();
				}
			}
		}
	}
	
	
	// SET GAME STATES
	void StartScreen(){
		awaitingRestart = false;
	}
	
	public void RestartScreen(){
		awaitingRestart = true;
	}
	
	public void CongratsScreen(){
		coins += (int)levelScore / 10;
		levelScore = 0;
		
		SaveGame();
		
		awaitingContinue = true;
	}
	
	
	// BEGIN-LEVEL FUNCTIONS
	void StartLevel(){
		OrganizeField();
		RestartKnife();
		OrganizeObstacles();
		
		StartScreen();
	}
	
	void ChangeLevel(){
		level++;
		
		try{
			OrganizeField();
			RestartKnife();
			OrganizeObstacles();
			
			StartScreen();
		}
		catch{
			level = 1;
			
			OrganizeField();
			RestartKnife();
			OrganizeObstacles();
			
			StartScreen();
		}
	}
	
	void OrganizeField(){
		foreach(Transform child in GameObject.Find("LevelSpace").transform){
			Destroy(child.gameObject);
		}
		
		string path = $"Level{level}/knifeStand";
		Object knifeStandPrefab = Resources.Load(path);
		GameObject knifeStandPrefabGO = knifeStandPrefab as GameObject;
		knifeStand = Instantiate(knifeStandPrefab, knifeStandPrefabGO.transform.position, knifeStandPrefabGO.transform.rotation)as GameObject;
		knifeStand.transform.parent = GameObject.Find("LevelSpace").transform;
		
		path = $"Level{level}/gamePlane";
		Object gamePlanePrefab = Resources.Load(path);
		GameObject gamePlanePrefabGO = gamePlanePrefab as GameObject;
		gamePlane = Instantiate(gamePlanePrefab, gamePlanePrefabGO.transform.position, gamePlanePrefabGO.transform.rotation)as GameObject;
		gamePlane.transform.parent = GameObject.Find("LevelSpace").transform;
		
		path = $"Level{level}/finishLine";
		Object finishLinePrefab = Resources.Load(path);
		GameObject finishLinePrefabGO = finishLinePrefab as GameObject;
		finishLine = Instantiate(finishLinePrefab, finishLinePrefabGO.transform.position, finishLinePrefabGO.transform.rotation)as GameObject;
		finishLine.transform.parent = GameObject.Find("LevelSpace").transform;
	}
	
	void RestartKnife(){
		Knife.transform.position = knifeStand.transform.position + new Vector3(0f, 1.5f, 0f);
		Knife.transform.rotation = originalKnifeRotation;
		
		knifeScript.RestartKnifeConstraints();
	}

	void OrganizeObstacles(){
		for(int j=0; j<obstacleList.Count; j++){
			Destroy(obstacleList[j]);
		}
		obstacleList.Clear();
		
		for(int i=1; ; i++){
			try{
				string path = $"Level{level}/Obstacles/Obstacle{i}";
				Object obstaclePrefab = Resources.Load(path);
				GameObject obstaclePrefabGO = obstaclePrefab as GameObject;
				GameObject obstacle = Instantiate(obstaclePrefab, obstaclePrefabGO.transform.position, obstaclePrefabGO.transform.rotation)as GameObject;
				obstacle.transform.parent = GameObject.Find("Obstacles").transform;
				
				obstacleList.Add(obstacle);
			}
			catch{
				break;
			}
		}
	}
	
	void RestartLevel(){
		levelScore = 0;
		
		RestartKnife();
		OrganizeObstacles();
		
		StartScreen();
	}


	// SAVED DATA MANAGEMENT
	void SaveGame(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
		
		SaveData data = new SaveData();
		data.levelNumber = level;
		data.coinNumber = coins;
		
		for(int i=0; i<knifeList.Count; i++){
			data.knivesOwned[i] = knifeList[i].GetComponent<knifeSCR>().canEquip;
		}
		
		bf.Serialize(file, data);
		file.Close();
	}
	
	void LoadGame(){
		if(File.Exists(Application.persistentDataPath + "/MySaveData.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
			
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			
			level = data.levelNumber;
			coins = data.coinNumber;
			
			for(int i=0; i<data.knivesOwned.Length; i++){
				knifeList[i].GetComponent<knifeSCR>().canEquip = data.knivesOwned[i];
			}
		}
		else{
			level = 1;
			coins = 0;
			
		}
	}

	void ResetGame(){
		if(File.Exists(Application.persistentDataPath + "/MySaveData.dat")){
			File.Delete(Application.persistentDataPath + "/MySaveData.dat");
			Application.Quit();
		}
	}
}

[System.Serializable]
class SaveData{
	public int levelNumber;
	public int coinNumber;
	public bool[] knivesOwned = new bool[4];
}
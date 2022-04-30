using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasSCR : MonoBehaviour{
	GameObject startGameTxt, levelTxt, coinTxt;
	int fontsize;
	
	void Start(){
		startGameTxt = GameObject.Find("EventSystem/Canvas/StartDisplay");
		levelTxt = GameObject.Find("EventSystem/Canvas/LevelDisplay");
		coinTxt = GameObject.Find("EventSystem/Canvas/CoinDisplay");
		
		fontsize = (int)Screen.height/20;
		
		startGameTxt.GetComponent<Text>().fontSize = fontsize;
		levelTxt.GetComponent<Text>().fontSize = fontsize;
		coinTxt.GetComponent<Text>().fontSize = fontsize;
		
		startGameTxt.transform.position = new Vector3(Screen.height*3/5, Screen.width/2, 0f);
		levelTxt.transform.position = new Vector3(Screen.height/10, Screen.width/2, 0f);
		coinTxt.transform.position = new Vector3(Screen.height/10, Screen.width*2/3, 0f);
	}
}

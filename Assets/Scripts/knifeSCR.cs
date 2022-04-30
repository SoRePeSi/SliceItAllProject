using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class knifeSCR : MonoBehaviour{
	Rigidbody m_Rigidbody;
	Transform m_Transform;
	
	sceneManagerSCR sceneManagerScript;
	
	float screenHeight, screenWidth;
	
	public float jumpForce;
	public bool canJump = false, canRotate = false, canEquip;
	public int buyingPrice;
	
	void Start(){
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Transform = GetComponent<Transform>();
		
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		
		sceneManagerScript = GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>();
	}


	// JUMP FUNCTION
	void OnGUI(){
		if(canJump){
			GUI.color = new Color(1,1,1,0f);
			
			if(GUI.Button(new Rect(screenWidth*1/15, screenHeight*2/25, screenWidth*13/15, screenHeight*21/25), " ")){
				m_Rigidbody.velocity = new Vector3(0f,0f,0f);
				m_Rigidbody.AddForce(new Vector3(jumpForce, jumpForce, 0f), ForceMode.Impulse);
			}
		}
	}
	
	void OnCollisionEnter(Collision other){
		string otherTag = other.gameObject.tag;
		
		// Restart level on collision with floor
		if(otherTag == "Floor"){
			canJump = false;
			canRotate = false;
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			
			sceneManagerScript.RestartScreen();
		}
		// Change level on collision with finish line
		else if(otherTag == "FinishLine"){
			canJump = false;
			canRotate = false;
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			
			sceneManagerScript.CongratsScreen();
		}
		// Get pushed back on collision with anything else
		else{
			m_Rigidbody.AddForce(new Vector3(-jumpForce, -jumpForce, 0f), ForceMode.Impulse);
		}
	}

	public void RestartKnifeConstraints(){
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
	}


	public void BuyKnife(){
		if(sceneManagerScript.coins >= buyingPrice){
			sceneManagerScript.coins -= buyingPrice;
			canEquip = true;
		}
	}
	
	// Deactivate every knife that's not the chosen one and activate chosen one
	public void EquipKnife(int knifeNum){
		for(int i=0; i<4; i++){
			string knifeTag = $"Knife{knifeNum}";
			GameObject chosenKnife = GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>().knifeList[i];
			
			if(chosenKnife.tag == gameObject.tag){
				Debug.Log(i+1);
				chosenKnife.SetActive(true);
				GameObject.Find("Main Camera").GetComponent<cameraSCR>().knife = chosenKnife.transform;
				GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>().Knife = gameObject;
				GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>().originalKnifeRotation = transform.rotation;
				GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>().knifeScript = GetComponent<knifeSCR>();
			}
			else{
				chosenKnife.SetActive(false);
			}
		}
	}
}
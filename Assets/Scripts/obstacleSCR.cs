using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSCR : MonoBehaviour{
	Rigidbody m_Rigidbody;
	Transform Knife;
	sceneManagerSCR sceneManagerScript;
	
	public int scoreWorth;
	
	void Start(){
		m_Rigidbody = GetComponent<Rigidbody>();
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		
		Knife = GameObject.Find("Knife").transform;
		Physics.IgnoreCollision(GetComponent<Collider>(), Knife.GetComponent<Collider>());
		
		sceneManagerScript = GameObject.Find("SceneManager").GetComponent<sceneManagerSCR>();
		
		// Set obstacle value according to tag
		switch(transform.tag){
			case "Cookie":
				scoreWorth = 50;
				break;
			
			case "Slice":
				scoreWorth = 25;
				break;
			
			case "Onigiri":
				scoreWorth = 20;
				break;
			
			case "Apple":
				scoreWorth = 15;
				break;
				
			case "Bottle":
				scoreWorth = 10;
				break;
			
			case "Ham":
				scoreWorth = 5;
				break;
				
			default:
				scoreWorth = 5;
				break;
		}
	}
	
	// Stops "floating" on collision
	void OnCollisionEnter(Collision other){
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}
	
	public void SliceObstacle(){
		if(sceneManagerScript.obstacleList.Exists(x => x.transform == transform))
			sceneManagerScript.obstacleList.Remove(gameObject);
			sceneManagerScript.levelScore += scoreWorth;
		
		Destroy(gameObject);
	}
}

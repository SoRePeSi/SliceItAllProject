using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bladeSCR : MonoBehaviour{
	List<string> obstacleTagList;
	
	void Start(){
		obstacleTagList = new List<string> {"Apple", "Bottle", "Cookie", "Ham", "Steack", "Slice", "Onigiri"};
	}
	
	void OnTriggerEnter(Collider other){
		string otherTag = other.tag;
		
		if(obstacleTagList.Exists(x => x == otherTag)){
			other.gameObject.GetComponent<obstacleSCR>().SliceObstacle();
		}
	}
}

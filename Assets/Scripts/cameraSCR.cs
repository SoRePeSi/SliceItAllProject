using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSCR : MonoBehaviour{
	public Transform knife;

	void Update(){
		transform.position = knife.transform.position + new Vector3(-3f, 0.5f, -4f);
	}
}

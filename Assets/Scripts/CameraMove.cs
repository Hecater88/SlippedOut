using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
	//objetivo del tracking de la cámara
	public Transform target;

	// Update is called once per frame
	void Update () {
		//giramos la cámara hacia el objetivo
		transform.LookAt (target);
	}
}

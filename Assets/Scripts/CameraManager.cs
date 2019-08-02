using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public GameObject player;
	public Transform positionCam;
	public Camera cam;

	//Cuando player toca el collider
	void OnTriggerEnter(Collider other) {
		//cambiamos la posicion de la camara
		cam.transform.position = positionCam.transform.position;
	}
}

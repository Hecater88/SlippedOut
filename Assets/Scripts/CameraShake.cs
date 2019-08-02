using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	static public CameraShake CS;
	Vector3 cameraInitialPosition;
	public float shakeMagnitude = 0.05f;
	public float shakeTime = 0.5f;
	public CameraShake mainCamera;
	static public bool explosion = false;
	// Use this for initialization
	void Start () {
		if (CS == null) {
			CS = GetComponent<CameraShake> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(explosion){
			ShakeIt();
		}
	}

	public void ShakeIt(){
		cameraInitialPosition = mainCamera.transform.position;
		InvokeRepeating("StartCameraShaking",0f, 0.005f);
		Invoke("StopCameraShaking", shakeTime);
	}

	void StartCameraShaking(){
		float cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
		float cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
		Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
		cameraIntermadiatePosition.x += cameraShakingOffsetX;
		cameraIntermadiatePosition.y += cameraShakingOffsetY;
		mainCamera.transform.position = cameraIntermadiatePosition;
	}

	void StopCameraShaking(){
		CancelInvoke("StartCameraShaking");
		mainCamera.transform.position = cameraInitialPosition;
	}
}

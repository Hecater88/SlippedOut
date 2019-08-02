using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour {

	//objetivo del dibujado de la linea
	public GameObject target;

	//referencia al componente LineRenderer
	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		//recuperamos la referenca al LineRenderer
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, target.transform.position);

	}
}

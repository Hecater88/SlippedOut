using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	//referencia al text del canvas
	public Text text;
	//tiempo minimo de mostrado por caracter
	public float displayTimeCharacter = 0.1f;
	//tiempo adicional de mostrdo
	public float addtionalDisplayTime = 0.5f;

	//temporizador para el borrado del texto
	private float clearTime;

	public static TextManager TM;


	void Awake(){
		if (TM == null) {
			TM = GetComponent<TextManager> ();
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (Time.time >= clearTime) {
			text.text = "";
		}
	}

	/// <summary>
	/// Muestra el texto por pantalla
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="textColor">Text color.</param>
	public void Displaymessage (string message,Color textColor){

		//calcula el tiempo de duracion de la frase
		float displayDuration = message.Length * displayTimeCharacter + addtionalDisplayTime;

		//calculamos en que momento debera desaparecer el texto
		clearTime = Time.time + displayDuration;

		//asignamos el texto al mensaje
		//text.text = message;
		StartCoroutine (TypeLetters (message));

		//cambiamos de color el texto
		text.color = textColor;
	}

	/// <summary>
	/// Escribe las palabras letra por letra
	/// </summary>
	/// <returns>The letters.</returns>
	/// <param name="message">Message.</param>
	IEnumerator TypeLetters (string message){
		//ponemos el texto en blanco
		text.text = "";
		//convertimos el mensaje en un array de caracteres y lo recorremos con foreach
		foreach (char letter in message.ToCharArray()) {
			//asignamos cada letra del texto al mensaje
			text.text += letter;
			//interrumpimos corrutina cuando no haya mas texto
			yield return null;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionSound : Reaction {

	//clip de audio que va a ser reproducido
	public AudioClip audioClip;
	//referencia al audioSource
	private AudioSource audioSource;

	void Start(){
		//recuperamos la referencia al componente audiosource
		audioSource =  GetComponent<AudioSource>();
	}

	/// <summary>
	/// Metodo que ejecuta la duracion, con override para que pise la corrutina heredada.
	/// </summary>
	protected override IEnumerator React(){
		//realizamos la espera
		yield return new WaitForSeconds(delay);

		//asignamos el audioclip
		audioSource.clip = audioClip;
		//reproducimos el clip
		audioSource.Play();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionLockPlayer : Reaction {

	//referencia al componente playermovement
	private PlayerMovement playerMovement;


	// Use this for initialization
	void Start () {
		//recupera la referencia al componente playermovement, buscando el objeto Player
		//para que esto funcione es importante que el personaje jugable se llame "Player"
		playerMovement = GameObject.Find ("Player").GetComponent<PlayerMovement> ();
	}


	protected override IEnumerator React(){
		//inicialmente desactivo el control del jugador
		playerMovement.handleInput = false;

		//realizo la espera del tiempo indicado
		yield return new WaitForSeconds (delay);

		//devuelvo el control al jugador una vez terminada la espera
		playerMovement.handleInput = true;
	}


	void OnDisable(){
		//como medida de seguridad
		//cuando se desactive el objeto, devolvemos el control al jugador
		playerMovement.handleInput = true;
	}
}

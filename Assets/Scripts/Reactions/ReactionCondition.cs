using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionCondition : Reaction {

	//nombre de la condicion a modificar
	public string conditionName;
	//estado en que dejaremos la condicion
	public bool conditionStatus;


	protected override IEnumerator React (){
		yield return new WaitForSeconds (delay);

		//modificamos el estado de la condicion
		DataManager.DM.SetCondition (conditionName, conditionStatus);
	}

}

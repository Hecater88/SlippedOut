using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionPlayerMove : Reaction {


		public Transform target;


	protected override IEnumerator React(){
		yield return new WaitForSeconds (delay);
		PlayerMovement.PM.movePlayerAnimation (target);
	}

			


}

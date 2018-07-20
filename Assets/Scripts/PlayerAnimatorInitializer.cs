using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorInitializer : MonoBehaviour {

    public Collider2D weapon;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().GetBehaviour<AttackBehaviour>().weapon = weapon;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

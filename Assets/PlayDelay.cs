using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDelay : MonoBehaviour {

    public AudioSource audio;
    public float delayseconds;

    // Use this for initialization
    void Start () {
        audio.PlayDelayed(delayseconds);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

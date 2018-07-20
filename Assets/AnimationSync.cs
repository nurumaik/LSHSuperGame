using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSync : MonoBehaviour, IBeatListener {

    public BeatSource beatSource;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().speed = (float)beatSource.audioBpm / 60;
        beatSource.AddBeatListener(this);
	}

    public void BeatUpdate()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

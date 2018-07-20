using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IBeatListener {

    bool hadContact = false;
    public int health = 3;
    public BeatSource beatSource;
    public HPBar hpManager;

	// Use this for initialization
	void Start () {
        beatSource.AddBeatListener(this);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D (Collider2D other)
    {
        if (!hadContact && other.CompareTag("Enemy"))
        {
            hadContact = true;
            health--;
            hpManager.UpdateHP(health);
        }
    }

    public void BeatUpdate()
    {
        hadContact = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboScript : MonoBehaviour {

    public TextMesh comboCounter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateComboCounter(int value)
    {
        comboCounter.text = "Combo: " + value.ToString();
    }
}

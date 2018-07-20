using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shittiest code ever
public class HPBar : MonoBehaviour {
    public GameObject oneHP;
    public GameObject twoHP;
    public GameObject threeHP;

	// Use this for initialization
	void Start () {
        oneHP.SetActive(false);
        twoHP.SetActive(false);
        threeHP.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisableAll()
    {
        oneHP.SetActive(false);
        twoHP.SetActive(false);
        threeHP.SetActive(false);
    }

    public void UpdateHP(int newHp)
    {
        DisableAll();
        if (newHp == 1)
        {
            oneHP.SetActive(true);
        }
        else if (newHp == 2)
        {
            twoHP.SetActive(true);
        }
        else if (newHp == 3)
        {
            threeHP.SetActive(true);
        }
    }
}

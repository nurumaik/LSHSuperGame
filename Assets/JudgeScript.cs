using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeScript : MonoBehaviour {

    public MeshRenderer PerfectText;
    public MeshRenderer GoodText;
    public MeshRenderer MissText;

    private void DisableAll()
    {
        PerfectText.enabled = false;
        GoodText.enabled = false;
        MissText.enabled = false;
    }

	// Use this for initialization
	void Start () {
        DisableAll();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateJudge(BeatSource.Grade grade)
    {
        DisableAll();
        switch(grade)
        {
            case BeatSource.Grade.PERFECT:
                PerfectText.enabled = true;
                break;
            case BeatSource.Grade.GOOD:
                GoodText.enabled = true;
                break;
            case BeatSource.Grade.MISS:
                MissText.enabled = true;
                break;
        }
    }
}

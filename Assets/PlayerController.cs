using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    static private float epsilon = 0.001f;

    private Vector3 destination;
    public float speed = 1f;
    public float meshSize = 1f;
    public BeatSource beatSource;
    public JudgeScript judge;

	// Use this for initialization
	void Start () {
	    destination = transform.position;	
	}

    bool HasArrived()
    {
        return (destination - transform.position).magnitude <= epsilon;
    }
	
	// Update is called once per frame
	void Update () {
	    if (!HasArrived())
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BeatSource.Grade grade = beatSource.getCurrentGrade();
            judge.UpdateJudge(grade);
            if (grade == BeatSource.Grade.PERFECT)
            {
                destination = transform.position;
                destination.x -= meshSize * 2;
            }
            else if (grade == BeatSource.Grade.GOOD)
            {
                destination = transform.position;
                destination.x -= meshSize;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            BeatSource.Grade grade = beatSource.getCurrentGrade();
            judge.UpdateJudge(grade);
            if (grade == BeatSource.Grade.PERFECT)
            {
                destination = transform.position;
                destination.x += meshSize * 2;
            }
            else if (grade == BeatSource.Grade.GOOD)
            {
                destination = transform.position;
                destination.x += meshSize;
            }
        }
	}
}

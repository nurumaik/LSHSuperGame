using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBeatListener {

    static private float epsilon = 0.001f;

    private Vector3 destination;
    public float speed = 1f;
    public BeatSource beatSource;
    public JudgeScript judge;
    public Transform groundIndicator;
    public LayerMask groundLayer;
    public bool isGrounded = true;

    private Action action = null;

    class Action
    {
        public KeyCode keyCode;
        public BeatSource.Grade grade; 

        public Action(KeyCode keyCode, BeatSource.Grade grade)
        {
            this.keyCode = keyCode;
            this.grade = grade;
        }
    }

    private void updateGrounded()
    {
        isGrounded = Physics2D.Linecast(transform.position, groundIndicator.position, groundLayer.value);
    }

	// Use this for initialization
	void Start () {
        beatSource.AddBeatListener(this);
        GetComponent<Animator>().speed = (float)beatSource.audioBpm / 60;
	    destination = transform.position;	
	}

    bool HasArrived()
    {
        return (destination - transform.position).magnitude <= epsilon;
    }

    public void BeatUpdate()
    {
        Vector3 oldDestination = destination;
        bool canfall = HasArrived();
        if (action == null)
        {
            judge.UpdateJudge(BeatSource.Grade.MISS);
        }
        else
        {
            switch (action.keyCode)
            {
                case KeyCode.A:
                    switch (action.grade)
                    {
                        case BeatSource.Grade.PERFECT:
                            GetComponent<SpriteRenderer>().flipX = true;
                            destination.x -= Globals.meshSize;
                            break;
                        case BeatSource.Grade.GOOD:
                            GetComponent<SpriteRenderer>().flipX = true;
                            destination.x -= Globals.meshSize;
                            break;
                    }
                    break;
                case KeyCode.D:
                    switch (action.grade)
                    {
                        case BeatSource.Grade.PERFECT:
                            GetComponent<SpriteRenderer>().flipX = false;
                            destination.x += Globals.meshSize;
                            break;
                        case BeatSource.Grade.GOOD:
                            GetComponent<SpriteRenderer>().flipX = false;
                            destination.x += Globals.meshSize;
                            break;
                    }
                    break;
                case KeyCode.W:
                    if (isGrounded)
                    {
                        switch (action.grade)
                        {
                            case BeatSource.Grade.PERFECT:
                                destination.y += 3 * Globals.meshSize;
                                canfall = false;
                                break;
                            case BeatSource.Grade.GOOD:
                                destination.y += 3 * Globals.meshSize;
                                canfall = false;
                                break;
                        }
                    }
                    break;
            }
        }
        if (Physics2D.Linecast(transform.position, destination, groundLayer.value))
        {
            destination = oldDestination;
        }
        oldDestination = destination;
        if (!isGrounded && canfall)
        {
            destination.y -= Globals.meshSize;    
        }
        if (Physics2D.Linecast(transform.position, destination, groundLayer.value))
        {
            destination = oldDestination;
        }
        action = null;
    }
	
	// Update is called once per frame
	void Update () {
        updateGrounded();

	    if (!HasArrived())
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
	}

    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            EventType type = e.type;

            switch (e.type)
            {
                case EventType.KeyDown:
                    if (action == null)
                    {
                        BeatSource.Grade grade = beatSource.getCurrentGrade();
                        judge.UpdateJudge(grade);
                        action = new Action(e.keyCode, grade);
                    }
                    break;
            }
        }
    }

}

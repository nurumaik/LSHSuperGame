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
    private bool canDash = false;
    private int lastHit = -4;
    private bool dead = false;
    private int direction = 1;

    private Action action = null;
    private Animator anim;

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
        anim.SetBool("Grounded", isGrounded);
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        beatSource.AddBeatListener(this);
	    destination = transform.position;	
	}

    bool HasArrived()
    {
        bool res = (destination - transform.position).magnitude <= epsilon;
        anim.SetBool("Reached", res);
        return res;
    }

    private bool canMoveToDestination()
    {
        return !Physics2D.Linecast(transform.position, destination, groundLayer.value);
    }

    private void moveLeft()
    {
        Vector3 oldDestination = destination;
        direction = -1;
        GetComponent<SpriteRenderer>().flipX = true;
        destination.x -= Globals.meshSize;
        if (!canMoveToDestination())
        {
            destination = oldDestination;
        }
    }

    private void moveRight()
    {
        Vector3 oldDestination = destination;
        direction = 1;
        GetComponent<SpriteRenderer>().flipX = false;
        destination.x += Globals.meshSize;
        if (!canMoveToDestination())
        {
            destination = oldDestination;
        }
    }

    private void dashLeft()
    {
        Vector3 oldDestination = destination;
        canDash = false;
        GetComponent<SpriteRenderer>().flipX = true;
        direction = -1;
        destination.x -= 3 * Globals.meshSize;
        while (!canMoveToDestination())
        {
            destination.x += Globals.meshSize;
        }
    }

    private void dashRight()
    {
        Vector3 oldDestination = destination;
        canDash = false;
        GetComponent<SpriteRenderer>().flipX = false;
        direction = 1;
        destination.x += 3 * Globals.meshSize;
        while (!canMoveToDestination())
        {
            destination.x -= Globals.meshSize;
        }
    }

    private void jump()
    {
        Vector3 oldDestination = destination;
        destination.y += 3 * Globals.meshSize;
        while (!canMoveToDestination())
        {
            destination.y -= Globals.meshSize;
        }
        canDash = true;
    }

    private void fall()
    {
        while (canMoveToDestination() && destination.y > -1000)
            destination.y -= Globals.meshSize;
        destination.y += Globals.meshSize;
    }

    public void BeatUpdate()
    {
        hadContact = false;
        if (isGrounded)
            canDash = false;
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
                    if (action.grade != BeatSource.Grade.MISS)
                    {
                        if (isGrounded)
                        {
                            anim.SetTrigger("Moved");
                            moveLeft();
                        }
                        else if (canDash)
                        {
                            anim.SetTrigger("Dash");
                            dashLeft();
                            canfall = false;
                        }
                    }
                    break;
                case KeyCode.D:
                    if (action.grade != BeatSource.Grade.MISS)
                    {
                        if (isGrounded)
                        {
                            anim.SetTrigger("Moved");
                            moveRight();
                        }
                        else if (canDash)
                        {
                            anim.SetTrigger("Dash");
                            dashRight();
                            canfall = false;
                        }
                    }
                    break;
                case KeyCode.W:
                    if (action.grade != BeatSource.Grade.MISS)
                    {
                        if (isGrounded)
                        {
                            jump();
                            anim.SetTrigger("Jump");
                            canfall = false;
                        }
                    }
                    break;
                case KeyCode.Space:
                    if (isGrounded && action.grade != BeatSource.Grade.MISS)
                    {
                        anim.SetTrigger("Attack");
                        if (direction == 1)
                        {
                            moveRight();
                        }
                        else if (direction == -1)
                        {
                            moveLeft();
                        }
                    }
                    break;
            }
        }
        if (!isGrounded && canfall)
        {
            anim.SetTrigger("Fall");
            fall();
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
        if (dead) return;
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

    bool hadContact = false;
    public int health = 3;
    public HPBar hpManager;

    void OnTriggerStay2D (Collider2D other)
    {
        int currentBeat = beatSource.getCurrentBeat();
        if (!hadContact && other.CompareTag("Enemy") && (currentBeat - lastHit) >= 4)
        {
            lastHit = currentBeat;
            hadContact = true;
            health--;
            if (health <= 0)
            {
                dead = true;
                anim.SetTrigger("Die");
            }
            hpManager.UpdateHP(health);
        }
    }

}

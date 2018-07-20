using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour {

    public float speed = 1f;
    public Transform leftWaypoint;
    public Transform rightWaypoint;
    private int direction;
    static private float epsilon = 0.01f;
    public LayerMask playerLayer;
    public float attackRange = 2f;
    public bool isMoving = true;
    public BeatSource beatSource;

	// Use this for initialization
	void Start ()
    {
        direction = 1;	
	}

    private void flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    private Vector3? playerPosition()
    {
        RaycastHit2D hit = Physics2D.Linecast(leftWaypoint.transform.position, rightWaypoint.transform.position, playerLayer.value);
        if (hit)
        {
            return hit.transform.position;
        }
        else
        {
            return null;
        }
    }

    public void Die()
    {
        GetComponent<Animator>().speed = (float)beatSource.audioBpm / 60;
        GetComponent<Animator>().SetTrigger("Die");
        isMoving = false;
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isMoving)
        {
            return;
        }
        bool attack = false;
        Vector3? pos = playerPosition();
        if (pos != null)
        {
            if (Vector3.Distance(transform.position, pos.Value) <= attackRange)
            {
                if ((pos.Value.x - transform.position.x) * direction < 0)
                {
                    direction *= -1;
                    flip();
                }
                attack = true;
            }
            else
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, pos.Value, speed);
                if ((newPos.x - transform.position.x) * direction < 0)
                {
                    direction *= -1;
                    flip();
                }
                transform.position = newPos;
            }
        }
        else
        {
            if (direction == -1)
            {
                transform.position = Vector3.MoveTowards(transform.position, leftWaypoint.position, speed);
                if (Vector3.Distance(transform.position, leftWaypoint.position) <= epsilon)
                {
                    flip();
                    direction = 1;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, rightWaypoint.position, speed);
                if (Vector3.Distance(transform.position, rightWaypoint.position) <= epsilon)
                {
                    flip();
                    direction = -1;
                }
            }
        }
        GetComponent<Animator>().SetBool("Attacking", attack);
    }
}

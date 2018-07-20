using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera2D : MonoBehaviour {
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Rect cameraBounds;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 delta = target.position - transform.position;
            Vector3 destination = transform.position + delta;
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, new Vector3(destination.x, destination.y, transform.position.z), ref velocity, dampTime);
            if (newPosition.x < cameraBounds.xMin || newPosition.x > cameraBounds.xMax)
            {
                newPosition.x = transform.position.x;
            }
            if (newPosition.y < cameraBounds.yMin || newPosition.y > cameraBounds.yMax)
            {
                newPosition.y = transform.position.y;
            }
            transform.position = newPosition; 
        }
    }
}

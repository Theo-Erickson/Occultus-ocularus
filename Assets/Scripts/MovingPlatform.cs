﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to move platforms in any of these three ways:
 *   1. A continuous back and forth pattern without stopping
 *   2. A continuous back and forth pattern that can be started and
 *      stopped at any time with the StartMoving() and StopMoving() functions
 *   3. Extended and retracted with the Extend() and Retract() functions
 * 
 * The distance and speed of movement are configured with the moveDistance and
 * moveSpeed fields, and the orientation of movement can be set to horizontal
 * or vertical with the movementDirection field.
 */

public class MovingPlatform : MonoBehaviour
{

    [Tooltip("If true, platform will move on its own")]
    public bool move;

    [Header("Movement Mode")]
    [Tooltip("If true, the platform shifts between extended and retracted positions. Otherwise, the platform moves continuously.")]
    public bool extendAndRetract;

    [Header("Movement Properties")]
    [Tooltip("Distance platform moves, can be positive or negative")]
    public float distance;
    [Tooltip("The platform's speed in m/s")]
    public float speed;
    public enum MvDir {horizontal, vertical};
    [Tooltip("Make platform move side-to-side or up-and-down")]
    public MvDir direction;

    private bool extend;
    private float back;
    private float front;
    private bool inverted;
    private float displacement;
    private bool forwards;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        displacement = speed / 60;
        if (distance < 0)
            inverted = true;

        // Set front and back bounds for movement
        if (!inverted)
        {
            if (direction == MvDir.horizontal)
            {
                back = originalPosition.x;
                front = originalPosition.x + distance;
            }
            else if (direction == MvDir.vertical)
            {
                back = originalPosition.y;
                front = originalPosition.y + distance;
            }
        }
        else if (inverted)
        {
            if (direction == MvDir.horizontal)
            {
                back = originalPosition.x + distance;
                front = originalPosition.x;
            }
            else if (direction == MvDir.vertical)
            {
                back = originalPosition.y + distance;
                front = originalPosition.y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move && distance != 0)
        {

            if (direction == MvDir.horizontal)
            {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.x >= front)
                {
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.x <= back)
                {
                    forwards = true;
                    if (extendAndRetract && !extend)
                        move = false;
                }

                // If going wrong direction, turn around
                if (!inverted && !forwards && extend)
                    forwards = true;
                else if (inverted && forwards && !extend)
                    forwards = false;

                // Move right or left
                if (move)
                {
                    if (!inverted)
                    {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x - displacement, transform.position.y, transform.position.z);
                    }

                }
            }
            else if (direction == MvDir.vertical)
            {
                // If at bound, turn around or stop if in extendAndRetract mode
                if (transform.position.y >= front)
                {
                    forwards = false;
                    if (extendAndRetract && extend)
                        move = false;
                }
                else if (transform.position.y <= back)
                {
                    forwards = true;
                    if (extendAndRetract && !extend)
                        move = false;
                }

                // If going wrong direction, turn around
                if (!inverted && !forwards && extend)
                    forwards = true;
                else if (inverted && forwards && !extend)
                    forwards = false;

                // Move up or down
                if (move)
                {
                    if (!inverted)
                    {
                        if (forwards && (!extendAndRetract || extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                    }
                    else
                    {
                        if (!forwards && (!extendAndRetract || !extend))
                            transform.position = new Vector3(transform.position.x, transform.position.y - displacement, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y + displacement, transform.position.z);
                    }
                }
            }
        }
    }

    public void StartMoving()
    {
        move = true;
    }

    public void StopMoving()
    {
        move = false;
    }

    public void Extend()
    {
        move = true;
        if (!inverted)
            extend = true;
        else
            extend = false;
    }

    public void Retract()
    {
        move = true;
        if (!inverted)
            extend = false;
        else
            extend = true;
    }

    // Makes player stick to platform when they're standing on it
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Platform collided with something");

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = this.transform;
            Debug.Log("Attached player to platform");
        }
    }

    // Unsticks player from platform when they jump or walk off of it
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = null;
        }
    }

}

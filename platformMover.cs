using UnityEngine;
using System.Collections.Generic;


public class platformMover : MonoBehaviour
{
    public Transform[] points;
    public bool active;
    public bool elevator;
    public float movementSpeed;
    public float finishDistance;
    public bool lookAtDestination;
    [Range(0.01f, 0.5f)]
    public float lookSpeed;
    public bool debug;

    Rigidbody rb;
    private int destPoint = 0;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (elevator == true)
        {
            transform.position = new Vector3(points[0].position.x, points[0].position.y + finishDistance, points[0].position.z);
        }

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        destPoint = (destPoint + 1) % points.Length;

    }

    void FixedUpdate()
    {
        if (active == true)
        {
            Vector3 direction = (points[destPoint].position - transform.position).normalized;
            Vector3 direction1 = (new Vector3(points[destPoint].position.x, 0f, points[destPoint].position.z) - new Vector3(transform.position.x, 0f, transform.position.z)).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            var distance = (transform.position - points[destPoint].position).magnitude;

            if (lookAtDestination == true)
            {
                Quaternion lookRota = Quaternion.LookRotation(direction1);
                Quaternion slerp = Quaternion.Slerp(rb.rotation, lookRota, lookSpeed);
                rb.MoveRotation(slerp);
            }

            if (distance < finishDistance)
            {
                if (elevator == true)
                {
                    active = false;

                    if (destPoint == 0)
                    {
                        destPoint = 1;
                    } else
                    {
                        destPoint = 0;
                    }
                    return;
                }
                GotoNextPoint();
            }

        }
    }


    void OnDrawGizmos()
    {
        if (debug == true)
        {
            Gizmos.color = Color.green;
            foreach (Transform trans in points)
            {
                Gizmos.DrawSphere(trans.position, 0.5f);
            }
        }
    }



}

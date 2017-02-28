using UnityEngine;
using System.Collections;

public class flightPath : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float stoppingDistance;
    public bool stopAtTarget;
    public float rayDistance;
    Rigidbody rb;
    Vector3 dir;
    Vector3 dir1;
    public bool debug;
    public LayerMask layerMask;
    bool dontMove;
    bool noHit;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        dontMove = false;
    }

    void Reset()
    {
        moveSpeed = 30;
        stopAtTarget = false;
        stoppingDistance = 0;
        rayDistance = 20;
    }


    void FixedUpdate()
    {
        if (dontMove != true)
        {
            dir = (target.position - transform.position).normalized;
        } else
        {
            dir = (target.position - transform.position).normalized;
        }

        if (dontMove != true)
        {
            DoCasts();
        }

        CheckDistance();



        if (dir != Vector3.zero)
        {
            var rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, Time.deltaTime));

            if (!dontMove)
            {
                rb.MovePosition(rb.position += transform.forward * moveSpeed * Time.deltaTime);
            }

        }
    }

    void DoCasts()
    {
        noHit = true;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            noHit = false;
            if (hit.transform != transform)
            {
                if (debug == true)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
                dir += hit.normal * 20;
            }
        }


        var leftR = transform.position;
        var rightR = transform.position;
        var up = transform.position;
        var down = transform.position;

        leftR.x -= 5;
        rightR.x += 5;
        up.y += 5;
        down.y -= 5;


        if (Physics.Raycast(leftR, transform.forward, out hit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            noHit = false;
            if (hit.transform != transform)
            {
                if (debug == true)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                }
                dir += hit.normal * rayDistance;
            }
        }

        if (Physics.Raycast(rightR, transform.forward, out hit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            noHit = false;
            if (hit.transform != transform)
            {
                if (debug == true)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.yellow);
                }
                dir += hit.normal * rayDistance;
            }
        }

        if (Physics.Raycast(up, transform.forward, out hit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            noHit = false;
            if (hit.transform != transform)
            {
                if (debug == true)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.blue);
                }
                dir += hit.normal * rayDistance;
            }
        }

        if (Physics.Raycast(down, transform.forward, out hit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            noHit = false;
            if (hit.transform != transform)
            {
                if (debug == true)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.cyan);
                }
                dir += hit.normal * rayDistance;
            }
        }


    }

    void CheckDistance()
    {
        var distance = Vector3.Distance(transform.position, target.position);
        dir1 = (target.position - transform.position).normalized;

        if (distance <= stoppingDistance)
        {
            RaycastHit hitto;
            if (Physics.Raycast(transform.position, dir1, out hitto, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {

                if (hitto.transform == target)
                {
                    if (stopAtTarget == true)
                    {
                        dontMove = true;
                    }
                } 
                else
                {
                    dontMove = false;
                }
            }


        } else
        {

            dontMove = false;
        }
    }
}

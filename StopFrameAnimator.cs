using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFrameAnimator : MonoBehaviour
{
    public Animator Animator;
    public int FrameInterval;
    int liveFrameInterval;
    bool isNormalSpeed;
    public bool IsActive;

    private void Update()
    {
        if(IsActive)
        {
            liveFrameInterval++;

            if (liveFrameInterval >= FrameInterval)
            {
                liveFrameInterval = 0;

                if (isNormalSpeed)
                {
                    Animator.speed = 0;
                } 
                else
                {
                    Animator.speed = 1;
                }

                isNormalSpeed = !isNormalSpeed;
            }
        }
 
    }

}

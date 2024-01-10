using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimationManager : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent myAgent; 
    public bool IsRunning;

    void Start()
    {
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (myAgent != null)
        {
            if (myAgent.velocity.magnitude > 0.2f)
                IsRunning = true;
            else
                IsRunning = false;
            animator.SetBool("IsRunning", IsRunning);
        }
    }
}
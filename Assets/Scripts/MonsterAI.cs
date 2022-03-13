using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public Transform target;

    public Animator animator;

    public bool attack = false;
    public bool hurt = false;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        navMesh.destination = target.position;
        AnimationControl();
    }

    void AnimationControl()
    {
        if(navMesh.velocity.magnitude > 0)
        {
            animator.SetBool("is_walking", true);
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            transform.LookAt(other.gameObject.transform);
            animator.SetTrigger("attack_trigger");
        }
    }
}

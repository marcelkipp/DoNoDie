using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;

    Animator animator;
    Transform target;
    NavMeshAgent agent;
    EnemyBase enemyBase;
    Rigidbody rb;
    Collider collider; 

    bool stopWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyBase = GetComponent<EnemyBase>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>(); 
    }

    // Update is called once per frame
    void Update()
    {

        checkIfAgentShouldBeStopped();

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius && !stopWalking)
        {

            agent.SetDestination(target.position);
            animator.SetBool("running", true);

            if (distance <= agent.stoppingDistance)
            {
                //Attack the target
                attackAnimation();
                FaceTarget();
            }
            else
            {
                disableAnimations();
            }
        } else
        {
            animator.SetBool("running", false);
        }
    
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; // get the direction to the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // rotation to point to the target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4.0f); // apply rotation to the character with interpolation for the smoothness
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            if (enemyBase.reduceLife(collision.gameObject.GetComponent<BulletMovement>().damage))
            {
                letHimDie();
            } else
            {
                animator.SetTrigger("hit");
            }

        }
    }

    private void checkIfAgentShouldBeStopped()
    {

        if (enemyBase.life <= 0)
        {
            letHimDie();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Reaction Hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Dying"))
        {
            agent.Stop();
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Dying"))
            {
                agent.Resume();
            }
        }
    }

    void letHimDie()
    {
        agent.Stop();
        Destroy(collider);
        rb.isKinematic = false;
        animator.SetBool("die", true);

        InvokeRepeating("removeEnemy", 5.0f, 10.0f);
    }

    void attackAnimation()
    {
        float random = Random.Range(0.0f, 1.0f);

        if (random <= 0.5f)
        {
            animator.SetBool("punch_1", true);
        }
        else
        {
            animator.SetBool("punch_2", true);
        }

    }

    void disableAnimations()
    {
        animator.SetBool("punch_1", false);
        animator.SetBool("punch_2", false);
    }

    void removeEnemy()
    {
        Destroy(gameObject);
    }

    
}

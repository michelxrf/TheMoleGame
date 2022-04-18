using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public Vector3 targetPosition;
    public Transform playerTransform;
    public string aiMode; // hunt, idle, stalk, escape
    private bool shouldRecalculateTarget = true;
    private LayerMask raycastFilter = 0b1100000000;

    public Transform attackPoint;
    public LayerMask playerLayer;
    public GameObject heartPrefab;
    public GameObject spiderParticles;

    public Animator animator;

    public int monsterHealth;
    public float attackRange;
    public int monsterDamage;
    public float spawnHeartChance;
    public float idleSpeed;
    private float attackSpeed;
    public float detectionRange = 5f;
    private float lostPlayerTime;
    private float stalkRange;
    private bool monsterSeesPlayer = false;
    private Vector3 lastPlayerLocation = new Vector3();
    private float aggroChance;
    public float minAggroChance, maxAggroChance;
    private bool stalkTimerActive = false;
    private float fleeMultiplier = 1.5f;
    public float memoryDuration = 5f;

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        stalkRange = (GameObject.Find("TopLight").transform.position.y - 0.5f) * Mathf.Tan((20f + 5f * GameData.lamp) * Mathf.Deg2Rad);
        navMesh.speed = idleSpeed;
        attackSpeed = 2*idleSpeed;

        aggroChance = Random.Range(minAggroChance, maxAggroChance); // using negative min will add pacifist spiders
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldRecalculateTarget)
        CalculateTarget();
        
        navMesh.destination = targetPosition;
        
        WalkingAnimationControl();

        DetectPlayer();
    }

    void WalkingAnimationControl()
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

    public void Hurt(int damageTaken)
    {
        monsterHealth -= damageTaken;

        animator.SetTrigger("hurt_trigger");
        aiMode = "stalk";

        if(monsterHealth == 1)
        {
            StopCoroutine(StrikeTimer());
            aiMode = "fleeing";
        }

        if(monsterHealth <= 0)
        {
            GameData.monsterPopulation--;
            GameData.killedMonstersThisLevel++;

            spawnHeart();
            Instantiate(spiderParticles, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }

    public void EnableHitBox()
    {
        Collider[] playerHurtBox = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach(Collider player in playerHurtBox)
        {
            var playerScript = player.transform.gameObject.GetComponent<PlayerController>();

            if(playerScript)
            playerScript.Hurt(monsterDamage);
        }    

    animator.ResetTrigger("attack_trigger");
    
    if(aiMode != "fleeing")
    aiMode = "stalk";
    }

    private void spawnHeart()
    {
        if(Random.Range(0f, 100f) < spawnHeartChance)
        {
            var heart = Instantiate(heartPrefab, transform.position, Quaternion.identity, GameObject.Find("InteractableParent").transform);
            heart.transform.LookAt(new Vector3(0, 50, 0));
        }
    }

    private void CalculateTarget()
    {
        switch (aiMode)
        {
            case "hunt":
                StopCoroutine(RecalculateTargetTimer());
                targetPosition = playerTransform.position;
                navMesh.updateRotation = !monsterSeesPlayer;
                navMesh.speed = attackSpeed;
                transform.LookAt(lastPlayerLocation);
                break;

            case "idle":
                targetPosition = new Vector3(transform.position.x + 5 * Random.Range(-1f, 1f), transform.position.y, transform.position.z + 5 * Random.Range(-1f, 1f));
                shouldRecalculateTarget = false;
                navMesh.speed = idleSpeed;
                navMesh.updateRotation = true;
                StartCoroutine(RecalculateTargetTimer());
                break;

            case "stalk":
                StopCoroutine(RecalculateTargetTimer());
                
                if(!stalkTimerActive)
                StartCoroutine(StrikeTimer());

                navMesh.speed = attackSpeed;
                navMesh.updateRotation = !monsterSeesPlayer;
                   
                var distance = transform.position - playerTransform.position;

                if(monsterSeesPlayer || (playerTransform.position - lastPlayerLocation).magnitude < stalkRange * .3f)
                {
                    targetPosition = playerTransform.position + distance.normalized * stalkRange;
                    transform.LookAt(lastPlayerLocation);
                }
                else
                {
                    targetPosition = lastPlayerLocation;
                }
                break;

            case "fleeing":
                StopCoroutine(RecalculateTargetTimer());

                navMesh.speed = attackSpeed;
                navMesh.updateRotation = true;

                shouldRecalculateTarget = true;

                targetPosition = playerTransform.position + (transform.position - playerTransform.position).normalized * stalkRange * fleeMultiplier;
                fleeMultiplier += .05f;
                break;

            default:
                Debug.LogError("AI mode not defined correctly.");
                break;
        }
        
    }

    IEnumerator RecalculateTargetTimer()
    {
        yield return new WaitForSeconds(5);
        shouldRecalculateTarget = true;
    }

    IEnumerator StrikeTimer()
    {
        stalkTimerActive = true;
        yield return new WaitForSeconds(1);
        if(aiMode == "stalk")
        {
            if(Random.Range(0f, 100f) < aggroChance)
            {
                aiMode = "hunt";
            }
        }
        stalkTimerActive = false;
    }

    private void DetectPlayer()
    {
        RaycastHit hit;
        var distance = playerTransform.position - transform.position;

        if(Physics.Raycast(transform.position, distance, out hit, detectionRange, raycastFilter))
        {
            Debug.DrawRay(transform.position, distance, Color.green, .1f);
            monsterSeesPlayer = hit.collider.gameObject.layer == 9;
        }
        else
        {
            monsterSeesPlayer = false;
        }

        if(monsterSeesPlayer)
        {
            lastPlayerLocation = playerTransform.position;
            if(aiMode == "idle")
            aiMode = "stalk";

            if(monsterHealth == 1)
            aiMode = "fleeing";

            shouldRecalculateTarget = true;
        }

        if(aiMode != "idle" && distance.magnitude > stalkRange * 1.5f)
        {
            lostPlayerTime += Time.deltaTime;
            if(lostPlayerTime > memoryDuration)
            {
                aiMode = "idle";
                fleeMultiplier = 1.5f;
                shouldRecalculateTarget = true;
            }
        }
        else
        {
            lostPlayerTime = 0;
        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public Rigidbody gravityController;
    public GameObject dirtParticles;
    
    public GameObject moleModel;
    public GameObject spadeModel;
    public Light flashLight;
    public Light overLight;

    public Animator animator;

    public float speed = 3f;

    private bool playerIsAlive = true;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask monstersLayer;
    public LayerMask destructiblesLayer;
    public LayerMask undestructiblesLayer;
    public float generateNewMapHeight = -3f;

    public bool isInvulnerable = false;

    private void Start()
    {
        SaveSystem.SaveGame();
        Cursor.visible = false;

        speed = 1 + .25f*GameData.speed;

        switch (GameData.lamp)
        {
            case 1:
                overLight.spotAngle = 50f;
                overLight.intensity = 8f;

                flashLight.spotAngle = 60f;
                flashLight.range = 6f;
                flashLight.intensity = 1f;
                break;

            case 2:
                overLight.spotAngle = 60f;
                overLight.intensity = 9f;

                flashLight.spotAngle = 75f;
                flashLight.range = 7f;
                flashLight.intensity = 1.5f;
                break;

            case 3:
                overLight.spotAngle = 70f;
                overLight.intensity = 10f;

                flashLight.spotAngle = 90f;
                flashLight.range = 12f;
                flashLight.intensity = 2f;
                break;

            default:
                Debug.LogError("GameData.lamp level not valid.");
                break;
        }
        /*level1: angle 50, intensity 8;
          level2: angle 60, intensity 9;
          level3: angle 70, intensity 10;
        */

        // flashligth
        /*level1: angle 60, range 6, intensitty 1
          level2: angle 75, range 7,  intensity 1.5
          level3: angle 90, range 12, intesity 2
        
        */    
    }

    void Update()
    {
        if(playerIsAlive)
        {
            if(gravityController.isKinematic == true && GameData.gameIsPaused == false)
            {
                Move();
                Strike();
            }
            checkFalls();
        }
        
    }
    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0)
        {
            animator.SetBool("is_walking", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTransform.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle , 0f);
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime );
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
        
        // DEBUG for regenerating level
        if(Input.GetKeyDown("r"))
        {
            GameData.level += 5;

            GameData.storedSilver = 300;
            GameData.storedGold = 300;
            GameData.storedEmerald = 300;

            SceneManager.LoadScene("Play");
        }
    }

    void Strike()
    {
        if(Input.GetMouseButton(0))
        {
            animator.SetTrigger("strike_trigger");
        }
    }

    void OnTriggerEnter(Collider other) {
        // check for level end point
        if(other.gameObject.name == "LevelEnd" || other.gameObject.name == "LevelEnd(Clone)")
        {
            animator.SetTrigger("fall_trigger");
            gravityController.isKinematic = false;
        } 
    }

    void checkFalls()
    {
        if(transform.position.y < generateNewMapHeight)
        {
            if(GameData.leaveMines)
            {
                SceneManager.LoadScene("Retreat");
            }
            else
            {
                GameData.level++;
                GameData.killedMonsters += GameData.killedMonstersThisLevel;
                GameData.killedMonstersThisLevel = 0;

                if(GameData.level > GameData.highestLevel)
                GameData.highestLevel = GameData.level;

                SceneManager.LoadScene("Play");
            }
        }
    }

    public void enableHitBox()
    {
        Collider[] hitStuff = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider stuff in hitStuff)
        {
            // Breakable walls
            if(stuff.transform.gameObject.layer == 8)
            {
                bool shouldDestroyIt = true;
                var goldSpawnScript = stuff.transform.gameObject.GetComponent<SpawnGold>();
                
                if(goldSpawnScript)
                shouldDestroyIt = goldSpawnScript.Hit();

                if(shouldDestroyIt)
                {
                    Instantiate(dirtParticles, stuff.transform.position, Quaternion.identity);

                    Destroy(stuff.transform.gameObject);
                }
            }

            // Monsters
            if(stuff.transform.gameObject.layer == 6)
            {
                var monsterScript = stuff.transform.gameObject.GetComponent<MonsterAI>();

                if(monsterScript)
                monsterScript.Hurt(GameData.damage);
            }
        }

        animator.ResetTrigger("strike_trigger");

        //TODO: react to unbreakable walls
    }

    public void Hurt(int damage)
    {
        if(playerIsAlive && !isInvulnerable)
        {
            if(GameData.armor > 0)
            {
                GameData.armor--;      
            }
            else
            {
                GameData.health -= damage;
            }

            animator.SetTrigger("hurt_trigger");

            if(GameData.health <= 0)
            {
                animator.SetTrigger("death_trigger");
                StartCoroutine(FadeLights(50, 1));
                playerIsAlive = false;

                flashLight.intensity = 0;
            }
            else
            {
                StartCoroutine(BecomeInvulnerable(2f));
            }
        }
        
    }

    public void PlayerIsDead()
    {
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator BecomeInvulnerable(float seconds = 1f)
    {
        isInvulnerable = true;
        StartCoroutine(Blink(.2f, seconds));
        yield return new WaitForSeconds(seconds);
        isInvulnerable = false;
    }

    IEnumerator Blink(float time = .2f, float totalDuration = 1f)
    {
        while(totalDuration > 0)
        {
            moleModel.SetActive(false);
            spadeModel.SetActive(false);
            yield return new WaitForSeconds(time);

            moleModel.SetActive(true);
            spadeModel.SetActive(true);
            yield return new WaitForSeconds(time);

            totalDuration -= time*2;
        }
    }

    IEnumerator FadeLights(int angleSteps, float totalSeconds)
    {
        float degreesPerStep =  (overLight.spotAngle - 25)/angleSteps;
        float waitPeriod = totalSeconds/angleSteps;

        while(overLight.spotAngle > 25)
        {
            overLight.spotAngle -= degreesPerStep;
            yield return new WaitForSeconds(waitPeriod);
        }
    }
}

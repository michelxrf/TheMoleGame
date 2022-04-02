using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public Rigidbody gravityController;

    public Animator animator;

    public float speed = 1f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask monstersLayer;
    public LayerMask destructiblesLayer;
    public LayerMask undestructiblesLayer;
    public float generateNewMapHeight = -3f;

    private void Start()
    {
        SaveSystem.SaveGame();
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if(gravityController.isKinematic == true && GameData.gameIsPaused == false)
        {
            Move();
            Strike();
        }
        checkFalls();
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
            GameData.level++;

            if(GameData.level > GameData.highestLevel)
            GameData.highestLevel = GameData.level;

            SceneManager.LoadScene("Play");
        }
    }

    void enableHitBox()
    {
        //TODO: detect enemies
        Collider[] hitWalls = Physics.OverlapSphere(attackPoint.position, attackRange, destructiblesLayer);
        //TODO: detect undestructibles

        //TODO: hurt hit enemies
        foreach (Collider breakableWall in hitWalls)
        {
            var spawnFunction = breakableWall.transform.gameObject.GetComponent<SpawnGold>();
            if(spawnFunction != null)
                spawnFunction.Hit();

            Destroy(breakableWall.transform.gameObject);      
        }

        animator.ResetTrigger("strike_trigger");

        //TODO: react to unbreakable walls
    }
}

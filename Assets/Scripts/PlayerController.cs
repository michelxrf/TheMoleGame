using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;
    public Rigidbody gravityController;

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
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if(gravityController.isKinematic==true)
        Move();
        Strike();
        checkFalls();
    }
    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle , 0f);
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime );
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
            //TODO: play animation
            
            //TODO: detect enemies
            //TODO: detect destructibles
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
            //TODO: don't destroy walls
        
        
        }
    }

    void OnTriggerEnter(Collider other) {
        // check for level end point
        if(other.gameObject.name == "LevelEnd" || other.gameObject.name == "LevelEnd(Clone)")
        {
            gravityController.isKinematic = false;
        }

        // check for treasures
        if(other.gameObject.name == "Silver(Clone)")
        {
            GameData.money += 0.2f;
            Destroy(other.gameObject);
        }
        else if(other.gameObject.name == "Gold(Clone)")
        {
            GameData.money += 1f;
            Destroy(other.gameObject);
        }
        else if(other.gameObject.name == "Emerald(Clone)")
        {
            GameData.money += 5f;
            Destroy(other.gameObject);
        }
    }

    void checkFalls()
    {
        if(transform.position.y < generateNewMapHeight)
        {
            GameData.level++;
            SceneManager.LoadScene("Play");
        }
    }
}

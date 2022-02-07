using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;

    public float speed = 1f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask monstersLayer;
    public LayerMask destructiblesLayer;
    public LayerMask undestructiblesLayer;

    private void Start()
    {
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Strike();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, .4f, transform.position.z);
        }
    }

    void Strike()
    {
        if(Input.GetButton("Fire1"))
        {
            //TODO: play animation
            
            //TODO: detect enemies
            //TODO: detect destructibles
            Collider[] hitWalls = Physics.OverlapSphere(attackPoint.position, attackRange, destructiblesLayer);
            //TODO: detect undestructibles

            //TODO: hurt hit enemies
            foreach (Collider breakableWall in hitWalls)
            {
                Destroy(breakableWall.transform.gameObject);      
            }
            //TODO: don't destroy walls
        
        
        }
    }
}

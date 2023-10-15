using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    Rigidbody rb;
    public float speed=0.5f;
    float movementspeed=0f;
    Animator anim;
    bool isGrounded=true;
    public float jumpForce = 2.0f;

    Vector3 rotate;
    Vector3 MoveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection = Vector3.zero;
        rotate = Vector3.zero;

        movementspeed = 0;
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                MoveDirection += Input.GetAxisRaw("Vertical") * Vector3.forward;
                anim.SetFloat("Speed", 1);
                 movementspeed = 1;

            }
             if (Input.GetAxisRaw("Horizontal") != 0)
            {
                MoveDirection += Input.GetAxisRaw("Horizontal") * Vector3.right;
                anim.SetFloat("Speed", 1);
                movementspeed = 1;



            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                 movementspeed *= 2;
             }
             else
             {
                anim.SetBool("Run", false);
             }
            if(MoveDirection==Vector3.zero)
            {
                anim.SetFloat("Speed", 0);
            }
            MoveDirection *= speed;
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Vector3 jump = new Vector3(0.0f, 100f, 0.0f);
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                anim.SetTrigger("Jump");
                anim.SetBool("isGround", isGrounded);
            }
        
        
       
    }
    private void FixedUpdate()
    {
        Quaternion newRotation = Quaternion.LookRotation(MoveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        rb.MovePosition(transform.position+ movementspeed*transform.forward*speed);
        if (cam != null)
        {
            cam.transform.LookAt(this.transform);
        }
        
       



    }
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGround", isGrounded);
        }
    }
}

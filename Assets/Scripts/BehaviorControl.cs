using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorControl : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    //Run
    private float maxSpeed = 3f;
    bool facingRight;

    //Attack
    bool attack;
    bool jumpAttack;

    // jump
    Animator amin;
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    float jumpForce = 250f;

    // Use this for initialization
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        amin = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // HandleJump();
        grounded = Physics2D.OverlapCircle(
            groundCheck.position, groundRadius, whatIsGround);
        amin.SetBool("Ground", grounded);
        

        HandleLayers();


        // HandleMovement();
        float move = Input.GetAxis("Horizontal");
        HandleMovement(move);
        Flip(move);

        //HandleAttacks();
        HandleAttacks();
        ResetValues();
    }

    void Update()
    {
        HandleInput();
    }

    void Flip(float move)
    {
        if (move > 0 && !facingRight || move<0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

        }
            
    }

    void HandleMovement(float move)
    {
        if (!this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack")
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack2")
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack3"))
        {
            amin.SetFloat("Speed", Mathf.Abs(move));
            myRigidbody.velocity = new Vector2(move * maxSpeed, myRigidbody.velocity.y);
        }

        
       
    }

    void HandleAttacks()
    {
        if (attack && grounded)
        {
            if (this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack2") 
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack3"))
            {
                amin.SetBool("Base_Attack3", true);
                amin.SetBool("Base_Attack2", false);

            }
            else if (this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack")
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack2"))
            {
                amin.SetBool("Base_Attack2", true);
            }
            else if (!this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack")
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack2")
                && !this.amin.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack3"))
            {
                amin.SetTrigger("Base_Attack");
                myRigidbody.velocity = Vector2.zero;
                amin.SetBool("Base_Attack3", false);
               
            }
        }
        if (jumpAttack && !grounded && !this.amin.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            amin.SetBool("JumpAttack", true);
        }
        if(!jumpAttack && !this.amin.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            amin.SetBool("JumpAttack", false);
        }

    }

    void ResetValues()
    {
        attack = false;
        jumpAttack = false;
    }

    void HandleInput()
    {
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            amin.SetBool("Ground", false);
            amin.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            attack = true;
            jumpAttack = true;
        }
    }

    private void HandleLayers()
    {
        if (myRigidbody.velocity.y < 0)
        {
            amin.SetBool("land", true);
        }

        if (!grounded)
        {
            amin.SetLayerWeight(1, 1);
            amin.SetTrigger("Jump");
        }
        else
        {
            amin.SetLayerWeight(1, 0);
            amin.ResetTrigger("Jump");
            amin.SetBool("land", false);
        }
    }

}

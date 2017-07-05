using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    public ControlGUIScript healthStat;

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Jump { get; set; }

    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }

    }

    private Vector2 startPos;

    // Use this for initialization
    public override void Start()
    {

        base.Start();
        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y <= -14f)
        {
            healthStat.Value = 0;
            Destroy(gameObject, 1);
        }
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleLayers();


    }

    void Flip(float move)
    {
        if (move > 0 && !facingRight || move < 0 && facingRight)
        {
            changeDirection();
        }

    }

    void HandleLayers()
    {
        if (MyRigidbody.velocity.y < 0)
        {
            amin.SetBool("land", true);
        }
        if (!OnGround)
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

    void HandleMovement(float move)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            amin.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidbody.velocity =
                new Vector2(move * movementSpeed, MyRigidbody.velocity.y);
        }


        amin.SetFloat("Speed", Mathf.Abs(move));



    }



    void HandleInput()
    {
        if (OnGround && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            amin.SetTrigger("Jump");
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            HandleAttack();
        }
    }

    void HandleAttack()
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
            MyRigidbody.velocity = Vector2.zero;
            amin.SetBool("Base_Attack3", false);

        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(
            point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    return true;
                }
            }
        }
        return false;

    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        healthStat.Value -= 0.1f;
        if (!IsDead)
        {
            amin.SetTrigger("damage");
        }
        else
        {
            amin.SetLayerWeight(1, 0);
            amin.SetTrigger("dead");
            Destroy(gameObject, 1);
        }
        yield return null;
    }
}

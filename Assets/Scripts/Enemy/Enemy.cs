﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private IEnemy currentState;

    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }

    }

    // Use this for initialization
    public override void Start()
    {

        base.Start();

        ChangeState(new IdleState());
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsDead) {
            if (!TakingDamage)
            {
                currentState.Execute();
                LookAtTarget();
            }
        }

        
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                changeDirection();
            }
        }
    }

    public void ChangeState(IEnemy newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            amin.SetFloat("Speed", 1);

            transform.Translate(
                GetDirection() * movementSpeed * Time.deltaTime);
        }
        else if (Attack)
        {
            amin.SetFloat("Speed", 0);
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
        {
            amin.SetTrigger("damage");
        }
        else
        {
            amin.SetTrigger("dead");
            yield return null;
            Destroy(gameObject, 1);
        }
    }
}

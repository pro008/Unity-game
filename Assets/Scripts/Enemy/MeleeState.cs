using System;
using System.Collections;
using UnityEngine;

public class MeleeState : IEnemy
{
    private Enemy enemy;

    private float attackTimer;
    private float attackCoolDown = 2;
    private bool canAttack = true;


    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }


    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }
    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }
        if (canAttack)
        {
            canAttack = false;
            enemy.amin.SetTrigger("Base_Attack");
        }
    }
}

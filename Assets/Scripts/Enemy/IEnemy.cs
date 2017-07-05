using System.Collections;
using UnityEngine;

public interface IEnemy {
    void Execute();
    void Enter(Enemy enemy);
    void Exit();
    void OnTriggerEnter(Collider2D other);

}

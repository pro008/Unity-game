using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStage : MonoBehaviour {
    void OnColliderEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            print("phong123");
            Debug.Log("its Colliding!");
        }
    }
}

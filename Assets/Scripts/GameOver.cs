using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {
    public ControlGUIScript healthStat;
    public float restartDelay = 5f;

    Animator aminGameOver;
    float restartTimer;

	void Awake () {
        aminGameOver = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(healthStat.Value <= 0)
        {
            aminGameOver.SetTrigger("GameOver");
            restartTimer += Time.deltaTime;
            if(restartTimer >= restartDelay)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
	}
}

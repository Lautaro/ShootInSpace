﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour {

    [SerializeField]
    Text playerLife;
    [SerializeField]
    Text playerEnergy;
	
	
	// Update is called once per frame
	void Update () {

        playerLife.text = "LIFES " + ShootInSpaceSceneManager.Player.Lifes;
        playerEnergy.text = "ENERGY " + ShootInSpaceSceneManager.Player.Energy;
		
	}
}
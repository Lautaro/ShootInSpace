using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : Pickup {


	// Use this for initialization
	void Start () {
        EntityType = EntityType.EnergyPickup;
        SfxManager.PlaySfx(SfxNames.ReactivateShield.ToString());
	}
	
}

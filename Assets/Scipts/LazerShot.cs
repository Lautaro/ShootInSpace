using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerShot : MonoBehaviour {

    public float Speed;

	// Use this for initialization
	void Start () {
       
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        transform.position += transform.up * Time.deltaTime * Speed;
        
        if (GetComponentInChildren<SpriteRenderer>().isVisible == false)
        {
            GameObject.DestroyImmediate(gameObject);
        }
	}
}

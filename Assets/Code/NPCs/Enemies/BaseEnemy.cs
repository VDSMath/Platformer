using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour {

    Collider2D myCol;
    Rigidbody2D myRB;

    GameObject player;

    [Header("Enemy Attributes")]
    [SerializeField]
    int health;
    [SerializeField]
    int damage;

	// Use this for initialization
	void Start () {
        myCol = GetComponent<Collider2D>();
        myRB = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            Hit();
        }
    }

    void Hit()
    {
        Debug.Log("hit");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField]
    private float followSpeed;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        Follow();
	}

    void Follow()
    {
        float tempZ = transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, followSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, tempZ);
    }
}

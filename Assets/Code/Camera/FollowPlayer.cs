using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField]
    private float followSpeed;

    private GameObject player;
    GameObject playerCamPos;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerCamPos = player.transform.Find("Camera Position").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Follow();
	}

    void Follow()
    {
        float tempZ = transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, playerCamPos.transform.position, followSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, tempZ);
    }
}

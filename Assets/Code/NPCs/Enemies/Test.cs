using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    [Header("Movement Options")]
    [SerializeField]
    float wanderBreakTime;
    [SerializeField]
    float wanderDistance;
    [SerializeField]
    float wanderSpeed;

    bool move;
    float targetPosition, 
          timer;


	// Use this for initialization
	void Start () {
        timer = 0;

        move = false;

        targetPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Wander();

        if (move)
        {
            Move();
        }
	}

    void Move()
    {
        float tempZ = transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition, transform.position.y), wanderSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, tempZ);

        if(transform.position.x == targetPosition)
        {
            timer = 0;

            move = false;
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if(timer >= wanderBreakTime)
        {
            timer = -999;

            move = true;

            targetPosition = transform.position.x + (int)Random.Range(-1, 1)*wanderDistance;
        }
    }
}

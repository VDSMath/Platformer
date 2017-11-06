using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour {

    [Range(1,5)]
    [SerializeField]
    private float movementSpeed;

    [Range(1, 2)]
    [SerializeField]
    private int jumpHeight;

    [SerializeField]
    private GameObject bullet;
    [Range(3,7)]
    [SerializeField]
    private int bulletSpeed;

    [SerializeField]
    GameObject shootPoint;
    
    private Rigidbody2D myRB;

    Animator animator;

    private int lastDirection;

    //Animation Variables
    private float idleTime, shooting;

    private bool dashing, jumping;
    private float dashTimer, movementMultiplier;

    // Use this for initialization
    void Start ()
    {
        dashTimer = 0;
        idleTime = 0;
        jumping = false;
        lastDirection = 1;
        movementMultiplier = 5;
        shooting = 1;
        animator = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Jump();
        Shoot();
        Walk();

        ShootAnimation();
	}

    void Dash()
    {
        if (!dashing)
            dashing = Input.GetButtonDown("Fire2");

        if (dashTimer <= .5f && dashing)
        {
            if (Input.GetButton("Fire2"))
            {
                movementMultiplier = 8;
                dashTimer += Time.deltaTime;
            }
            else
            {
                movementMultiplier = 5;
                dashTimer = 0;
                dashing = false;
            }
        }
        else
        {
            movementMultiplier = 5;
            dashTimer = 0;
            dashing = false;
        }

    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject b = Instantiate(bullet);
            Destroy(b, 10);
            b.transform.position = Vector2.zero;
            Vector3 temp = shootPoint.transform.position;
            b.transform.position = new Vector3(temp.x, temp.y, temp.z);
            b.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed * 100f * lastDirection, 0));
            shooting = 0f;
        }
    }

    void ShootAnimation()
    {
        shooting += Time.deltaTime;

        if(shooting < .4f)
        {
            animator.SetFloat("shooting", 1);
        }
        else
        {
            animator.SetFloat("shooting", 0);
        }
    }

    void Walk()
    {
        Dash();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0)
        {
            myRB.velocity = new Vector2(movementSpeed * movementMultiplier * h, myRB.velocity.y);
            lastDirection = (int)h;
        }
        else
        {
            animator.SetTrigger("stopped");
            myRB.velocity = new Vector2(0, myRB.velocity.y);
        }
        animator.SetFloat("xSpeed", Mathf.Abs(myRB.velocity.x));
        animator.SetInteger("ySpeed", (int)myRB.velocity.y);

        if(lastDirection == -1)
            transform.localScale = new Vector3(-5, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(5, transform.localScale.y, transform.localScale.z);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && myRB.velocity.y == 0 && !jumping)
        {
            myRB.AddForce(new Vector2(0, jumpHeight * 800));
            jumping = true;
        }
    }

    IEnumerator DropDown(GameObject platform)
    {
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Input.GetKeyDown(KeyCode.C))
        {
            Collider2D c2d = platform.GetComponent<Collider2D>();
            c2d.enabled = false;
            yield return new WaitForSeconds(.5f);
            c2d.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "DropGround") && jumping)
        {
            jumping = false;
            animator.SetTrigger("landed");  
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DropGround")
        {
            StartCoroutine(DropDown(collision.gameObject));
        }
    }
}

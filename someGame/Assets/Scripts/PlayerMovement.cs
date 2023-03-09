using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int defSpeed;
    public float Speed;
    public Transform groundCheck;
    public LayerMask layerMask;
    public bool isGrounded;
    public float jumpForce;
    public float gravity;
    public float dashForce;
    private Rigidbody rb;
    private bool canDoubleJump;
    public bool isDashing;
    public float dashSpeed;
    public int numOfDashes;
    private bool cooldown = false;
    PlayerMovement moveScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<PlayerMovement>();
        Speed = defSpeed;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.01f, layerMask);


        //Jump stuff, includes double jumps
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            
        }
        else
        {
            if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
            rb.AddForce(Vector3.down * gravity * Time.deltaTime);
        }

        //Everything below is code for Dashing
        if (!cooldown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(Dash());
                cooldown = true;
            }
        }
        else
        {
            Invoke("dashCD", 0.3f);
        }

            if (isGrounded && numOfDashes == 0)
            {
                Invoke("resetDashes", 0.5f);
            }
       
    }

    IEnumerator Dash()
    {
        isDashing = true;
        if (numOfDashes > 0)
        {
            if (isDashing && Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector3(0, 0, dashSpeed);
                gravity = 0;
                numOfDashes -= 1;
                yield return new WaitForSeconds(0.3f);
                gravity = 2000;
                rb.velocity = new Vector3(0, 0, 0);
                isDashing = false;
            }
            if (isDashing && Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector3(-dashSpeed, 0, 0);
                gravity = 0;
                numOfDashes -= 1;
                yield return new WaitForSeconds(0.3f);
                gravity = 2000;
                rb.velocity = new Vector3(0, 0, 0);
                isDashing = false;
            }
            if (isDashing && Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector3(0, 0, -dashSpeed);
                gravity = 0;
                numOfDashes -= 1;
                yield return new WaitForSeconds(0.3f);
                gravity = 2000;
                rb.velocity = new Vector3(0, 0, 0);
                isDashing = false;
            }
            if (isDashing && Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector3(dashSpeed, 0, 0);
                gravity = 0;
                numOfDashes -= 1;
                yield return new WaitForSeconds(0.3f);
                gravity = 2000;
                rb.velocity = new Vector3(0, 0, 0);
                isDashing = false;
            }
        }
    }

    void resetDashes()
    {
        numOfDashes = 2;
    }

    void dashCD()
    {
        cooldown = false;
    }

}

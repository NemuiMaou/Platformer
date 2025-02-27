using System;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float jumpSpeed = 20f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 8f;

    public GameManager gamemanager;
    

    [Header("Debug Stuff")] public bool isGrounded;

    Animator animator;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);

        float horizontalSpeed = rb.linearVelocity.x;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
        
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalSpeed;

        float verticalSpeed = rb.linearVelocity.y;
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, jumpSpeed);
        newVelocity.y = verticalSpeed;
        
        rb.linearVelocity = newVelocity;
        
        // clamp vertical velocity
        
        // test if character on ground

        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y + 0.01f;

        Color color = (isGrounded) ? Color.green : Color.red;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
            }
        }

        if (horizontalAmount == 0f)
        {
            Vector3 decayedVelocity = rb.linearVelocity;
            decayedVelocity.x *= 1f - Time.deltaTime * 12f;
            rb.linearVelocity = decayedVelocity;
        }
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
        
        UpdateAnimation();
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject myObject = other.gameObject;
        Vector3 startPos = myObject.transform.position;
        
        if (myObject.CompareTag("Brick"))
        {
            float cast = myObject.GetComponent<Collider>().bounds.extents.y + 0.1f;

            if (Physics.Raycast(startPos, Vector3.down, cast))
            {
                Destroy(myObject);
                gamemanager.ScoreIncrease(gamemanager.brickBreakScoreIncrease);
            }
        }

        if (myObject.CompareTag("QuestionBox"))
        {
            float cast = myObject.GetComponent<Collider>().bounds.extents.y + 0.1f;
            
            if (Physics.Raycast(startPos, Vector3.down, cast))
            {
                gamemanager.CoinIncrease();
                gamemanager.ScoreIncrease(gamemanager.coinScoreIncrease);
            }
        }

        if (myObject.CompareTag("FinishLine"))
        {
            gamemanager.winCheck();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject myObject = other.gameObject;
        if (myObject.CompareTag("Water"))
        {
            Destroy(rb.gameObject);
            Debug.Log("Game Over");
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("Jumping", !isGrounded);
    }
}

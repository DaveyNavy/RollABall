using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 10;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    [SerializeField] GameObject child;
    private int count;
    bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        onGround = false;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        if (speed < 15)
        {
            rb.AddForce(movement * speed);
        }
        // Forces "feet" to remain at bottom of the player ever as the player rotates
        child.transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z); ;
        child.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.z * -1.0f);
        
        // Respawn mechanic
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        } else if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        onGround = false;
    }

    void OnJump ()
    {
        if (onGround)
        {
            rb.AddForce(new Vector3(0, 300, 0), ForceMode.Force);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 14)
        {
            winTextObject.SetActive(true);
        }
    }
}
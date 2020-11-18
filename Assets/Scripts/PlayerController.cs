using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{

    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI lossText;
    public GameObject lavaPlane;
    public Transform pickupableHolder;
    private LavaController lavaController;

    private int count = 0;
    private int losses = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        lavaController = lavaPlane.GetComponent<LavaController>();
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
    }

    void Update()
    {
        if (losses >= 0)
        {
            lossText.text = "Losses: " + losses;
        }
        else
        {
            lossText.text = "Wins: " + losses * -1;
        }
        
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump()
    {
        if (!isJumping)
        {
            Vector3 jump = new Vector3(0.0f, 300.0f, 0.0f);
            rb.AddForce(jump);
            isJumping = true;
            Invoke("resetJump", 2);
        }
    }

    void SetCountText()
    {
        countText.text = "Amount: " + count.ToString();
        if (count >= pickupableHolder.childCount)
        {
            losses--;
        }
    }

    void FixedUpdate()
    {

        Vector3 movement = new Vector3(movementX, 0f, movementY);

        rb.AddForce(movement*speed);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickupable"))
        {
            Rotater rotater = other.gameObject.GetComponent<Rotater>();
            switch (rotater.powerupType)
            {
                case "speed":
                    resetEffects();
                    speed = 30;
                    break;
                case "slowLava":
                    resetEffects();
                    lavaController.speed = 0.0025f;
                    break;
                default:
                    resetEffects();
                    break;
            }
            effectsText.text = rotater.powerupType;
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Respawn"))
        {
            Invoke("resetGame", 2);

        }
    }

    void resetGame()
    {
        transform.position = new Vector3(0, 0.5f, 0);
        losses++;
        lavaController.reset();
    }

    void resetJump()
    {
        isJumping = false;
    }

    void resetEffects()
    {
        speed = 10;
        lavaController.speed = 0.005f;
    }
}

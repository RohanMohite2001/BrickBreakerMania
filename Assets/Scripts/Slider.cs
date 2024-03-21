using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Slider : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private float speed = 0f;
    private float maxBouncingAngle = 75f;
    public Vector3 startingPos;
    public Vector3 originalScale;
    private float powerUpCollectTime;
    [SerializeField] private GameObject block;
    [SerializeField] private float touchMoveThreshold = 10f;

    private bool isDragging = false;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Vector2 previousTouchPosition;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPos = transform.position;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        // {
        //     direction = Vector2.left;
        // }
        // else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        // {
        //     direction = Vector2.right;
        // }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                touchStartPos = touch.position;
                previousTouchPosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                if (isDragging)
                {
                    float touchDeltaX = touch.position.x - previousTouchPosition.x;

                    if (Mathf.Abs(touchDeltaX) > touchMoveThreshold)
                    {
                        direction = new Vector2(touchDeltaX, 0).normalized;
                        transform.Translate(direction * speed * Time.deltaTime);
                        //rb.AddForce(direction * speed);
                    }
                    else
                    {
                        direction = Vector2.zero;
                    }
                    previousTouchPosition = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                direction = Vector2.zero;
            }
        }
        else
        {
            direction = Vector2.zero;
        }
    }
                                                                                            
    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.tap);
            
            float contactPoint = transform.position.x - other.GetContact(0).point.x;
            float centre = gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, other.gameObject.GetComponent<Rigidbody2D>().velocity);
            float bounceAngle = (contactPoint / centre) * maxBouncingAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBouncingAngle, maxBouncingAngle);
            
            Quaternion afterBounceAngle = Quaternion.AngleAxis(newAngle, Vector3.forward);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = afterBounceAngle * Vector2.up *
                                                                   other.gameObject.GetComponent<Rigidbody2D>().velocity
                                                                       .magnitude;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ScalePowerUp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            Destroy(other.gameObject);
            transform.localScale += new Vector3(.2f, 0, 0);
            StartCoroutine(PowerUpScaleTimer(5f));
        }

        if (other.gameObject.CompareTag("BlockPowerUp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            Destroy(other.gameObject);
            block.gameObject.SetActive(true);
            StartCoroutine(PowerUpBlockTimer(5));
        }

        if (other.gameObject.CompareTag("Star"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            Destroy(other.gameObject);
            GameManager.Instance.stars += 1;
        }
    }

    private IEnumerator PowerUpScaleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale = originalScale;
        Debug.Log("Size resets");
    }
    
    private IEnumerator PowerUpBlockTimer(float time)
    {
        yield return new WaitForSeconds(time);
        block.gameObject.SetActive(false);
    }
}

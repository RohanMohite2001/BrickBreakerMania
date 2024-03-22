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
    [SerializeField] private float touchMoveThreshold = .2f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Vector2 previousTouchPosition;
    private Vector3 prevWorldPos;
    private float maxPos;
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject ballPos;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float bounds = (float)Screen.width / (float)Screen.height * Camera.main.orthographicSize * 2f;
        startingPos = transform.position;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        Vector3 newPos = transform.position;

        float screenWidth = Screen.width;
        float objectWidth = transform.localScale.x * 0.5f; 

        float maxX = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, 0, 0)).x - transform.localScale.x;
        float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + transform.localScale.x;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        transform.position = newPos;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPositionWorld = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPosition = touch.position;
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.position - previousTouchPosition;
                Vector3 touchDeltaWorld = Camera.main.ScreenToWorldPoint(new Vector3(touchDelta.x, transform.position.y, transform.position.z)) - Camera.main.ScreenToWorldPoint(Vector3.zero);
                transform.position += new Vector3(touchDeltaWorld.x * speed, 0, 0);
                previousTouchPosition = touch.position;
            }
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
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            transform.localScale += new Vector3(.2f, 0, 0);
            StartCoroutine(PowerUpScaleTimer(5f));
        }

        if (other.gameObject.CompareTag("BlockPowerUp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            block.gameObject.SetActive(true);
            StartCoroutine(PowerUpBlockTimer(5));
        }

        if (other.gameObject.CompareTag("Star"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            GameManager.Instance.stars += 1;
            GameManager.Instance.CheckStars();
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

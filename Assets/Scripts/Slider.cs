using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Slider : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    private float maxBouncingAngle = 75f;
    public Vector3 startingPos;
    public Vector3 originalScale;
    [SerializeField] private GameObject block;

    private Vector2 previousTouchPosition;
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject ballPos;
    [SerializeField] private GameObject ballPref;
    private Coroutine scalePowerUpCoroutine;
    private Coroutine blockPowerUpCoroutine;

    
    private void Start()
    {
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
            if (GameManager.Instance.inPlay)
            {
                transform.localScale += new Vector3(.2f, 0, 0);
            }

            if (scalePowerUpCoroutine != null)
            {
                StopCoroutine(scalePowerUpCoroutine);
            }
            
            scalePowerUpCoroutine = StartCoroutine(PowerUpScaleTimer(5f));
        }

        if (other.gameObject.CompareTag("BlockPowerUp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            if (GameManager.Instance.inPlay)
            {
                block.gameObject.SetActive(true);

                if (blockPowerUpCoroutine != null)
                {
                    StopCoroutine(blockPowerUpCoroutine);
                }
                
                blockPowerUpCoroutine = StartCoroutine(PowerUpBlockTimer(5));
            }
        }

        if (other.gameObject.CompareTag("Star"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            GameManager.Instance.stars += 1;
            GameManager.Instance.CheckStars();
        }
        
        if (other.gameObject.CompareTag("MultiplyPowerUp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.powerUpSfx);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            Instantiate(ballPref, ball.transform.position, ball.transform.rotation);
            Instantiate(ballPref, ball.transform.position, ball.transform.rotation);
        }
    }

    private IEnumerator PowerUpScaleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale = originalScale;
        scalePowerUpCoroutine = null;
    }
    
    private IEnumerator PowerUpBlockTimer(float time)
    {
        yield return new WaitForSeconds(time);
        block.gameObject.SetActive(false);
        blockPowerUpCoroutine = null;
    }
}

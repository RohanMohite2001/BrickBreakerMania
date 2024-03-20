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
    [SerializeField] private float speed = 30f;
    private float maxBouncingAngle = 75f;
    public Vector3 startingPos;
    public Vector3 originalScale;
    private float powerUpCollectTime;
    [SerializeField] private GameObject block;
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
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
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
            rb.AddForce(direction * speed);
            //transform.Translate(direction * speed * Time.deltaTime);
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

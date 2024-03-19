using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private float speed = 400f;
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private int probabilityTime = 10;
    [SerializeField] private ParticleSystem brickExplosion; 
    public Slider slider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPos = transform.position;
        StartLevel();
    }

    public void StartLevel()
    {
        transform.position = startingPos;
        speed = 300f;
        slider.transform.position = slider.startingPos;
        slider.transform.localScale = slider.originalScale;
        force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        
        rb.AddForce(force.normalized * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            int probability = Random.Range(1, 100);
            if (probability < probabilityTime)
            {
                Instantiate(powerUp, other.transform.position, other.transform.rotation);
            }
            //Instantiate(explosionPrefab, other.transform.position, other.transform.rotation);
            brickExplosion.transform.position = other.transform.position;
            brickExplosion.gameObject.SetActive(true);
            brickExplosion.Play();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.brickDestroySFX);
            other.gameObject.SetActive(false);
            GameManager.Instance.ShowScore(1);
        }
        
        if (other.gameObject.CompareTag("MissZone"))
        {
            GameManager.Instance.ShowLives(-1);
            if (GameManager.Instance.lives > 0)
            {
                Invoke("StartLevel", .2f);
            }else if (GameManager.Instance.lives <= 0)
            {
                Debug.Log("GameOver");
                gameObject.SetActive(false);
            }
        }
    }
}

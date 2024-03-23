using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour
{
    [SerializeField] private int scaleProbabilityTime = 10;
    [SerializeField] private int blockProbabilityTime = 80;
    [SerializeField] private GameObject brickExplosion;

    private void Start()
    {
        brickExplosion = FindObjectOfType<Explosion>().gameObject;
        brickExplosion.GetComponent<ParticleSystem>().Stop();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.brickCount++;
            GameManager.Instance.CheckBrickCount();
            
            //power-up
            int probability = Random.Range(1, 100);
            if (probability < scaleProbabilityTime)
            {
                //Instantiate(powerUpScale, transform.position, transform.rotation);
                GameObject powerUpScale = ObjectPool.Instance.GetPooledObject(0);
                if (powerUpScale != null)
                {
                    powerUpScale.transform.position = transform.position;
                    powerUpScale.SetActive(true);
                }
            }
            if (probability > blockProbabilityTime)
            {
                //Instantiate(powerUpBlock, transform.position, transform.rotation);
                GameObject powerUpBlock = ObjectPool.Instance.GetPooledObject(1);
                if (powerUpBlock != null)
                {
                    powerUpBlock.transform.position = transform.position;
                    powerUpBlock.SetActive(true);
                }
            }
            if (probability < 40 && probability > 10 && GameManager.Instance.stars < 3)
            {
                //Instantiate(star, transform.position, transform.rotation);
                GameObject star = ObjectPool.Instance.GetPooledObject(2);
                if (star != null)
                {
                    star.transform.position = transform.position;
                    star.SetActive(true);
                }
            }
            if (probability < 85 && probability > 75)
            {
                //Instantiate(star, transform.position, transform.rotation);
                GameObject multiply = ObjectPool.Instance.GetPooledObject(3);
                if (multiply != null)
                {
                    multiply.transform.position = transform.position;
                    multiply.SetActive(true);
                }
            }
            
            //explosion effect
            brickExplosion.transform.position = transform.position;
            brickExplosion.gameObject.SetActive(true);
            brickExplosion.GetComponent<ParticleSystem>().Play();
            
            //Brick sfx
            AudioManager.Instance.PlaySfx(AudioManager.Instance.tap);
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private void Update()
    {
        transform.Translate(new Vector2(0f, -1f) * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MissZone"))
        {
            gameObject.SetActive(false);
        }
    }
}

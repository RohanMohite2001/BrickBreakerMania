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
}

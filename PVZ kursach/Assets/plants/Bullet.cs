using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 direction;
    private void Start()
    {
        rb.AddForce(direction * speed);
    }
}

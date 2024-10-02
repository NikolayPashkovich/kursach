using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 direction;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider;
    private void Start()
    {
        rb.AddForce(direction * speed);
    }
    public int GetDamage()
    {
        return damage;
    }
    public void Hit()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        collider.enabled = false;
        animator.SetTrigger("hit");
    }
    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}

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
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] AudioSource hitAudio;
    private void Start()
    {
        rb.AddForce(direction * speed);
    }
    public int GetDamage()
    {
        return damage;
    }
    public virtual void Hit(Zombie zombie)
    {
        zombie.Damage(damage);
        Hit();
    }
    public void Hit()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        collider.enabled = false;
        animator.SetTrigger("hit");
        hitAudio.Play();
    }
    public void DestroyObj()
    {
        StartCoroutine(DestroyCorutine());
    }
    IEnumerator DestroyCorutine()
    {
        float timer = 0;
        Color normalColor = spriteRenderer.color;
        Color colorToDestroy = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        while(timer < 1)
        {
            timer += Time.deltaTime * 2;
            spriteRenderer.color = Color.Lerp(normalColor, colorToDestroy, timer);
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            Hit(zombie);
        }

    }
}

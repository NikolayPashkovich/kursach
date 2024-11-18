using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDirection;
    Plant targetPlant;
    public virtual void Awake()
    {
        base.Awake();
    }
    public void StartMove()
    {
        moveDirection = new Vector2(-1, 0);
        animator.SetInteger("Action", 1);
    }
    private void FixedUpdate()
    {
        if (targetPlant == null)
        {
            rb.MovePosition((Vector2)transform.position + (moveDirection * speed));
            if (animator.GetInteger("Action") == 2)
            {
                animator.SetInteger("Action", 1);
            }
        }
        posInGrid = GridManager.Instance.GetGridCellFromPosition(transform.position);
        SelectLayer();
    }
    public override void Damage(int damage)
    {
        hp -= damage;
        StartCoroutine(glareCorutine());
        if (hp < maxHP / 2)
        {
            animator.SetBool("isHaveHand", false);
        }
        if (hp <= 0)
        {
            animator.SetBool("isHaveHead", false);
            Dead();
        }

    }
    public virtual void FireDeath()
    {
        speed = 0;
        //отключаю все слои аниматора кроме 0-го чтобы все возможные отваливающиеся части зомби не вылетали при анимации сгорания
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetBool("isHaveHand", false);
        animator.SetBool("isHaveHead", false);
        animator.SetTrigger("fireDeath");
    }
    public void Eat()
    {
        if (targetPlant == null) { return; }
        targetPlant.Damage(damage);
    }
   
    void Dead()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        collider.enabled = false;
        moveDirection = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Damage(bullet.GetDamage());
            bullet.Hit();
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (targetPlant == null && collision.gameObject.tag == "Plant")
        {
            Plant plant = collision.gameObject.GetComponent<Plant>();
            animator.SetInteger("Action", 2);
            targetPlant = plant;
        }
    }
}


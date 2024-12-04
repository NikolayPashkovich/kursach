using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    // Start is called before the first frame update
    [SerializeField] protected float speed;
    [SerializeField] int damage;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDirection;
    protected Plant targetPlant;
    [SerializeField] SpriteRenderer[] additionalRenderers;

    [SerializeField] Color frorzenColor;
    protected bool isFrozen = false;
    private float freezeTimer = 0f;
    protected const float froozeSpeedCoef = 0.6f;
    private Coroutine freezeCoroutine = null;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] AudioClip[] brainsClips;
    [SerializeField] AudioClip[] eatSounds;
    public override Color NormalColor()
    {
        if (isFrozen) { return frorzenColor; }
        return base.NormalColor();
    }
    protected override void SelectLayer()
    {
        base.SelectLayer();
        for (int i = 0; i < additionalRenderers.Length; i++)
        {
            additionalRenderers[i].sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }
    public virtual float Speed()
    {
        if (isFrozen)
        {
            return speed * froozeSpeedCoef;
        }
        return speed;
    }
    public virtual void Awake()
    {
        base.Awake();
    }
    public void StartMove()
    {
        moveDirection = new Vector2(-1, 0);
        animator.SetInteger("Action", 1);
        audioSource.clip = brainsClips[Random.Range(0, brainsClips.Length)];
        audioSource.Play();
    }
    public void SetFreeze(float time)
    {
        freezeTimer = time; // Обновляем таймер заморозки

        if (!isFrozen && freezeTimer > 0)
        {
            isFrozen = true;
            freezeCoroutine = StartCoroutine(HandleFreeze());
        }
    }

    private IEnumerator HandleFreeze()
    {
        spriteRenderer.color = frorzenColor; 
        while (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            yield return null; // Ждём следующий кадр
        }
        isFrozen = false;
        spriteRenderer.color = NormalColor(); // Возвращаем цвет
        freezeCoroutine = null;
    }
    protected virtual void FixedUpdate()
    {
        animator.SetFloat("speedCoef", isFrozen ? froozeSpeedCoef : 1);
        if (targetPlant == null)
        {
            rb.MovePosition((Vector2)transform.position + (moveDirection * Speed()));
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
        audioSource.clip = eatSounds[Random.Range(0,eatSounds.Length)];
        audioSource.Play();
    }
   
    void Dead()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        collider.enabled = false;
        moveDirection = Vector2.zero;
    }
    
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (targetPlant == null && collision.gameObject.tag == "Plant")
        {
            Plant plant = collision.gameObject.GetComponent<Plant>();
            animator.SetInteger("Action", 2);
            targetPlant = plant;
        }
    }
}


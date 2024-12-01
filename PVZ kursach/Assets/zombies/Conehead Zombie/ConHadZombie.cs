using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConHadZombie : Zombie
{
    [SerializeField] int maxConHealth;
    int conHealth;
    [SerializeField] Sprite[] conSprites;
    [SerializeField] SpriteRenderer conRenderer;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        conHealth = maxConHealth;
    }
    protected override void SelectLayer()
    {
        base.SelectLayer();
        conRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
    }
    public override void FireDeath()
    {
        conRenderer.enabled = false;
        base.FireDeath();
    }
    public override void Damage(int damage)
    {
        if (conHealth > 0)
        {
            conHealth -= damage;
            conRenderer.sprite = conSprites[Mathf.Max(0,  Mathf.Min(conSprites.Length, (int)(conSprites.Length * ((float)conHealth / maxConHealth))))];
            if (conHealth < 0)
            {
                Damage(-damage);
                return;
            }
            StartCoroutine(glareCorutine());
        }
        else
        {
            if (animator.GetBool("isHaveCon"))
            {
                animator.SetBool("isHaveCon", false);
            }
            base.Damage(damage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

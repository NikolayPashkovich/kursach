using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : Plant
{
    bool isActive = false;
    [SerializeField] int boomDamage;
    public override void Start()
    {
        base.Start();
        animator.SetBool("isActive", isActive);
    }
    protected override void Action()
    {
        StopAllCoroutines();
        isActive = true;
        animator.SetBool("isActive", isActive);
        hp = 9999;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.tag == "Zombie")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                Boom(zombie);
            }    
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
    public void Boom(Zombie zombie)
    {
        animator.SetTrigger("Boom");
        zombie.Damage(boomDamage);
        audio.clip = actionAudio;
        audio.Play();
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            if (GridManager.Instance.zombies[i] != zombie && GridManager.Instance.zombies[i].posInGrid == posInGrid)
            {
                GridManager.Instance.zombies[i].Damage(boomDamage);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleVaultingZombie : Zombie
{
    bool isRun = true;
    [SerializeField] float runSpeedCoef = 2;
    [SerializeField] float XWhereActivateCollider = 100;
    public override float Speed()
    {
        float resSpeed = speed;
        resSpeed *= isFrozen ? froozeSpeedCoef : 1;
        resSpeed *= isRun ? runSpeedCoef : 1;
        return resSpeed;
    }
    
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (isRun)
        {
            if (collision.gameObject.tag == "Plant")
            {
                animator.SetBool("Jump",true);
                collider.enabled = false;
                XWhereActivateCollider = transform.position.x - GridManager.Instance.cellSize.x * 1.5f;
                
                StartCoroutine(JumpProcess());
            }
        }
        else
        {
            base.OnTriggerStay2D(collision);
        }
    }
    public void InAirAnim()
    {
        runSpeedCoef = 5;
    }
    IEnumerator JumpProcess()
    {
        while (transform.position.x > XWhereActivateCollider)
        {
            yield return null;
        }
        isRun = false;
        animator.SetBool("Jump", false);
        collider.enabled = true;
        
    }

}

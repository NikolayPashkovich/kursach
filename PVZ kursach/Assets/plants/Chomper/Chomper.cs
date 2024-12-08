using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Plant
{
    Zombie targetZombie;
    bool isActive = true;
    public override void Start()
    {
    }
    public bool isCanSetTarget()
    {
        return isActive && (targetZombie == null);
    }
    public void SetTarget(Zombie zombie)
    {
        if (isActive == false || targetZombie != null) { return; }
        targetZombie = zombie;
        animator.SetTrigger("Atack");
        audio.clip = actionAudio;
        audio.Play();
    }
    public void KillZombie()
    {
        if (targetZombie == null) { return; }
        Destroy(targetZombie.gameObject);
        isActive = false;
        animator.SetBool("isActive", false);
        StartCoroutine(waitForAction());
    }
    protected override IEnumerator waitForAction()
    {
        yield return new WaitForSeconds(timeToAction);
        Action();
    }
    protected override void Action()
    {
        isActive = true;
        animator.SetBool("isActive", true);
    }
}

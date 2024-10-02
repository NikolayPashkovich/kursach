using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goroch : Plant
{
    [SerializeField] Bullet bullet;
    protected override void Action()
    {
        
        if (isHaveZombieInLineFromFront())
        {
            Instantiate(bullet.gameObject, transform.position + posToInstance, transform.rotation);
            animator.SetTrigger("Atack");
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

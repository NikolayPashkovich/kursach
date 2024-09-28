using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goroch : Plant
{
    [SerializeField] Bullet bullet;
    [SerializeField] Vector3 posToInstance;
    protected override void Action()
    {
        Instantiate(bullet.gameObject, transform.position + posToInstance, transform.rotation);
        animator.SetTrigger("Atack");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    // Start is called before the first frame update
    public override void Damage(int damage)
    {
        base.Damage(damage);
        animator.SetInteger("HP", Mathf.FloorToInt(hp / (maxHP/3f)));
    }
}

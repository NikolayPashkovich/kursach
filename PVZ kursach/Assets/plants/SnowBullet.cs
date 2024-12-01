using System;
using UnityEngine;

class SnowBullet : Bullet
{
    [SerializeField] float freezeTime;
    public override void Hit(Zombie zombie)
    {
        zombie.SetFreeze(freezeTime);
        base.Hit(zombie);
    }
}

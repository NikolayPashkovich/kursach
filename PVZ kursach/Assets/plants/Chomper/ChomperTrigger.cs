using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperTrigger : MonoBehaviour
{
    [SerializeField] Chomper chomper;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Zombie" && chomper.isCanSetTarget())
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            chomper.SetTarget(zombie);
        }
    }
}

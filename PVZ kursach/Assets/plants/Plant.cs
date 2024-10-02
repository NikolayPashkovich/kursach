using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Entity
{
    // Start is called before the first frame update
    [SerializeField] float timeToAction;
    private void Awake()
    {
        hp = maxHP;
        GridManager.Instance.plants.Add(this);
    }
    protected bool isHaveZombieInLine()
    {
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            if (GridManager.Instance.zombies[i].posInGrid.x == posInGrid.x)
            {
                return true;
            }
        }
        return false;
    }
    void Start()
    {
        StartCoroutine(waitForAction());
    }
    IEnumerator waitForAction()
    {
        yield return new WaitForSeconds(timeToAction);
        Action();
        StartCoroutine(waitForAction());
    }
    public void Damage(int damage)
    {
        hp -= damage;
        StartCoroutine(glareCorutine());
        if (hp <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    protected virtual void Action()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

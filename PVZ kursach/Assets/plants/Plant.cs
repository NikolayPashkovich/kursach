using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Entity
{
    // Start is called before the first frame update
    [SerializeField] protected float timeToAction;
    [SerializeField] protected Vector3 posToInstance;
    [SerializeField] int sunsCost;
    [SerializeField] float timeToRecharge;
    public float GetTimeToRecharge() { return timeToRecharge; }
    public int GetCost() { return sunsCost; }

    [SerializeField] protected AudioSource audio;
    [SerializeField] protected AudioClip actionAudio;
    
    public virtual void Start()
    {
        StartCoroutine(waitForAction());
    }
    protected bool isHaveZombieInLine()
    {
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            if (GridManager.Instance.zombies[i].posInGrid.y == posInGrid.y)
            {
                return true;
            }
        }
        return false;
    }
    protected bool isHaveZombieInLineFromFront()
    {
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            if (GridManager.Instance.zombies[i].posInGrid.y == posInGrid.y && GridManager.Instance.zombies[i].posInGrid.x >= posInGrid.x)
            {
                return true;
            }
        }
        return false;
    }
    protected virtual IEnumerator waitForAction()
    {
        yield return new WaitForSeconds(timeToAction);
        Action();
        StartCoroutine(waitForAction());
    }
    protected virtual void Action()
    {

    }
}

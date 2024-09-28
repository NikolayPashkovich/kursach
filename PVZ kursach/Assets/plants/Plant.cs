using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer spriteRenderer;
    public Vector2Int posInGrid;
    [SerializeField] protected Animator animator;
    [SerializeField] float timeToAction;
    [SerializeField] int HP;

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
    protected virtual void Action()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

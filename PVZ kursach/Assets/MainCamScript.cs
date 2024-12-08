using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamScript : MonoBehaviour
{
    [SerializeField] Vector3 selectCamPos;
    [SerializeField] Vector3 gameCamPos;
    [SerializeField] float speedCoef;
    [SerializeField] float waitTime;
    Camera cam;
    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }
    public IEnumerator GoToGame()
    {
        StopAllCoroutines();
        yield return StartCoroutine(GoToPoint(gameCamPos,false));
    }
    public void GoToSelect()
    {
        StopAllCoroutines();
        StartCoroutine(GoToPoint(selectCamPos,true));
    }
    IEnumerator GoToPoint(Vector3 point,bool toSelect)
    {
        if (toSelect) { yield return new WaitForSeconds(waitTime); }
        while (Vector3.Distance(transform.position,point) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, point, speedCoef);
            yield return null;
        }
    }
}

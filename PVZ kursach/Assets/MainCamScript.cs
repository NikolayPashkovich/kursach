using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamScript : MonoBehaviour
{
    [SerializeField] Vector3 selectCamPos;
    [SerializeField] Vector3 gameCamPos;
    [SerializeField] float speedCoef;
    Camera cam;
    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }
    public void GoToGame()
    {
        StopAllCoroutines();
        StartCoroutine(GoToPoint(gameCamPos));
    }
    public void GoToSelect()
    {
        StopAllCoroutines();
        StartCoroutine(GoToPoint(selectCamPos));
    }
    IEnumerator GoToPoint(Vector3 point)
    {
        while (Vector3.Distance(transform.position,point) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, point, speedCoef);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetPos;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color normalColor;
    [SerializeField] Color hideColor;
    [SerializeField] float speed;
    [SerializeField] float alphaAnimTime;
    [SerializeField] float liveTime;
    [SerializeField] Collider2D collider;
    bool isLive = true;
    const int SunsQunatity = 25;
    void Start()
    {
        StartCoroutine(CahngeAlpha(false));
    }
    IEnumerator CahngeAlpha(bool hide)
    {
        float timer = 0;
        while (timer < alphaAnimTime)
        {
            timer += Time.deltaTime;
            float t;
            if (hide) { t = timer / alphaAnimTime; }
            else { t = 1- (timer / alphaAnimTime); }
            spriteRenderer.color = Color.Lerp(normalColor, hideColor, t);
            yield return null;
        }
        if (hide) { spriteRenderer.color = hideColor; }
        else { spriteRenderer.color = normalColor; }
    }
    IEnumerator DestroyCorutine()
    {
        yield return StartCoroutine(CahngeAlpha(true));
        Destroy(gameObject);
    }
    IEnumerator MoveToStorageOfSuns()
    {
        //targetPos = Camera.main.ScreenToWorldPoint(EconomicController.instance.GetSunsTextPos());
        targetPos = Camera.main.ScreenToWorldPoint(new Vector3(0,Camera.main.pixelHeight));
        Vector3 startPos = transform.position;
        float timer = 0;
        float timeLength = alphaAnimTime * 4;
        while(timer < timeLength)
        {
            timer += Time.deltaTime;
            float t = Mathf.Pow(timer / timeLength,2);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            spriteRenderer.color = Color.Lerp(normalColor, hideColor, t);
            yield return null;
        }
        EconomicController.instance.AddSuns(SunsQunatity);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLive) { return; }
        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
        }
        liveTime -= Time.deltaTime;
        if (liveTime <= 0)
        {
            isLive = false;
            StartCoroutine(DestroyCorutine());
        }
    }
    private void OnMouseDown()
    {
        isLive = false;
        collider.enabled = false;
        StartCoroutine(MoveToStorageOfSuns());
    }
}

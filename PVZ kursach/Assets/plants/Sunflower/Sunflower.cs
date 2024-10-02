using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{
    [SerializeField] SpriteRenderer lightRenderer;
    [SerializeField] float timeToChangeLight;

    [SerializeField] Color withoutLightColor;
    [SerializeField] Color withLightColor;
    [SerializeField] Sun sunPrefab;
    protected override void Action()
    {
        StartCoroutine(CreateSun());
    }
    private IEnumerator CreateSun()
    {
        lightRenderer.sortingOrder = spriteRenderer.sortingOrder;
        float timer = 0;
        while(timer < timeToChangeLight)
        {
            timer += Time.deltaTime;
            float t = timer / timeToChangeLight;
            lightRenderer.color = Color.Lerp(withoutLightColor, withLightColor, t);
            yield return null;
        }
        SpawnSun();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float t = timer / timeToChangeLight;
            lightRenderer.color = Color.Lerp(withoutLightColor, withLightColor, t);
            yield return null;
        }
        lightRenderer.color = withoutLightColor;
    }
    void SpawnSun()
    {
        sunPrefab.targetPos = transform.position + new Vector3(Random.Range(-0.25f, 0.25f), 0, -5);
        Instantiate(sunPrefab.gameObject, transform.position + posToInstance, Quaternion.Euler(0, 0, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomicController : MonoBehaviour
{
    // Start is called before the first frame update
    public static EconomicController instance { get; private set; }
    int Suns;
    [SerializeField] Text sunsQuantityText;
    Coroutine changeTextCorutine;

    [SerializeField] float timeToSpawnSun;
    [SerializeField] Sun sunPrefab;
    float sunsTimer =0;
    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); }
        else
        {
            instance = this;
        }
        UpdateUI();
    }
    
    public void AddSuns(int plusSuns)
    {
        Suns += plusSuns;
        UpdateUI();
    }
    public void RemoveSuns(int minusSuns)
    {
        Suns -= minusSuns;
        UpdateUI();
    }
    void UpdateUI()
    {
        if (changeTextCorutine == null) { changeTextCorutine = StartCoroutine(ChangeText()); }
    }
    IEnumerator ChangeText()
    {
        int textSuns = int.Parse(sunsQuantityText.text);
        while(textSuns != Suns)
        {
            if (textSuns < Suns) { textSuns++; }
            if (textSuns > Suns) { textSuns--; }
            sunsQuantityText.text = textSuns.ToString();
            yield return null;
        }
        changeTextCorutine = null;
    }
    private void FixedUpdate()
    {
        sunsTimer += Time.fixedDeltaTime;
        if (sunsTimer >= timeToSpawnSun)
        {
            sunsTimer = 0;
            SpawnSun();
        }
    }
    void SpawnSun()
    {
        Vector3 target = GridManager.Instance.GetPositionFromGridCell(new Vector2Int(Random.Range(0, GridManager.Instance.cols), Random.Range(0, GridManager.Instance.rows)));
        target.z = -5;
        target.x += Random.Range(-GridManager.Instance.cellSize.x/2, GridManager.Instance.cellSize.x/2);
        sunPrefab.targetPos = target;
        Instantiate(sunPrefab.gameObject, new Vector3(target.x,5, target.z), Quaternion.Euler(0, 0, 0));
    }
}

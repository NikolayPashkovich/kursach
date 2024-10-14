using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class SelectButton : MonoBehaviour
{
    public UIController uIController;
    public Plant plant;
    Vector3 startPos;
    public RectTransform rectTransform { get; private set; }
    Button button;
    bool isSelected;
    const float moveTime = 0.5f;
    Image image;
    private List<UnityAction> savedActions = new List<UnityAction>();
    private List<Object> savedTargets = new List<Object>();
    private List<string> savedMethodNames = new List<string>();

    bool isGameStarted = false;
    float rechargeTimer = 0;

    float timeToRecharge;
    [SerializeField] Slider rechargeBar;
    public void Awake()
    {
        timeToRecharge = plant.GetTimeToRecharge();
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
        startPos = rectTransform.anchoredPosition;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SelectClick);
    }
    public void Select()
    {
        button.Select();
    }
    public void GoToGame()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(TrySelectPlant);
        isGameStarted = true;
        rechargeTimer = timeToRecharge;
    }
    public void ResetTimer()
    {
        rechargeTimer = timeToRecharge;
        
    }
    void TrySelectPlant()
    {
        EconomicController.instance.TrySelectPlant(this);
    }
    public void Update()
    {
        if (!isGameStarted) { return; }
        if (rechargeTimer > 0)
        {
            rechargeTimer -= Time.deltaTime;
            rechargeBar.value = Mathf.Clamp(rechargeTimer / timeToRecharge, 0, 1);
        }
        button.interactable = rechargeTimer <= 0 && EconomicController.instance.Suns >= plant.GetCost();
    }
    public void SelectClick()
    {
        if (isSelected)
        {
            isSelected = false;
            uIController.RemoveSelectButton(this);
            rectTransform.SetParent(uIController.selectButtonsContent, true);
            StartCoroutine(MoveCorutine(rectTransform.anchoredPosition, startPos));
        }
        else
        {
            if (!uIController.isCanSelectButton()) { return; }
            isSelected = true;
            uIController.AddSelectButton(this);
        }
    }
    public void MoveToSelected(Transform parent)
    {
        rectTransform.SetParent(parent, true);
        StartCoroutine(MoveCorutine(rectTransform.anchoredPosition, new Vector2(0, UIController.selectButtonShift)));
    }
    private IEnumerator MoveCorutine(Vector2 start, Vector2 target)
    {
        //Debug.Log($"start = {start}, end = {target}");
        button.enabled = false;
        image.maskable = false;
        float timer = 0;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Pow(timer / moveTime, 2);
            rectTransform.anchoredPosition = Vector2.Lerp(start, target, t); // Используем anchoredPosition для UI
            yield return null;
        }

        rectTransform.anchoredPosition = target; // Устанавливаем финальную позицию
        button.enabled = true;
        image.maskable = true;
    }
}

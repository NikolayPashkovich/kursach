using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] Text sunsQuantityText;
    Coroutine changeTextCorutine;
    [SerializeField] float selectedWait;
    [SerializeField] float selectedDragTime;
    [SerializeField] RectTransform selectedTransform;
    public List<SelectButton> selectButtons;
    [SerializeField] SelectButton[] allSelectButtons;
    public Transform selectButtonsContent;
    public const int selectButtonShift = -5;

    [SerializeField] Slider ProgressBarSlider;
    [SerializeField] RectTransform[] flagImages;

    [SerializeField] Image endPlantImage;
    [SerializeField] float endImgMaxSize;

    [SerializeField] CanvasGroup losePanelGroup;
    [SerializeField] AudioSource buttonClickSound;
    private void Awake()
    {
        
        selectButtons = new List<SelectButton>();
    }
    private void Start()
    {
        LoadSelectButtonsFromFile();
    }
    public void ButtonClickSound()
    {
        buttonClickSound.Play();
    }
    public void PutUpFlags(int[] points)
    {
        // 1. ¬ычисл€ем среднее значение
        int pointsLength = points.Length;
        float sum = 0;
        foreach (int point in points)
        {
            sum += point;
        }
        float average = sum / points.Length;

        // 2. —оставл€ем список индексов с наибольшими отклонени€ми от среднего значени€
        List<int> indexes = new List<int>();
        List<float> deviations = new List<float>();

        // 3. ¬ычисл€ем отклонение каждого элемента от среднего
        for (int i = 0; i < points.Length; i++)
        {
            float deviation = Mathf.Abs(points[i] - average);
            deviations.Add(deviation);
        }

        // 4. Ќаходим 1-3 индекса с наибольшими отклонени€ми
        var maxDeviations = deviations
            .Select((deviation, index) => new { deviation, index })
            .OrderByDescending(x => x.deviation)
            .Take(3)
            .ToList();

        // 5. ¬ыводим индексы с наибольшими отклонени€ми
        foreach (var item in maxDeviations)
        {
            indexes.Add(item.index);
        }

        for (int i = 0; i < indexes.Count; i++)
        {
            if (indexes[i] < average) { continue; }
            flagImages[i].gameObject.SetActive(true);
            flagImages[i].pivot = new Vector2(1- (float)indexes[i] / pointsLength, 0.25f);
        }

    }
    public IEnumerator YouLooseRoutine()
    {
        float time = 1;
        float timer = 0;
        losePanelGroup.gameObject.SetActive(true);
        while (timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            losePanelGroup.alpha = t;
            yield return null;
        }
    }
    public IEnumerator endImageRoutine(int plantIndex)
    {
        endPlantImage.sprite = allSelectButtons[plantIndex].button.image.sprite;
        float time = 1;
        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            endPlantImage.rectTransform.sizeDelta = new Vector2(endImgMaxSize * t, endImgMaxSize * t);
            yield return null;
        }
    }
    public static void DeselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    
    public void GoToGame()
    {
        StartCoroutine( ActivateSelect(false));
        for (int i = 0; i < selectButtons.Count; i++)
        {
            selectButtons[i].GoToGame();
        }
    }
    public void UpdateProgressBar(float value)
    {
        ProgressBarSlider.value = value;
    }
    void LoadSelectButtonsFromFile()
    {
        Debug.Log("LoadFromFile");
        int[] plants = FileManager.LoadOpenPlants();
        Debug.Log($"plants count = {plants.Length}");
        Debug.Log($"allSelectButtons = {allSelectButtons.Length}");
        for (int i = 0; i < allSelectButtons.Length; i++)
        {
            allSelectButtons[i].button.interactable = false;
        }
        for (int i = 0; i < plants.Length; i++)
        {
            allSelectButtons[plants[i]].button.interactable = true;
        }
    }
    public void SetActiveSelect(bool isActive)
    {
        StartCoroutine(ActivateSelect(isActive));
    
    }
    public bool isCanSelectButton()
    {
        return selectButtons.Count < EconomicController.instance.maxPlants; 
    }
    public void AddSelectButton(SelectButton selectButton)
    {
        selectButton.MoveToSelected(GetParentForSelectButton());
        selectButtons.Add(selectButton);
    }
    public void RemoveSelectButton(SelectButton selectButton)
    {
        MoveChildrensToSelectedButton(selectButton);
        selectButtons.Remove(selectButton);
    }
    public RectTransform GetParentForSelectButton()
    {
        if (selectButtons.Count == 0)
        {
            return sunsQuantityText.rectTransform;
        }
        else
        {
            return selectButtons[selectButtons.Count - 1].rectTransform;
        }
    }
    public void MoveChildrensToSelectedButton(SelectButton selectButton)
    {
        if (!selectButtons.Contains(selectButton)) { return; }
        int index = selectButtons.IndexOf(selectButton);
        if (index <selectButtons.Count-1)
        {
            if (index > 0)
            {
                selectButtons[index + 1].MoveToSelected(selectButtons[index - 1].rectTransform);
            }
            if (index == 0)
            {
                selectButtons[index + 1].MoveToSelected(sunsQuantityText.rectTransform);
            }
        }
        
    }
    IEnumerator ActivateSelect(bool isActive)
    {
        if (isActive)
        {
            yield return new WaitForSeconds(selectedWait);
            selectedTransform.gameObject.SetActive(true);
        }
        
        float timer = 0;
        while(timer < selectedDragTime)
        {
            timer += Time.deltaTime;
            float t;
            if (!isActive)
            {
                t = Mathf.Pow(timer / selectedDragTime, 2);
            }
            else
            {
                t = Mathf.Pow(1-(timer / selectedDragTime), 2);
            }
            
            selectedTransform.localPosition = new Vector3(0,-canvasScaler.referenceResolution.y* t, 0);
            yield return null;
        }
        if (!isActive)
        {
            selectedTransform.gameObject.SetActive(false);
        }
    }
    public void UpdateUI()
    {
        if (changeTextCorutine == null) { changeTextCorutine = StartCoroutine(ChangeText()); }
        
    }
    IEnumerator ChangeText()
    {
        int textSuns = int.Parse(sunsQuantityText.text);
        while (textSuns != EconomicController.instance.Suns)
        {
            if (textSuns < EconomicController.instance.Suns) { textSuns++; }
            if (textSuns > EconomicController.instance.Suns) { textSuns--; }
            sunsQuantityText.text = textSuns.ToString();
            yield return null;
        }
        changeTextCorutine = null;
    }
    
}

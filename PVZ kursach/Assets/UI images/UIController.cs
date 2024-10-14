using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
    public Transform selectButtonsContent;
    public const int selectButtonShift = -5;

    private void Awake()
    {
        selectButtons = new List<SelectButton>();
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

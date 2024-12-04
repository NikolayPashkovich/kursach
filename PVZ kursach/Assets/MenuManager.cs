using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    static MenuManager _instanvce;
    [SerializeField] CanvasGroup[] levelPanels;
    [SerializeField] AudioSource buttonClickSound;
    public void GoToLevel(LevelData levelData)
    {
        ButtonClickSound();
        StartCoroutine(LoadSceneRoutine(levelData));
    }
    public void ButtonClickSound()
    {
        buttonClickSound.Play();
    }
    private void Start()
    {
        int levelCount = FileManager.LoadLevelNumber();
        for (int i = 0; i < levelPanels.Length; i++)
        {
            if (i > levelCount)
            {
                levelPanels[i].alpha = 0.5f;
                levelPanels[i].interactable = false;
            }
        }
    }
    public void NewGameButtonClick()
    {
        FileManager.DeleteSaveFile();
        ButtonClickSound();
        SceneManager.LoadSceneAsync(0);
    }
    private IEnumerator LoadSceneRoutine(LevelData levelData)
    {
        // Загружаем сцену асинхронно
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Ждем завершения загрузки сцены
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // После загрузки находим объект Singleton в новой сцене и передаем данные
        Debug.Log("StartLevel");
        EconomicController.instance.StartLevel(levelData);
    }
    private void Awake()
    {
        if (_instanvce != null)
        {
            Destroy(_instanvce.gameObject);
            _instanvce = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void QuitButton()
    {
        ButtonClickSound();
        Application.Quit();
    }

    // Update is called once per frame

}

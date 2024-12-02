using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    static MenuManager _instanvce;
    public void GoToLevel(LevelData levelData)
    {
        StartCoroutine(LoadSceneRoutine(levelData));
    }
    private IEnumerator LoadSceneRoutine(LevelData levelData)
    {
        // ��������� ����� ����������
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // ���� ���������� �������� �����
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ����� �������� ������� ������ Singleton � ����� ����� � �������� ������
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

    // Update is called once per frame

}

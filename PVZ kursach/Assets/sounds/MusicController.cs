using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip mainBattleClip;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip looseClip;
    
    public void GoToGame()
    {
        audio.clip = mainBattleClip;
        audio.Play();
    }
    public IEnumerator Win()
    {
        audio.clip = winClip;
        audio.loop = false;
        audio.Play();
        while (audio.isPlaying)
        {
            yield return null;
        }
    }
    public void Loose()
    {
        audio.clip = looseClip;
        audio.loop = false;
        audio.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{
    public void Boom()
    {
        audio.clip = actionAudio;
        audio.Play();
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            Vector2Int zombiePosInGrid = GridManager.Instance.zombies[i].posInGrid;
            Vector2 dir = posInGrid - zombiePosInGrid;
            if (dir.x <= 1 && dir.x >= -1 && dir.y <= 1 && dir.y >= -1)
            {
                GridManager.Instance.zombies[i].FireDeath();
            }
        }
    }
   
}

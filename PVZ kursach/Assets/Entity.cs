using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;
    public Vector2Int posInGrid;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Collider2D collider;
    [SerializeField] protected int maxHP;
    protected int hp;
    [SerializeField] protected Color normalColor;
    [SerializeField] protected Color glareColor;
    [SerializeField] protected float glareSpeed;
    protected IEnumerator glareCorutine()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * glareSpeed;
            spriteRenderer.color = Color.Lerp(normalColor, glareColor, timer);
            yield return null;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime * glareSpeed;
            spriteRenderer.color = Color.Lerp(normalColor, glareColor, timer);
            yield return null;
        }
        spriteRenderer.color = normalColor;
    }
}

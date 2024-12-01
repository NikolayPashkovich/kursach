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
    protected const float glareSpeed = 5;
    [SerializeField] protected Vector3 positionShift;

    public virtual Color NormalColor()
    {
        return normalColor;
    }
    public virtual void Awake()
    {
        SelectLayer();
        transform.position += positionShift;
        hp = maxHP;
        Plant plant = this as Plant;
        if (plant != null) { GridManager.Instance.plants.Add(plant); }
        Zombie zombie = this as Zombie;
        if (zombie != null) { GridManager.Instance.zombies.Add(zombie); }
    }
    private void OnDestroy()
    {
        Plant plant = this as Plant;
        Zombie zombie = this as Zombie;
        if (GridManager.Instance.plants.Contains(plant)) { GridManager.Instance.plants.Remove(plant); }
        if (GridManager.Instance.zombies.Contains(zombie)) { GridManager.Instance.zombies.Remove(zombie); }
    }
    public void DestroyGameObj()
    {
        Destroy(gameObject);
    }
    public Vector3 GetPosShift()
    {
        return positionShift;
    }
    public virtual void Damage(int damage)
    {
        hp -= damage;
        StartCoroutine(glareCorutine());
        if (hp <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    public Vector3 Position()
    {
        return transform.position - positionShift;
    }
    protected virtual void SelectLayer()
    {
        if (this is Zombie)
        {
            spriteRenderer.sortingOrder = GridManager.Instance.rows - GridManager.Instance.GetGridCellFromPosition(Position()).y + 1;
            return;
        }
        spriteRenderer.sortingOrder = GridManager.Instance.rows - GridManager.Instance.GetGridCellFromPosition(Position()).y;
    }
    
    protected IEnumerator glareCorutine()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * glareSpeed;
            spriteRenderer.color = Color.Lerp(NormalColor(), glareColor, timer);
            yield return null;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime * glareSpeed;
            spriteRenderer.color = Color.Lerp(NormalColor(), glareColor, timer);
            yield return null;
        }
        spriteRenderer.color = NormalColor();
    }
}

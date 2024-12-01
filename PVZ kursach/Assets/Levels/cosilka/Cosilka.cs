using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosilka : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 minScale;
    [SerializeField] Vector2 maxScale;
    [SerializeField] float timeToScaleChange;
    float scaleChangeTimer;
    [SerializeField] Rigidbody2D rb;
    bool isActive = false;
    [SerializeField] float maxX;
    private void Update()
    {
        if (!isActive) { return; }
        rb.MovePosition((Vector2)transform.position + (Vector2.right * moveSpeed));
        scaleChangeTimer -= Time.deltaTime;
        if (scaleChangeTimer <= 0)
        {
            scaleChangeTimer = timeToScaleChange;
            transform.localScale = new Vector2(Mathf.Lerp(minScale.x, maxScale.x, Random.Range(0, 1f)), Mathf.Lerp(minScale.y, maxScale.y, Random.Range(0, 1f)));
        }
        if (transform.position.x > maxX)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            zombie.Damage(int.MaxValue);
            isActive = true;
        }

    }
}

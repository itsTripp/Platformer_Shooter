using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    public void Heal()
    {
        sprite.color = Color.green;
        StartCoroutine(ReturnToNormal());
    }

    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(ReturnToNormal());
    }

    IEnumerator ReturnToNormal()
    {
        yield return new WaitForSeconds(.2f);
        sprite.color = Color.white;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

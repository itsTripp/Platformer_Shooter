using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, float percent)
    {
        textMesh.text = Mathf.Abs(damageAmount).ToString();

         if (percent > 25)
        {
            textMesh.faceColor = Color.red;
        }  else if (percent < 0)
        {
            textMesh.faceColor = Color.green;
        } else
        {
            textMesh.faceColor = Color.yellow;
        }
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.UI;
using UnityEngine.UI;

public class PatternPanelController : MonoBehaviour
{
    private bool cleared = false;
    private int scorePoint = 10;
    private int patternIndex;

    public void SetPattern()
    {
        patternIndex = Random.Range(1, 5);
        var rend = this.GetComponent<Renderer>();
        rend.material.SetFloat("_MaskTex", patternIndex);
    }

    public void StartMoving(Transform target, float speed)
    {
        StartCoroutine(Move(target, speed));
    }

    IEnumerator Move(Transform target, float speed)
    {
        while (Vector3.Distance(transform.position, target.position) > 0)
        {
            if (cleared)
            {
                break;
            }
            var step = Time.deltaTime * speed * 30;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            yield return null;
        }

        gameObject.SetActive(false);
        CheckPattern();
    }

    void CheckPattern()
    {
        if (GameManager.instance.CurrentPatternIndex == this.patternIndex)
        {
            GameManager.instance.AddScore(this.scorePoint);
        }
        else
        {
            GameManager.instance.GameOver();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPanelController : MonoBehaviour
{
    private bool cleared = false;
    private int scorePoint = 10;
    private int patternIndex;

    public void SetPattern()
    {
        patternIndex = Random.Range(1, 5);
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
    }

    public void PatternMatched()
    {
        cleared = true;
        GameManager.instance.AddScore(scorePoint);
    }
}

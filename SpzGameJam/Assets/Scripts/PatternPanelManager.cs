using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPanelManager : MonoBehaviour
{
    [SerializeField] Transform patternPanelSpawnPos;
    [SerializeField] Transform panelMovingTargetPos;
    [SerializeField] GameObject patternPanelPrefab;

    [SerializeField] List<Sprite> patterns;

    private int panelPooledAmount = 5;
    private List<GameObject> panels;

    void Start()
    {
        panels = new List<GameObject>();
        for (int i = 0; i < panelPooledAmount; i++)
        {
            var panel = Instantiate(patternPanelPrefab);
            panel.SetActive(false);
            panels.Add(panel);
        }
    }

    public void SpawnPatternPanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            var panel = panels[i];
            if (!panel.activeInHierarchy)
            {
                panel.transform.position = patternPanelSpawnPos.position;
                panel.transform.rotation = Quaternion.identity;
                var panelController = panel.GetComponent<PatternPanelController>();
                panelController.SetPattern(patterns);
                panel.SetActive(true);
                panelController.StartMoving(panelMovingTargetPos, GameManager.instance.ScrollingSpeed);
                break;
            }
        }
    }
}

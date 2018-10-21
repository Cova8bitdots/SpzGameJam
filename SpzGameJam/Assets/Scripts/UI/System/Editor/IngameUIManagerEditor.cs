using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.UI;
using UnityEditor;


[CustomEditor(typeof(IngameUIManager))]
public class IngameUIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		IngameUIManager manager = target as IngameUIManager;

		EditorGUILayout.BeginVertical(GUI.skin.box);
		{
			//Title
			EditorGUILayout.LabelField("State", GUI.skin.box);

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				var names = manager.GetStateNameArray();

				for (int i = 0; i < names.Length; i++)
				{
					EditorGUILayout.LabelField(
						string.Format("[State {0}] {1}", i, names[i])
						);
				}
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();

		Repaint();
    }
}

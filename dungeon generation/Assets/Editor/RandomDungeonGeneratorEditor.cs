using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator),true)]
public class RandomDungeonGeneratorEditor : Editor
{
  AbstractDungeonGenerator m_Generator;
    private void Awake()
    {
        m_Generator=(AbstractDungeonGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            m_Generator.GenerateDungeon();
        }
    }
}

#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
/// <summary>
/// Should this field be visible in the GameTweaker window
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
sealed class ShowInTweaker : System.Attribute
{

   public ShowInTweaker () 
   { 

   }
}

public class GameTweaker : EditorWindow {
    [SerializeField]
    private string[] editableTypeNames = new string[10];
    [SerializeField]
    private bool open = false;
    private List<Type> editableTypes = new List<Type>();
    private List<UnityEngine.Object> modifyableObjects;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Game Settings")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GameTweaker window = (GameTweaker)EditorWindow.GetWindow(typeof(GameTweaker));
        window.Show();
    }

    private void RefreshContent()
    {
        modifyableObjects = new List<UnityEngine.Object>();
        foreach (string t in editableTypeNames)
        {
            Type type = Type.GetType(t);
            if (type != null)
            {
                modifyableObjects.AddRange(FindObjectsOfType(type));
            }
            
        }
    }

    void OnGUI()
    {
        
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        foreach (UnityEngine.Object obj in modifyableObjects)
        {
            DisplayObject(obj);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Under da hood");
        SerializedObject self = new SerializedObject(this);
        SerializedProperty p = self.FindProperty("editableTypeNames");

        open = EditorGUILayout.Foldout(open, p.name);
        Debug.Log(open);
        if (open)
        {
            int nSize = EditorGUILayout.IntField("Size", p.arraySize);
            for (int i = 0; i < p.arraySize; i++)
            {
                EditorGUILayout.PropertyField(p.GetArrayElementAtIndex(i));
            }
            p.arraySize = nSize;
        }

        
        self.ApplyModifiedProperties();
        if (GUILayout.Button("Refresh")) RefreshContent();
        EditorGUILayout.EndVertical();
    }

    private void DisplayObject(UnityEngine.Object obj)
    {
        Type type = obj.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        SerializedObject o = new SerializedObject(obj);
        foreach (FieldInfo field in fields)
        {
            bool hasShowinTweakerAttribute = false;
            foreach (Attribute at in field.GetCustomAttributes(true))
            {
                if (at is ShowInTweaker)
                {
                    hasShowinTweakerAttribute = true;
                }
            }
            if (hasShowinTweakerAttribute)
            {
                EditorGUILayout.PropertyField(o.FindProperty(field.Name));
            }
        }
        o.ApplyModifiedProperties();
    }
    
}


#endif
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
[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class ShowInTweaker : System.Attribute
{
    readonly bool sharedAmongAllInstances;
    public ShowInTweaker (bool sharedAmongAllInstances = false) 
    {
        this.sharedAmongAllInstances = sharedAmongAllInstances;
    }

    public bool isSharedAmongAllInstances { get { return sharedAmongAllInstances; } }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class UsedInTweaker : Attribute
{
    public UsedInTweaker()
    {
    }
}

public class GameTweaker : EditorWindow {
    [SerializeField]
    private string[] editableTypeNames = new string[10];
    [SerializeField]
    private bool open = false;
    private List<Type> editableTypes = new List<Type>();
    private List<List<UnityEngine.Object>> modifyables = new List<List<UnityEngine.Object>>();

    private bool hasFocus = false;
    private int refreshTick = 0;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Game Settings")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GameTweaker window = (GameTweaker)EditorWindow.GetWindow(typeof(GameTweaker));
        window.RefreshContent();
        window.Show();
    }

    private void RefreshContent()
    {
        editableTypes = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if(type.GetCustomAttributes(typeof(UsedInTweaker), true).Length > 0)
                {
                    editableTypes.Add(type);
                }
            }
        }
        modifyables = new List<List<UnityEngine.Object>>();
        foreach (Type type in editableTypes)
        {
            if (type != null)
            {
                UnityEngine.Object[] objs = FindObjectsOfType(type);
                if(objs.Length > 0)
                    modifyables.Add(new List<UnityEngine.Object>(objs));
            }
            
        }
    }

    void OnGUI()
    {
        try
        {
            GUIStyle s = new GUIStyle();
            s.fontSize = 14;
            s.fontStyle = FontStyle.Bold;
            
            //s.alignment = TextAnchor.UpperCenter;
            EditorGUILayout.LabelField("Shared Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            foreach (List<UnityEngine.Object> obj in modifyables)
                DisplayClass(obj);
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Instanced Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (List<UnityEngine.Object> list in modifyables)
                foreach (UnityEngine.Object obj in list)
                    DisplayObject(obj);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        catch (UnityException e)
        {
            RefreshContent();
        }
        
    }

    void OnFocus()
    {
        RefreshContent();
    }
    void OnLostFocus()
    {
        RefreshContent();
    }
    void OnHeirarchyChange()
    {
        RefreshContent();
    }

    private void DisplayClass(List<UnityEngine.Object> objRef)
    {
        SerializedObject o = new SerializedObject(objRef[0]);
        Type type = objRef[0].GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        EditorGUILayout.LabelField(type.ToString(), EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (FieldInfo field in fields)
        {
            bool hasShowinTweakerAttribute = false;
            foreach (Attribute at in field.GetCustomAttributes(true))
            {
                if (at is ShowInTweaker)
                {
                    Debug.Log(((ShowInTweaker)at).isSharedAmongAllInstances);
                    hasShowinTweakerAttribute = ((ShowInTweaker)at).isSharedAmongAllInstances;
                }
            }
            if (hasShowinTweakerAttribute)
            {
                var prop = o.FindProperty(field.Name);
                EditorGUILayout.PropertyField(prop);
                for (int i = 1; i < objRef.Count; i++)
                {
                    SerializedObject o2 = new SerializedObject(objRef[i]);
                    o2.CopyFromSerializedProperty(prop);
                    o2.ApplyModifiedPropertiesWithoutUndo();
                }
                o.ApplyModifiedProperties();
            }
        }
        EditorGUI.indentLevel--;

    }

    private void DisplayObject(UnityEngine.Object obj)
    {
        Type type = obj.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        SerializedObject o = new SerializedObject(obj);
        EditorGUILayout.LabelField(obj.name + " (" + type.ToString() + ")", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (FieldInfo field in fields)
        {
            bool hasShowinTweakerAttribute = false;
            foreach (Attribute at in field.GetCustomAttributes(true))
            {
                if (at is ShowInTweaker && !((ShowInTweaker)at).isSharedAmongAllInstances)
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
        EditorGUI.indentLevel--;
    }
    
}


#endif
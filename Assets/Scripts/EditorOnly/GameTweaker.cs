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
sealed class TweakableField : System.Attribute
{
    readonly bool sharedAmongAllInstances;
    public TweakableField (bool sharedAmongAllInstances = false) 
    {
        this.sharedAmongAllInstances = sharedAmongAllInstances;
    }

    public bool isSharedAmongAllInstances { get { return sharedAmongAllInstances; } }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class TweakableClass : Attribute
{
    public TweakableClass()
    {
    }
}

public class GameTweaker : EditorWindow {
    [SerializeField]
    private bool open = false;
    private List<Type> editableTypes = new List<Type>();
    private List<List<UnityEngine.Object>> modifyables = new List<List<UnityEngine.Object>>();
    private Dictionary<int, bool> arrShown;
    private bool hasFocus = false;
    private int refreshTick = 0;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Game Tweaker")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GameTweaker window = (GameTweaker)EditorWindow.GetWindow(typeof(GameTweaker));
        window.RefreshContent();
        window.Show();
    }

    private void RefreshContent()
    {
        arrShown = new Dictionary<int, bool>();
        editableTypes = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if(type.GetCustomAttributes(typeof(TweakableClass), true).Length > 0)
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
    private Vector2 scroll = Vector2.zero;
    void OnGUI()
    {
        try
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            
            GUIStyle s = new GUIStyle();
            s.fontSize = 14;
            s.fontStyle = FontStyle.Bold;
            
            //s.alignment = TextAnchor.UpperCenter;
            EditorGUILayout.LabelField("Shared Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            foreach (List<UnityEngine.Object> obj in modifyables)
                DisplayShared(obj);
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Instanced Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (List<UnityEngine.Object> list in modifyables)
                foreach (UnityEngine.Object obj in list)
                    DisplayInstanced(obj);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
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

    private void DisplayShared(List<UnityEngine.Object> objRef)
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
                if (at is TweakableField)
                {
                    hasShowinTweakerAttribute = ((TweakableField)at).isSharedAmongAllInstances;
                }
            }
            if (hasShowinTweakerAttribute)
            {
                var prop = o.FindProperty(field.Name);
                if (prop.isArray)
                {
                    DrawArrayProperty(prop);
                } 
                else
                {
                    EditorGUILayout.PropertyField(prop);
                }
                
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
    private bool booly;

    private void DrawArrayProperty(SerializedProperty prop)
    {
        /*int id = prop.serializedObject.targetObject.GetInstanceID();
        if (!arrShown.ContainsKey(id))
        {
            arrShown.Add(id, false);
        }
        arrShown[id] = EditorGUILayout.Foldout(arrShown[id], prop.name);
        if (true)
        {

            int nArrSize = EditorGUILayout.IntField("Size", prop.arraySize);
            for (int i = 0; i < prop.arraySize; i++)
            {
                
                SerializedProperty propItem = prop.GetArrayElementAtIndex(i);
                
                if (propItem.isArray)
                {
                    Debug.Log("SUBARRAY");
                    DrawArrayProperty(propItem);
                }
                else
                {
                    EditorGUILayout.PropertyField(propItem);
                }
            }
            prop.arraySize = nArrSize;
        }*/
        EditorGUILayout.PropertyField(prop);
        if (prop.isExpanded)
        {
            EditorGUI.indentLevel++;
            SerializedProperty propChild = prop.Copy();
            propChild.NextVisible(true);
            EditorGUILayout.PropertyField(propChild);

            for (int i = 0; i < prop.arraySize; i++)
            {
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i));
            }
               
            /*while (propChild.NextVisible(false))
                {
                    i++;
                    if (prop.isArray)
                    {
                        DrawArrayProperty(prop);
                    }
                    else
                    {
                    
                    }
                    EditorGUILayout.PropertyField(propChild);

                }*/
            EditorGUI.indentLevel--;
        }
        
    }

    private void DisplayInstanced(UnityEngine.Object obj)
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
                if (at is TweakableField && !((TweakableField)at).isSharedAmongAllInstances)
                {
                    hasShowinTweakerAttribute = true;
                }
            }
            if (hasShowinTweakerAttribute)
            {
                SerializedProperty prop = o.FindProperty(field.Name);
                if (prop.isArray)
                {
                    DrawArrayProperty(prop);
                }
                else
                {
                    EditorGUILayout.PropertyField(prop);
                }
                
            }
        }
        o.ApplyModifiedProperties();
        EditorGUI.indentLevel--;
    }
    
}


#endif
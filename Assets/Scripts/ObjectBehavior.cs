using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum ObjectType { Cube, Sphere };

public class ObjectBehavior : MonoBehaviour
{

    public ObjectType objectType;
    [HideInInspector] public float objectScale;

}

#if UNITY_EDITOR

[CustomEditor(typeof(ObjectBehavior))]
[CanEditMultipleObjects]
public class ObjectEditor : Editor
{
   
    public override void OnInspectorGUI()
    {        

        base.OnInspectorGUI();
        ObjectType editorType = (target as ObjectBehavior).objectType;

        EditorGUILayout.BeginHorizontal();

        var ourBehaviour = GameObject.FindObjectsOfType<ObjectBehavior>(true);

        var ourObjects = ourBehaviour
            .Select(theObject => { if (theObject.objectType == editorType) { return theObject.gameObject; } else { return null; } })
            .ToArray();

        if (GUILayout.Button("Select All"))
        {                       
            Selection.objects = ourObjects;
        }

        if (GUILayout.Button("Clear Selection"))
        {
            Selection.objects = new Object[] { (target as ObjectBehavior).gameObject };
        }
        EditorGUILayout.EndHorizontal();

        var cachedColor = GUI.backgroundColor;

        bool isOneDisabled = false;

        foreach (var foundObject in ourObjects) 
        {
            if (foundObject == null) 
            {
                continue;
            }

            if (!foundObject.activeSelf) 
            {
                GUI.backgroundColor = Color.red;
                isOneDisabled = true;
                break;
            }
        
            GUI.backgroundColor = Color.green;        
            
        }


        if (GUILayout.Button("Disable/Enable Objects", GUILayout.Height(40)))
        {
            
            for (int i = 0; i < ourObjects.Length; i++) 
            {
                if (ourObjects[i] == null) 
                {
                    continue;
                }

                ourObjects[i].SetActive(isOneDisabled);
            }

        }

        GUI.backgroundColor = cachedColor;
        float minValue = 0.5f, maxValue = 3.0f;

        serializedObject.Update();
        var thisObjectScale = serializedObject.FindProperty("objectScale");
        EditorGUILayout.PropertyField(thisObjectScale);       


        if (thisObjectScale.floatValue <= minValue)
        {
            EditorGUILayout.HelpBox("This object's minimum scale is " + minValue + "!", MessageType.Warning);
            thisObjectScale.floatValue = minValue;
        }
        else if (thisObjectScale.floatValue >= maxValue) 
        {
            EditorGUILayout.HelpBox("This object's maximum scale is " + maxValue + "!", MessageType.Warning);
            thisObjectScale.floatValue = maxValue;
        }

        serializedObject.targetObject.GetComponent<Transform>().transform.localScale 
            = new Vector3(thisObjectScale.floatValue, thisObjectScale.floatValue, thisObjectScale.floatValue);

        serializedObject.ApplyModifiedProperties();

    }

}

#endif

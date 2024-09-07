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
        if (GUILayout.Button("Select All"))
        {           
            var ourBehaviour = GameObject.FindObjectsOfType<ObjectBehavior>();

            var ourObjects = ourBehaviour
                .Select(theObject => { if (theObject.objectType == editorType) { return theObject.gameObject; } else { return null; } })
                .ToArray();
            Selection.objects = ourObjects;

            Debug.Log("This button works!");
        }

        if (GUILayout.Button("Clear Selection"))
        {
            Selection.objects = new Object[] { (target as ObjectBehavior).gameObject };
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Disable/Enable Objects", GUILayout.Height(40)))
        {
            // TO DO: This should select each of one object type, but it currently selects all the objects. 
            // It should also change between colors when clicked.
            foreach (var typeOfObject in GameObject.FindObjectsOfType<ObjectBehavior>(true) ) 
            {
                typeOfObject.gameObject.SetActive(!typeOfObject.gameObject.activeSelf);
            }
            
        }

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

#endif

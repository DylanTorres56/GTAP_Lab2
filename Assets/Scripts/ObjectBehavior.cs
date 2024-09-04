using System.Collections;
using System.Collections.Generic;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;



public class ObjectBehavior : MonoBehaviour
{

    public ObjectType objectType;

}

public enum ObjectType { Cube, Sphere };

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

        if (GUILayout.Button("Disable/Enable objects", GUILayout.Height(40)))
        {
            
        }
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

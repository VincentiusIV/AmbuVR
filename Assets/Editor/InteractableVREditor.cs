using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractableVR))]
public class InteractableVREditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        /*var myScript = target as InteractableVR;
        
        //--- Rotate ---//
        myScript.rotate = GUILayout.Toggle(myScript.rotate, "Rotate");

        if (myScript.rotate)
        {
            myScript.minRotation = EditorGUILayout.Vector3Field("Min Rotation", myScript.minRotation);
            myScript.maxRotation = EditorGUILayout.Vector3Field("Max Rotation", myScript.maxRotation);
            myScript.activationThreshold = EditorGUILayout.IntField("Activation Threshold",0);
        }
            */

    }
}

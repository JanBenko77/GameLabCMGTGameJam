using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Dialogue dialogue = (Dialogue)target;

        // Draw the dialogueLines list with custom handling
        SerializedProperty dialogueLinesProperty = serializedObject.FindProperty("dialogueLines");
        for (int i = 0; i < dialogueLinesProperty.arraySize; i++)
        {
            SerializedProperty element = dialogueLinesProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, new GUIContent($"Element {i}"), true);
        }

        if (GUILayout.Button("Add Dialogue Line"))
        {
            AddElement(dialogueLinesProperty, typeof(DialogueLine));
        }

        if (GUILayout.Button("Add Choice"))
        {
            AddElement(dialogueLinesProperty, typeof(Choice));
        }

        if (GUILayout.Button("Add Checkpoint"))
        {
            AddElement(dialogueLinesProperty, typeof(Checkpoint));
        }

        if (GUILayout.Button("Add ChoiceCheckpoint"))
        {
            AddElement(dialogueLinesProperty, typeof(ChoiceCheckpoint));
        }

        if (GUILayout.Button("Remove Last Element"))
        {
            dialogueLinesProperty.arraySize--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddElement(SerializedProperty list, System.Type type)
    {
        list.arraySize++;
        SerializedProperty element = list.GetArrayElementAtIndex(list.arraySize - 1);
        element.managedReferenceValue = System.Activator.CreateInstance(type);
    }
}

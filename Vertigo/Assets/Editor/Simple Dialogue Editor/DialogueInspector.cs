using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Dialogue))]
public class DialogueInspector : Editor {

	private ReorderableList dialoguelist;

	private void OnEnable(){

		dialoguelist = new ReorderableList (serializedObject, serializedObject.FindProperty ("DialogueItems"), true, true, true, true);
		dialoguelist.elementHeight = 110f;
		dialoguelist.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

			List<DialogueModel> _dlist = ((Dialogue)dialoguelist.serializedProperty.serializedObject.targetObject).DialogueItems;
			DialogueModel item = _dlist[index];


			EditorGUI.LabelField(new Rect(rect.x, rect.y, 40, EditorGUIUtility.singleLineHeight), "Title: ");
			item.name = EditorGUI.TextField(new Rect(rect.x + 40, rect.y, rect.width - 40, EditorGUIUtility.singleLineHeight), item.name);

			EditorGUI.LabelField(new Rect(rect.x, rect.y + 20, 40, EditorGUIUtility.singleLineHeight), "Event: ");
			item.event_key = EditorGUI.TextField(new Rect(rect.x + 40, rect.y + 20, rect.width - 40, EditorGUIUtility.singleLineHeight), item.event_key);

			EditorGUI.LabelField(new Rect(rect.x, rect.y + 40, 60, EditorGUIUtility.singleLineHeight), "Message: ");
			item.message = EditorGUI.TextArea(new Rect(rect.x + 60, rect.y + 40, rect.width - 60, 50), item.message);

		};

		dialoguelist.onAddCallback = (ReorderableList l) => {
			List<DialogueModel> _dlist = ((Dialogue)dialoguelist.serializedProperty.serializedObject.targetObject).DialogueItems;
			_dlist.Add(new DialogueModel());
		};

		dialoguelist.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField (rect, "Dialogue Items");
		};

		dialoguelist.onRemoveCallback = (ReorderableList l) => {
			if (EditorUtility.DisplayDialog ("Warning1", "Are you sure you want to delete this dialogue entry.", "Yes", "No")) {
				ReorderableList.defaultBehaviours.DoRemoveButton (l);
			}
		};

	}

	public override void OnInspectorGUI(){	

		Dialogue di = (Dialogue)serializedObject.targetObject;
		di.MessageSpeed = EditorGUILayout.FloatField ("Message Speed", di.MessageSpeed);
		di.MinimumWaitSpeed = EditorGUILayout.FloatField ("Minimum Wait Time", di.MinimumWaitSpeed);

		GUILayout.Space (10f);

		serializedObject.Update ();

		dialoguelist.DoLayoutList ();

		serializedObject.ApplyModifiedProperties ();

		GUILayout.Space (5f);

	}

}

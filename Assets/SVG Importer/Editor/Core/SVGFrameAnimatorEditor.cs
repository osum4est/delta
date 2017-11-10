// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Reflection;

namespace SVGImporter
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SVGFrameAnimator))]
	public class SVGFrameAnimatorEditor : Editor
    {
		SerializedProperty frames;
		SerializedProperty frameIndex;

		private ReorderableList framesList;
		private float thumbnailSize = 30f;

        void OnEnable()
        {
			frames = serializedObject.FindProperty("frames");
			frameIndex = serializedObject.FindProperty("frameIndex");

			framesList = new ReorderableList(serializedObject, frames, true, true, true, true);
			framesList.drawHeaderCallback = (Rect rect) => {  
				EditorGUI.LabelField(rect, "Animation Frames");
			};
			framesList.elementHeight = thumbnailSize + 2;

			framesList.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
				var element = framesList.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;

				Texture2D thumbnail = null;
				if(element.objectReferenceValue != null)
				{
					thumbnail = AssetPreview.GetAssetPreview(element.objectReferenceValue);
				}

				
				if(thumbnail != null)
				{
					EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, thumbnailSize, thumbnailSize), thumbnail);
				}
				EditorGUI.PropertyField(new Rect(rect.x + thumbnailSize + 4, rect.y, rect.width - thumbnailSize - 4, EditorGUIUtility.singleLineHeight), element, new GUIContent());
			};
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
			EditorGUILayout.Space();
			framesList.DoLayoutList();
			EditorGUILayout.Space();
			if(frames.arraySize > 1)
			{
				frameIndex.floatValue = EditorGUILayout.IntSlider("Current Frame", (int)frameIndex.floatValue, 0, frames.arraySize - 1);
			}
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }       
    }
}
// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SVGImporter {

    [CustomEditor(typeof(SVGImporterSettings))]
    public class SVGImporterSettingsEditor : Editor
    {
        SerializedProperty format;
		SerializedProperty meshCompression;
        SerializedProperty scale;
        SerializedProperty vpm;
        SerializedProperty depthOffset;
        SerializedProperty compressDepth;
        SerializedProperty customPivotPoint;
        SerializedProperty pivotPoint;
        SerializedProperty generateCollider;
		SerializedProperty keepSVGFile;

        void OnEnable()
        {
			SVGImporterLaunchEditor.OpenSettingsWindow();
            format = serializedObject.FindProperty("defaultSVGFormat");
			meshCompression = serializedObject.FindProperty("defaultMeshCompression");
            scale = serializedObject.FindProperty("defaultScale");
            vpm = serializedObject.FindProperty("defaultVerticesPerMeter");
            depthOffset = serializedObject.FindProperty("defaultDepthOffset");
            compressDepth = serializedObject.FindProperty("defaultCompressDepth");
            customPivotPoint = serializedObject.FindProperty("defaultCustomPivotPoint");
            pivotPoint = serializedObject.FindProperty("defaultPivotPoint");
            generateCollider = serializedObject.FindProperty("defaultGenerateCollider");
			keepSVGFile = serializedObject.FindProperty("defaultKeepSVGFile");
        }

        public override void OnInspectorGUI()
        {
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(format, new GUIContent("Format"));
			EditorGUILayout.PropertyField(meshCompression, new GUIContent("Mesh Compression"));
            EditorGUILayout.PropertyField(scale, new GUIContent("Scale"));
            EditorGUILayout.PropertyField(vpm, new GUIContent("Vertex per Meter"));
            
            if (format.enumValueIndex == (int)SVGAssetFormat.Opaque)
            {
                EditorGUILayout.PropertyField(depthOffset, new GUIContent("Depth Offset"));
                EditorGUILayout.PropertyField(compressDepth, new GUIContent("Compress Depth"));           
            }

            EditorGUILayout.PropertyField(customPivotPoint, new GUIContent("Custom Pivot"));
            if (customPivotPoint.boolValue)
            { 
                EditorGUILayout.PropertyField(pivotPoint, new GUIContent("Pivot"));
            } else
            {
                Vector2 pivotPointVector = pivotPoint.vector2Value;
                int selectedIndex = Mathf.RoundToInt(pivotPointVector.x * 2 + Mathf.Clamp(pivotPointVector.y * 6, 0, 8));
                
                selectedIndex = EditorGUILayout.Popup("Pivot ", selectedIndex, SVGAssetEditor.anchorPosition);
                
                int x = selectedIndex % 3;
                int y = Mathf.FloorToInt(selectedIndex / 3);
                
                pivotPointVector.x = x / 2f;
                pivotPointVector.y = y / 2f;
                
                pivotPoint.vector2Value = pivotPointVector;
            }
            EditorGUILayout.PropertyField(generateCollider, new GUIContent("Generate Collider"));
			EditorGUILayout.PropertyField(keepSVGFile, new GUIContent("Keep SVG File"));
            GUILayout.Space(10f);

			if(EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

            EditorGUILayout.BeginHorizontal();
            if(SVGPostprocessor.active)
            {
                EditorGUILayout.LabelField("Asset Postprocessor: On");
                if(GUILayout.Button("Stop"))
                {
                    SVGPostprocessor.Stop();
                }
            } else {
                EditorGUILayout.LabelField("Asset Postprocessor: Off");
                if(GUILayout.Button("Start"))
                {
                    SVGPostprocessor.Start();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if(SVGImporterLaunchEditor.active)
            {
                EditorGUILayout.LabelField("Support Service: On");
                /*
                if(GUILayout.Button("Stop"))
                {
                    SVGImporterLaunchEditor.Stop();
                }
                */
            } else {
                EditorGUILayout.LabelField("Support Service: Off");
                if(GUILayout.Button("Start"))
                {
                    SVGImporterLaunchEditor.Stop();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}

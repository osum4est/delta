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
    [CustomEditor(typeof(SVGRenderer))]
    public class SVGRendererEditor : Editor
    {
        SVGRenderer renderer;
        static bool materialsFoldout;
        static bool sortingLayerFoldout;
        string[] sortingLayerNames;
        int[] sortingLayerIDs;
        SerializedProperty vectorGraphics;
        SerializedProperty color;
        SerializedProperty materials;
        SerializedProperty transparentMaterial;
        SerializedProperty opaqueMaterial;
        SerializedProperty sortingLayerID;
//        SerializedProperty sortingLayerName;
        SerializedProperty sortingOrder;
        SerializedProperty overrideSorter;

        void OnEnable()
        {
            vectorGraphics = serializedObject.FindProperty("_vectorGraphics");
            color = serializedObject.FindProperty("_color");
            transparentMaterial = serializedObject.FindProperty("_transparentMaterial");
            opaqueMaterial = serializedObject.FindProperty("_opaqueMaterial");
            sortingLayerID = serializedObject.FindProperty("_sortingLayerID");
//            sortingLayerName = serializedObject.FindProperty("_sortingLayerName");
            sortingOrder = serializedObject.FindProperty("_sortingOrder");
            overrideSorter = serializedObject.FindProperty("_overrideSorter");

            if (serializedObject.isEditingMultipleObjects)
            {
                SVGRenderer renderer = (SVGRenderer)target;
                Renderer meshRenderer = renderer.GetComponent<Renderer>();
                if (meshRenderer != null)
                    UnityEditor.EditorUtility.SetSelectedWireframeHidden(meshRenderer, true);
            } else
            {
                UnityEngine.Object[] renderers = (UnityEngine.Object[])targets;
                SVGRenderer renderer;
                if (renderers != null && renderers.Length > 0)
                {
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        renderer = renderers [i] as SVGRenderer;
                        if (renderer == null)
                            continue;
                        MeshRenderer meshRenderer = renderer.GetComponent<MeshRenderer>();
                        if (meshRenderer != null)
                        {
                            EditorUtility.SetSelectedWireframeHidden(meshRenderer, true);
                        }
                    }
                }
            }

            sortingLayerNames = GetSortingLayerNames();
            sortingLayerIDs = GetSortingLayerUniqueIDs();
        }

        void OpaqueMaterialHelpBox()
        {
            EditorGUILayout.HelpBox("Opaque Material is not used when vector graphics format is set to transparent or UGUI", MessageType.Warning);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(vectorGraphics, new GUIContent("Vector Graphics"));
            EditorGUILayout.PropertyField(color, new GUIContent("Color"));

            if(!transparentMaterial.hasMultipleDifferentValues && !opaqueMaterial.hasMultipleDifferentValues)
            {
                if(transparentMaterial.objectReferenceValue != opaqueMaterial.objectReferenceValue)
                {
                    EditorGUI.showMixedValue = true;

                    Material material = EditorGUILayout.ObjectField("material", null, typeof(Material), false) as Material;
                    if(material != null)
                    {
                        transparentMaterial.objectReferenceValue = material;
                        opaqueMaterial.objectReferenceValue = material;
                    }

                    EditorGUI.showMixedValue = false;
                } else {
                    Material oldMaterial = transparentMaterial.objectReferenceValue as Material;
                    Material material = EditorGUILayout.ObjectField("material", oldMaterial, typeof(Material), false) as Material;
                    if(oldMaterial != material)
                    {
                        transparentMaterial.objectReferenceValue = material;
                        opaqueMaterial.objectReferenceValue = material;
                    }
                }
            }

            materialsFoldout = EditorGUILayout.Foldout(materialsFoldout, "Advanced Materials");
            if(materialsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(transparentMaterial, new GUIContent("Transparent"));
                EditorGUILayout.PropertyField(opaqueMaterial, new GUIContent("Opaque"));

                SVGRenderer renderer = (SVGRenderer)target;
                if (serializedObject.isEditingMultipleObjects)
                {
                    UnityEngine.Object[] renderers = (UnityEngine.Object[])targets;
                    for(int i = 0; i < renderers.Length; i++)
                    {
                        renderer = renderers [i] as SVGRenderer;
                        if(renderer.vectorGraphics != null && renderer.vectorGraphics.format != SVGAssetFormat.Opaque)
                        {
                            OpaqueMaterialHelpBox();
                            break;
                        }
                    }
                } else {
                    if(renderer.vectorGraphics != null && renderer.vectorGraphics.format != SVGAssetFormat.Opaque)
                    {
                        OpaqueMaterialHelpBox();
                    }
                }
                EditorGUI.indentLevel--;
            }

            sortingLayerFoldout = EditorGUILayout.Foldout(sortingLayerFoldout, "Sorting Layer");
            if (sortingLayerFoldout)
            {
                EditorGUI.indentLevel++;

                int selectedSortingLayer = 0;
                for (int i = 0; i < sortingLayerIDs.Length; i++)
                {
                    if (sortingLayerIDs [i] == sortingLayerID.intValue)
                    {
                        selectedSortingLayer = i;
                        break;
                    }
                }

                EditorGUI.showMixedValue = sortingLayerID.hasMultipleDifferentValues;

                int newSortingLayer = sortingLayerIDs [EditorGUILayout.Popup("Sorting Layer", selectedSortingLayer, sortingLayerNames)];
                if (newSortingLayer != sortingLayerID.intValue)
                    sortingLayerID.intValue = newSortingLayer;

                EditorGUI.showMixedValue = false;

                EditorGUILayout.PropertyField(sortingOrder);
                EditorGUILayout.PropertyField(overrideSorter);
                EditorGUI.indentLevel--;
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            //if(serializedObject.ApplyModifiedProperties() || (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
        }
       
        public override string GetInfoString()
        {
            SVGAsset svgAsset = vectorGraphics.objectReferenceValue as SVGAsset;
            if (svgAsset)
                return GetEditorInfo(svgAsset);
            return "";
        }
        
        protected string GetEditorInfo(SVGAsset asset)
        {
            PropertyInfo _editor_Info = typeof(SVGAsset).GetProperty("_editor_Info", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)_editor_Info.GetValue(asset, new object[0]);
        }

        // Get the sorting layer names
        public string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        // Get the unique sorting layer IDs -- tossed this in for good measure
        public int[] GetSortingLayerUniqueIDs()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
        }
    }
}

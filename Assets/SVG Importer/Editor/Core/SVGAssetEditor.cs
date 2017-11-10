// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SVGImporter
{
	public class SVGAssetSnapshot
	{
		public SVGAssetFormat format;
		public SVGMeshCompression meshCompression;
		public float scale;
		public float vpm;
		public float depthOffset;
		public bool compressDepth;
		public bool customPivotPoint;
		public Vector2 pivotPoint;
        public Vector4 border;
        public bool sliceMesh;
        public bool generateCollider;
		public bool keepSVGFile;

		public SVGAssetSnapshot(){}
		
		public SVGAssetSnapshot(SVGAsset svgAsset)
		{
			Apply(svgAsset);
		}

		public SVGAssetSnapshot(SVGAssetSnapshot snapshot)
		{
			Apply(snapshot);
		}

		public SVGAssetSnapshot(SerializedObject serializedObject)
		{
			Apply(serializedObject);
		}

		public SVGAssetSnapshot Clone()
		{
			SVGAssetSnapshot snapshot = new SVGAssetSnapshot();
			snapshot.format = this.format;
			snapshot.meshCompression = this.meshCompression;
			snapshot.scale = this.scale;
			snapshot.vpm = this.vpm;
			snapshot.depthOffset = this.depthOffset;
			snapshot.compressDepth = this.compressDepth;
			snapshot.customPivotPoint = this.customPivotPoint;
			snapshot.pivotPoint = this.pivotPoint;
            snapshot.border = this.border;
            snapshot.sliceMesh = this.sliceMesh;
            snapshot.generateCollider = this.generateCollider;
			snapshot.keepSVGFile = this.keepSVGFile;
			return snapshot;
		}

		public void Apply(SVGAsset svgAsset)
		{
			this.format = svgAsset.format;
			this.meshCompression = svgAsset.meshCompression;
			this.scale = svgAsset.scale;
			this.vpm = svgAsset.vpm;
			this.depthOffset = svgAsset.depthOffset;
			this.compressDepth = svgAsset.compressDepth;
			this.customPivotPoint = svgAsset.customPivotPoint;
			this.pivotPoint = svgAsset.pivotPoint;
            this.border = svgAsset.border;
            this.sliceMesh = svgAsset.sliceMesh;
            this.generateCollider = svgAsset.generateCollider;
			this.keepSVGFile = svgAsset.keepSVGFile;
		}

		public void Apply(SVGAssetSnapshot snapshot)
		{
			this.format = snapshot.format;
			this.meshCompression = snapshot.meshCompression;
			this.scale = snapshot.scale;
			this.vpm = snapshot.vpm;
			this.depthOffset = snapshot.depthOffset;
			this.compressDepth = snapshot.compressDepth;
			this.customPivotPoint = snapshot.customPivotPoint;
			this.pivotPoint = snapshot.pivotPoint;
            this.border = snapshot.border;
            this.sliceMesh = snapshot.sliceMesh;
            this.generateCollider = snapshot.generateCollider;
			this.keepSVGFile = snapshot.keepSVGFile;
		}
		
		public void Apply(SerializedObject serializedObject)
		{
			this.format = (SVGAssetFormat)serializedObject.FindProperty("_format").enumValueIndex;
			this.meshCompression = (SVGMeshCompression)serializedObject.FindProperty("_meshCompression").enumValueIndex;
			this.scale = serializedObject.FindProperty("_scale").floatValue;
			this.vpm = serializedObject.FindProperty("_vpm").floatValue;
			this.depthOffset = serializedObject.FindProperty("_depthOffset").floatValue;
			this.compressDepth = serializedObject.FindProperty("_compressDepth").boolValue;
			this.customPivotPoint = serializedObject.FindProperty("_customPivotPoint").boolValue;
			this.pivotPoint = serializedObject.FindProperty("_pivotPoint").vector2Value;
            this.border = serializedObject.FindProperty("_border").vector4Value;
            this.sliceMesh = serializedObject.FindProperty("_sliceMesh").boolValue;
            this.generateCollider = serializedObject.FindProperty("_generateCollider").boolValue;
			this.keepSVGFile = serializedObject.FindProperty("_keepSVGFile").boolValue;
		}

		public void ModifySerializedObject(SerializedObject serializedObject)
		{
			serializedObject.FindProperty("_format").enumValueIndex = (int)this.format;
			serializedObject.FindProperty("_meshCompression").enumValueIndex = (int)this.meshCompression;
			serializedObject.FindProperty("_scale").floatValue = this.scale;
			serializedObject.FindProperty("_vpm").floatValue = this.vpm;
			serializedObject.FindProperty("_depthOffset").floatValue = this.depthOffset;
			serializedObject.FindProperty("_compressDepth").boolValue = this.compressDepth;
			serializedObject.FindProperty("_customPivotPoint").boolValue = this.customPivotPoint;
			serializedObject.FindProperty("_pivotPoint").vector2Value = this.pivotPoint;
            serializedObject.FindProperty("_border").vector4Value = this.border;
            serializedObject.FindProperty("_sliceMesh").boolValue = this.sliceMesh;
            serializedObject.FindProperty("_generateCollider").boolValue = this.generateCollider;
			serializedObject.FindProperty("_keepSVGFile").boolValue = this.keepSVGFile;
		}

		public void ModifySVGAssetSnapshot(SVGAssetSnapshot snapshot)
		{
			snapshot.format = this.format;
			snapshot.meshCompression = this.meshCompression;
			snapshot.scale = this.scale;
			snapshot.vpm = this.vpm;
			snapshot.depthOffset = this.depthOffset;
			snapshot.compressDepth = this.compressDepth;
			snapshot.customPivotPoint = this.customPivotPoint;
			snapshot.pivotPoint = this.pivotPoint;
            snapshot.border = this.border;
            snapshot.sliceMesh = this.sliceMesh;
            snapshot.generateCollider = this.generateCollider;
			snapshot.keepSVGFile = this.keepSVGFile;
		}
	}

    [CanEditMultipleObjects]
    [CustomEditor(typeof(SVGAsset))]
    public class SVGAssetEditor : Editor
    {
        public static SVGAssetEditor Instance;

        const string SVGAsset_LastSVGRecoveryKey = "SVGAsset_LastSVGRecoveryKey";
        public string lastSVGRecoveryPath
        {
            get {
                string output = null;
                if(EditorPrefs.HasKey(SVGAsset_LastSVGRecoveryKey))
                {
                    output = EditorPrefs.GetString(SVGAsset_LastSVGRecoveryKey);
                    if(string.IsNullOrEmpty(output))
                        return null;

                    if(!Directory.Exists(output))
                        return null;
                }
                return output;
            }
            set {
                EditorPrefs.SetString(SVGAsset_LastSVGRecoveryKey, value);
            }
        }

        const string SVGAsset_LastMeshSaveKey = "SVGAsset_LastMeshSaveKey";
        public string lastMeshSavePath
        {
            get {
                string output = null;
                if(EditorPrefs.HasKey(SVGAsset_LastMeshSaveKey))
                {
                    output = EditorPrefs.GetString(SVGAsset_LastMeshSaveKey);
                    if(string.IsNullOrEmpty(output))
                        return null;
                    
                    if(!Directory.Exists(output))
                        return null;
                }
                return output;
            }
            set {
                EditorPrefs.SetString(SVGAsset_LastMeshSaveKey, value);
            }
        }

		SVGAssetSnapshot[] svgAssetSnapshots;

        SVGAsset asset;
        SVGAsset[] assets;

        SerializedProperty format;
		SerializedProperty meshCompression;
        SerializedProperty scale;
        SerializedProperty vpm;
        SerializedProperty depthOffset;
        SerializedProperty compressDepth;
        SerializedProperty customPivotPoint;
        SerializedProperty pivotPoint;
        //SerializedProperty border;
        SerializedProperty generateCollider;
		SerializedProperty keepSVGFile;

        public bool unappliedChanges;
        bool filesValid;

        public static string[] anchorPosition = new string[]{
            "Top left",
            "Top",
            "Top right",
            "Left",
            "Center",
            "Right",
            "Bottom left",
            "Bottom",
            "Bottom Right"
        };

		public static GUIContent[] anchorPositionContent;

        static SVGAsset CreateAsset()
        {       
            SVGAsset tempAsset = SVGAsset.CreateInstance<SVGAsset>();
            return tempAsset;
        }

        private PreviewRenderUtility m_PreviewUtility;
        private void Init()
        {
            if (this.m_PreviewUtility == null)
            {
                this.m_PreviewUtility = new PreviewRenderUtility();
                this.m_PreviewUtility.cameraFieldOfView = 30f;
            }
        }

        void OnEnable()
        {
            Instance = this;
            asset = (SVGAsset)serializedObject.targetObject;

			anchorPositionContent = new GUIContent[anchorPosition.Length];
			for(int i = 0; i < anchorPosition.Length; i++)
			{
				anchorPositionContent[i] = new GUIContent(anchorPosition[i]);
			}

            if(serializedObject.isEditingMultipleObjects)
            {
                assets = new SVGAsset[serializedObject.targetObjects.Length];
                for(int i = 0; i < serializedObject.targetObjects.Length; i++)
                {
                    assets[i] = (SVGAsset)serializedObject.targetObjects[i];
                }
            }

            filesValid = true;
            if(serializedObject.isEditingMultipleObjects)
            {
                for(int i = 0; i < assets.Length; i++)
                {
                    if(string.IsNullOrEmpty(assets[i].svgFile))
                        filesValid = false;
                }
            } else {
                if(string.IsNullOrEmpty(asset.svgFile))
                    filesValid = false;
            }

            format = serializedObject.FindProperty("_format");
			meshCompression = serializedObject.FindProperty("_meshCompression");
            scale = serializedObject.FindProperty("_scale");
            vpm = serializedObject.FindProperty("_vpm");
            depthOffset = serializedObject.FindProperty("_depthOffset");
            compressDepth = serializedObject.FindProperty("_compressDepth");
            customPivotPoint = serializedObject.FindProperty("_customPivotPoint");
            pivotPoint = serializedObject.FindProperty("_pivotPoint");
            //border = serializedObject.FindProperty("_border");
            generateCollider = serializedObject.FindProperty("_generateCollider");
			keepSVGFile = serializedObject.FindProperty("_keepSVGFile");

            CreateSnapshot();
            unappliedChanges = false;
        }

        void OnDisable()
        {
            if(unappliedChanges)
            {
                string assetPaths = "";
                if(serializedObject.isEditingMultipleObjects)
                {
                    for(int i = 0; i < assets.Length; i++)
                    {
                        assetPaths += "'"+AssetDatabase.GetAssetPath(assets[i])+"'\n";
                    }
                } else {
                    assetPaths = AssetDatabase.GetAssetPath(asset);
                }

                if(EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for "+assetPaths, "Apply", "Revert"))
                {
                    ApplyChanges();
                } else {
                    RevertChanges();
                }
                unappliedChanges = false;
            }
            Instance = null;
        }

        void OnDestroy()
        {
            if (this.m_PreviewUtility != null)
            {
                this.m_PreviewUtility.Cleanup();
                this.m_PreviewUtility = null;
            }
            Instance = null;
        }

        public override void OnInspectorGUI()
        {
            Instance = this;
            if (filesValid)
            {
                OnFilesValid();
            } else
            {
                EditorGUILayout.HelpBox("File is invalid or corrupted!", MessageType.Error);
            }
        }

        public static int GetPivotPointIndex(Vector2 pivotPointVector)
        {
            return Mathf.RoundToInt(pivotPointVector.x * 2 + Mathf.Clamp(pivotPointVector.y * 6, 0, 8));
        }

        public static Vector2 GetPivotPoint(int index)
        {
            int x = index % 3;
            int y = Mathf.FloorToInt(index / 3);

            return new Vector2(x / 2f, y / 2f);
        }

        void OnFilesValid()
        {
			bool valueChanged = false;

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(format, new GUIContent("Format", "The rendering format of the SVG Asset."));
			EditorGUILayout.PropertyField(meshCompression, new GUIContent("Mesh Compression", "Reduce file size of the mesh, but might introduce irregularities and visual artefacts."));
			EditorGUILayout.PropertyField(scale, new GUIContent("Scale", "The scale of the mesh relative to the SVG Asset. Does not affect the quality of the mesh"));
			EditorGUILayout.PropertyField(vpm, new GUIContent("Quality", "Larger number means better but more complex mesh, Vertex Per Meter represents number of vertices in the SVG Asset that correspond to one unit in world space."));

            if(format.enumValueIndex == (int)SVGAssetFormat.Opaque)
            {
				EditorGUILayout.PropertyField(depthOffset, new GUIContent("Depth Offset", "The minimal z-offset in WorldSpace for Opaque Rendering."));
				EditorGUILayout.PropertyField(compressDepth, new GUIContent("Compress Depth", "Compresses the overlapping objects to reduce z-offset requirements."));			
            }

			EditorGUILayout.PropertyField(customPivotPoint, new GUIContent("Custom Pivot", "Choose the predefined pivot point or the custom pivot point."));
            if(customPivotPoint.boolValue)
            { 
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(pivotPoint, new GUIContent("Pivot", "The location of the SVG Asset center point in the original Rect, specified in percents."));				
				EditorGUILayout.EndHorizontal();
            } else {
                Vector2 pivotPointVector = pivotPoint.vector2Value;
                int selectedIndex = GetPivotPointIndex(pivotPointVector);
				selectedIndex = EditorGUILayout.Popup(new GUIContent("Pivot", "The location of the SVG Asset center point in the original Rect, specified in percents."), selectedIndex, anchorPositionContent);
                pivotPoint.vector2Value = GetPivotPoint(selectedIndex);
            }

            if(!serializedObject.isEditingMultipleObjects)
            {
                #if !UNITY_4_6
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("SVG Editor"))
                {
                    SVGEditorWindow.GetWindow();
                }
                EditorGUILayout.EndHorizontal();
                #endif
            }

            EditorGUILayout.PropertyField(generateCollider, new GUIContent("Generate Collider", "Automatically generates polygon colliders."));
			EditorGUILayout.PropertyField(keepSVGFile, new GUIContent("Keep SVG File", "Keep the SVG file in the final build. This increases the file size"));

			if(EditorGUI.EndChangeCheck())
			{
				valueChanged = true;
            }
            			
            GUILayout.BeginHorizontal();
			if(GUILayout.Button(new GUIContent("Recover SVG File", "Save the original SVG Document to a specified directory.")))
            {            
                if(serializedObject.isEditingMultipleObjects)
                {
                    for(int i = 0; i < assets.Length; i++)
                    {
                        RecoverSVGFile(assets[i]);
                    }
                } else {
                    RecoverSVGFile(asset);
                }
            }

			if(GUILayout.Button(new GUIContent("Save Mesh File", "Save the mesh asset to a specified directory.")))
            {            
                if(serializedObject.isEditingMultipleObjects)
                {
                    for(int i = 0; i < assets.Length; i++)
                    {
                        SaveMeshFile(assets[i]);
                    }
                } else {
                    SaveMeshFile(asset);
                }
            }

			if(valueChanged)
            {
                unappliedChanges = true;
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.EndHorizontal();

			EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.enabled = unappliedChanges && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;

			if(GUILayout.Button(new GUIContent("Revert", "Revert all changes.")))
            {
				RevertChanges();
            }

			if (GUILayout.Button(new GUIContent("Apply", "Apply all changes.")))
            {
                serializedObject.ApplyModifiedProperties();
                ApplyChanges();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            SVGError[] errors = GetEditorErrors(asset);
            if(errors != null && errors.Length > 0)
            {
                for(int i = 0; i < errors.Length; i++)
                {
                    switch(errors[i])
                    {
                        case SVGError.CorruptedFile:
                            EditorGUILayout.HelpBox("SVG file is corrupted", MessageType.Error);
                            break;
                        case SVGError.Syntax:
                            EditorGUILayout.HelpBox("SVG syntax is invalid", MessageType.Error);
                            break;
                        case SVGError.ClipPath:
                            EditorGUILayout.HelpBox("Clip paths are experimental", MessageType.Warning);
                            break;
                        case SVGError.Mask:
                            EditorGUILayout.HelpBox("Masks are not supported", MessageType.Warning);
                            break;
                        case SVGError.Symbol:
                            EditorGUILayout.HelpBox("Re-import for working symbols", MessageType.Warning);
                            break;
                        case SVGError.Image:
                            EditorGUILayout.HelpBox("Images are not supported", MessageType.Warning);
                            break;
                        case SVGError.Unknown:
                            EditorGUILayout.HelpBox("Unknow error occurred", MessageType.Error);
                            break;
                    }
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Report Import!")))
            {
                SVGReportBugWindow.titleField = "Wrong SVG file import!";
                SVGReportBugWindow.descriptionField = "My file was incorrectly imported. I believe that the file is not corrupted.";
                SVGReportBugWindow.problemOccurrence = SVGReportBugWindow.PROBLEM_OCCURRENCE.Always;
                SVGReportBugWindow.problemType = SVGReportBugWindow.PROBLEM_TYPE.FileImport;
                SVGReportBugWindow.AddSVGAttachment(asset.name, asset.svgFile);
                SVGReportBugWindow.ShowReportBugWindow();
            }
            GUILayout.EndHorizontal();
        }

        protected void RecoverSVGFile(SVGAsset recoverAsset)
        {
            string assetPath = AssetDatabase.GetAssetPath(recoverAsset);
            string lastPath = lastSVGRecoveryPath;
            if(string.IsNullOrEmpty(lastPath))
            {
                lastPath = assetPath;           
            } else {
                lastPath += "/";
            }
            string path = EditorUtility.SaveFilePanel("Recover SVG File", Path.GetDirectoryName(lastPath), Path.GetFileNameWithoutExtension(assetPath), "svg" );
            if(!string.IsNullOrEmpty(path))
            {
                lastSVGRecoveryPath = Path.GetDirectoryName(path);
                File.WriteAllText(path, recoverAsset.svgFile);
                EditorUtility.RevealInFinder(path);
            }
        }

        protected void SaveMeshFile(SVGAsset meshAsset)
        {
            string assetPath = AssetDatabase.GetAssetPath(meshAsset);
            string lastPath = lastMeshSavePath;
            if(string.IsNullOrEmpty(lastPath))
            {
                lastPath = assetPath;           
            } else {
                lastPath += "/";
            }
            string path = EditorUtility.SaveFilePanel("Save Mesh File", Path.GetDirectoryName(lastPath), Path.GetFileNameWithoutExtension(assetPath)+"-mesh", "asset" );
            if(!string.IsNullOrEmpty(path))
            {
                System.Uri assetFolderPath = new Uri(Application.dataPath);
                System.Uri outputPath = new Uri(path);
                path = assetFolderPath.MakeRelativeUri(outputPath).ToString();

                lastSVGRecoveryPath = Path.GetDirectoryName(path);

                Mesh sharedMesh = meshAsset.sharedMesh;
                Mesh mesh = new Mesh();
                mesh.name = sharedMesh.name;
                mesh.vertices = (Vector3[])sharedMesh.vertices.Clone();
                mesh.triangles = (int[])sharedMesh.triangles.Clone();
                if(sharedMesh.uv != null || sharedMesh.uv.Length > 0)
                    mesh.uv = (Vector2[])sharedMesh.uv.Clone();
                if(sharedMesh.colors32 != null || sharedMesh.colors32.Length > 0)
                    mesh.colors32 = (Color32[])sharedMesh.colors32.Clone();
                AssetDatabase.CreateAsset(mesh, path);
                EditorUtility.RevealInFinder(path);
            }
        }

        public void ApplyChanges()
        {
            if(serializedObject.isEditingMultipleObjects)
            {
                int importTotalAssets = assets.Length;
                int currentAssetIndex = 0;
                for(int i = 0; i < importTotalAssets; i++)
                {
                    string assetPath = AssetDatabase.GetAssetPath(assets[i]);
                    float importProgress = (float)currentAssetIndex / (float)importTotalAssets;
                    if(EditorUtility.DisplayCancelableProgressBar("Importing SVG Assets", "Importing SVG Asset: "+assetPath+"...", importProgress))
                    {
                        EditorUtility.ClearProgressBar();
                        break;
                    } else {
                        //assets[i]._editor_ApplyChanges();
                        MethodInfo _editor_ApplyChanges = typeof(SVGAsset).GetMethod("_editor_ApplyChanges", BindingFlags.NonPublic | BindingFlags.Instance);
                        _editor_ApplyChanges.Invoke(assets[i], new object[]{false});
                        currentAssetIndex++;
                    }
                }

                EditorUtility.ClearProgressBar();
            } else {
                //asset._editor_ApplyChanges();
                MethodInfo _editor_ApplyChanges = typeof(SVGAsset).GetMethod("_editor_ApplyChanges", BindingFlags.NonPublic | BindingFlags.Instance);
                _editor_ApplyChanges.Invoke(asset, new object[]{false});
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Canvas.ForceUpdateCanvases();
            UnityEditor.SceneView.RepaintAll();
            unappliedChanges = false;

            GUI.FocusControl(null);
            EditorGUI.FocusTextInControl(null);

            UpdateInstances(this.serializedObject);
        }

        private string Format(long ts)
        {
            return String.Format("{0} ms", ts);
        }

        public override bool HasPreviewGUI() { return this.target != null; }

        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    EditorGUI.DropShadowLabel(new Rect(rect.x, rect.y, rect.width, 40f), "Preview requires\nrender texture support");
                }
                return;
            }
            if (this.target == null)
            {
                return;
            }
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            this.Init();
            this.m_PreviewUtility.BeginPreview(rect, background);
            this.DoRenderPreview();
            Texture image = this.m_PreviewUtility.EndPreview();
            GUI.DrawTexture(rect, image, ScaleMode.StretchToFill, false);
        }
        
        public override void OnPreviewSettings()
        {
            if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            {
                return;
            }
            GUI.enabled = true;
            this.Init();
        }

		void DoRenderPreview()
        {
			SVGEditorWindow.DoRenderPreview(target as SVGAsset, m_PreviewUtility);
        }

        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            {
                return null;
            }
            if(width <= 20 || height <= 20)
            {
                Debug.Log("RenderStaticPreview");
                return SVGImporterEditor.settings.defaultSVGIcon;
            } else {
                this.Init();
                this.m_PreviewUtility.BeginStaticPreview(new Rect(0f, 0f, (float)width, (float)height));
                this.DoRenderPreview();
                return this.m_PreviewUtility.EndStaticPreview();
            }
        }

        public override string GetInfoString()
        {        
            string editorInfo = "";

            if(serializedObject.isEditingMultipleObjects)
            {
                int assetLength = assets.Length - 1;
                for(int i = 0; i < assetLength; i++)
                {
                    editorInfo += GetEditorInfo(assets[i]) + ", ";
                }

                editorInfo += GetEditorInfo(assets[assetLength]);;
            } else {
                editorInfo = GetEditorInfo(asset);
            }

            return editorInfo;
        }

		public void RevertChanges()
		{
			for(int i = 0; i < svgAssetSnapshots.Length; i++)
			{
				SerializedObject serializedObject = new SerializedObject(targets[i]);
				svgAssetSnapshots[i].ModifySerializedObject(serializedObject);
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();
			}

			this.serializedObject.SetIsDifferentCacheDirty();
			this.serializedObject.Update();
			unappliedChanges = false;

			GUI.FocusControl(null);
			EditorGUI.FocusTextInControl(null);

            UpdateInstances(this.serializedObject);
		}

		public void CreateSnapshot()
		{			
			if(serializedObject.isEditingMultipleObjects)
			{
				int targetLength = serializedObject.targetObjects.Length;
				svgAssetSnapshots = new SVGAssetSnapshot[targetLength];
				for(int i = 0; i < targetLength; i++)
				{
					svgAssetSnapshots[i] = new SVGAssetSnapshot((SVGAsset)targets[i]);
				}
			} else {
				svgAssetSnapshots = new SVGAssetSnapshot[]{ new SVGAssetSnapshot(serializedObject) };
			}
		}

        protected string GetEditorInfo(SVGAsset asset)
        {
            PropertyInfo _editor_Info = typeof(SVGAsset).GetProperty("_editor_Info", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)_editor_Info.GetValue(asset, new object[0]);
        }

        protected SVGError[] GetEditorErrors(SVGAsset asset)
        {
            PropertyInfo _editor_errors = typeof(SVGAsset).GetProperty("_editor_errors", BindingFlags.NonPublic | BindingFlags.Instance);
            return (SVGError[])_editor_errors.GetValue(asset, new object[0]);
        }


        public static void UpdateInstances(SerializedObject serializedObject)
        {            
            if(serializedObject == null)
                return;

            if(serializedObject.targetObjects != null && serializedObject.targetObjects.Length > 0)
            {
                SVGAsset[] svgAssets = new SVGAsset[serializedObject.targetObjects.Length];
                for(int i = 0; i < svgAssets.Length; i++)
                {
                    svgAssets[i] = serializedObject.targetObjects[i] as SVGAsset;
                }
                
                UpdateInstances(svgAssets);
            }
        }

        public static void UpdateInstances(SVGAsset[] svgAssets)
        {
            if(svgAssets == null || svgAssets.Length == 0)
                return;

            SVGImage[] svgImages = Resources.FindObjectsOfTypeAll<SVGImage>();
            if(svgImages != null && svgImages.Length > 0)
            {
                for(int i = 0; i < svgImages.Length; i++)
                {
                    if(AssetDatabase.Contains(svgImages[i]))
                        continue;

                    for(int j = 0; j < svgAssets.Length; j++)
                    {
                        if(svgAssets[j] == null)
                            continue;

                        if(svgImages[i] == null || svgImages[i].vectorGraphics != svgAssets[j]) continue;

                        typeof(SVGImage).GetMethod("Clear", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(svgImages[i], null);
                        typeof(SVGImage).GetMethod("UpdateMaterial", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(svgImages[i], null);
                        svgImages[i].SetAllDirty();
                    }
                }
            }
        }
    }
}

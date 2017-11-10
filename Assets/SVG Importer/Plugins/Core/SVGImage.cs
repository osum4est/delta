// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//#define DEBUG_MATERIALS

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

using System.Collections;
using System.Collections.Generic;

namespace SVGImporter
{
    using Utils;

    [ExecuteInEditMode]
    [AddComponentMenu("UI/SVG Image", 21)]
    public class SVGImage : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        public enum Type
        {
            Simple,
            Sliced
        }

        [FormerlySerializedAs("vectorGraphics")]
        [SerializeField]
        protected SVGAsset _vectorGraphics;
        protected SVGAsset _lastVectorGraphics;
        public SVGAsset vectorGraphics
        {
            get {
                return _vectorGraphics;
            }
            set {
                if(SVGPropertyUtility.SetClass(ref _vectorGraphics, value))
                {
                    Clear();
                    UpdateMaterial();
                    SetAllDirty();
                }
            }
        }

        /// How the Image is drawn.
        [SerializeField] private Type m_Type = Type.Simple;
        public Type type { get { return m_Type; } set { if (SVGPropertyUtility.SetStruct(ref m_Type, value)) SetVerticesDirty(); } }

        [SerializeField] private bool m_PreserveAspect = false;
        public bool preserveAspect { get { return m_PreserveAspect; } set { if (SVGPropertyUtility.SetStruct(ref m_PreserveAspect, value)) SetVerticesDirty(); } }

        [SerializeField] private bool m_UsePivot = false;
        public bool usePivot { get { return m_UsePivot; } set { if (SVGPropertyUtility.SetStruct(ref m_UsePivot, value)) SetVerticesDirty(); } }

        // Not serialized until we support read-enabled sprites better.
        private float m_EventAlphaThreshold = 1;
        public float eventAlphaThreshold { get { return m_EventAlphaThreshold; } set { m_EventAlphaThreshold = value; } }
#if DEBUG_MATERIALS
        public Material maskMaterialTest;
        public Material defaultMaterialTest;
        public Material currenttMaterialTest;
        public Material renderMaterialTest;
#endif

        protected Material _defaultMaterial;

        protected SVGImage()
        { }

        protected override void Awake()
        {
#if UNITY_EDITOR
            Clear();
#endif
            UpdateMaterial();
            base.Awake();
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            Clear();
            _vectorGraphics = null;
            UpdateMaterial();
            base.Reset();
        }

        protected void Refresh()
        {
            Clear();
            UpdateMaterial();
        }
#endif

        /// <summary>
        /// Whether the Image has a border to work with.
        /// </summary>
        
        public bool hasBorder
        {
            get
            {
                if (vectorGraphics != null)
                {
                    return vectorGraphics.border.sqrMagnitude > 0f;
                }
                return false;
            }
        }

        public float pixelsPerUnit
        {
            get
            {
                float spritePixelsPerUnit = 100;
                return spritePixelsPerUnit;
            }
        }

        protected Mesh sharedMesh
        {
            get {
                if(_vectorGraphics == null)
                    return null;
                return _vectorGraphics.sharedMesh;
            }
        }

        /// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            Vector2 size = sharedMesh == null ? Vector2.zero : (Vector2)sharedMesh.bounds.size;
            
            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                var spriteRatio = size.x / size.y;
                var rectRatio = r.width / r.height;
                
                if (spriteRatio > rectRatio)
                {
                    var oldHeight = r.height;
                    r.height = r.width * (1.0f / spriteRatio);
                    r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                }
                else
                {
                    var oldWidth = r.width;
                    r.width = r.height * spriteRatio;
                    r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                }
            }

            return new Vector4(
                r.x,
                r.y,
                r.width,
                r.height
                );
        }

        public override void SetNativeSize()
        {
            if (sharedMesh != null)
            {
                Bounds bounds = sharedMesh.bounds;
                Vector2 size = bounds.size * 1000f;
                float w = size.x / pixelsPerUnit;
                float h = size.y / pixelsPerUnit;
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
                SetAllDirty();
            }
        }
#if DEBUG_MATERIALS
        public override Material materialForRendering
        {
            get
            {
                renderMaterialTest = base.materialForRendering;
                return renderMaterialTest;
            }
        }

        public override Material material
        {
            get
            {
                currenttMaterialTest = base.material;
                maskMaterialTest = this.m_MaskMaterial;
                //DebugMaterial(currenttMaterialTest);
                return currenttMaterialTest;
            }
            set
            {
                base.material = value;
            }
        }

#endif


        public override Material defaultMaterial
        {
            get
            {
                GetDefaultMaterial();
                return _defaultMaterial;
            }
        }

        protected float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {               
                value -= from;
                value /= to - from;
                return value;
            }
            else
            {
                return 1f - (value - to) / (from - to);
            }
        }

        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }
        
        public virtual float minWidth { get { return 0; } }
        
        public virtual float preferredWidth
        {
            get
            {
                if (sharedMesh == null)
                    return 0;
                Bounds bounds = sharedMesh.bounds;
                return bounds.size.x / pixelsPerUnit;
            }
        }
        
        public virtual float flexibleWidth { get { return -1; } }
        
        public virtual float minHeight { get { return 0; } }
        
        public virtual float preferredHeight
        {
            get
            {
                if (sharedMesh == null)
                    return 0;
                Bounds bounds = sharedMesh.bounds;
                return bounds.size.y / pixelsPerUnit;
            }
        }
        
        public virtual float flexibleHeight { get { return -1; } }
        
        public virtual int layoutPriority { get { return 0; } }
        
        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (m_EventAlphaThreshold >= 1)
                return true;

            if (sharedMesh == null)
                return true;

            return true;
        }
        
        private Vector2 MapCoordinate(Vector2 local, Rect rect)
        {
            Bounds bounds = sharedMesh.bounds;
            return new Vector2(local.x * bounds.size.x / rect.width, local.y * bounds.size.y / rect.height);
        }

        public override void SetMaterialDirty()
        {
            if (this.IsActive())
            {
                UpdateGradientShape(m_Material);
            }

            base.SetMaterialDirty();
        }  

        protected void UpdateGradientShape(Material material)
        {
            if(material != null)
            {
                if(material.HasProperty("_GradientShape"))
                {
                    material.SetTexture("_GradientShape", SVGAtlas.gradientShapeTexture);
                }
            }
        }

        const float epsilon = 0.0000001f;
#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1
        UIVertex[] tempVBO;
        protected override void OnFillVBO(List<UIVertex> vbo)
        {
            if (sharedMesh == null) { base.OnFillVBO(vbo); return; }
            Bounds bounds = sharedMesh.bounds;
            if(m_UsePivot)
            {             
                bounds.center += new Vector3((-0.5f + _vectorGraphics.pivotPoint.x) * bounds.size.x, (0.5f - _vectorGraphics.pivotPoint.y) * bounds.size.y, 0f);
            }
            
            tempVBO = (UIVertex[])_vectorGraphics.sharedUIMesh.Clone();
            int tempVBOLength = tempVBO.Length;
            
            if(m_Type == Type.Simple)
            {
                Vector4 v = GetDrawingDimensions(preserveAspect);
                for(int i = 0; i < tempVBOLength; i++)
                {
                    tempVBO[i].position.x = v.x + InverseLerp(bounds.min.x, bounds.max.x, tempVBO[i].position.x) * v.z;
                    tempVBO[i].position.y = v.y + InverseLerp(bounds.min.y, bounds.max.y, tempVBO[i].position.y) * v.w;
                    tempVBO[i].color *= color;
                }
            } else {
                
                Vector4 v = GetDrawingDimensions(false);
                Vector2 normalizedPosition;
                
                // LEFT, BOTTOM, RIGHT, TOP
                Vector4 border = _vectorGraphics.border;
                
                Vector4 borderCalc = new Vector4(border.x + epsilon, border.y + epsilon, 1f - border.z - epsilon, 1f - border.w - epsilon);
                
                float left = v.x;
                float top = v.y;
                float right = v.x + v.z;
                float bottom = v.y + v.w;
                float size = canvas.referencePixelsPerUnit * vectorGraphics.scale * 100f;
                
                float minWidth = (border.x + border.z) * size;
                float minHeight = (border.y + border.w) * size;
                float scaleX = 0f; if(minWidth != 0f) scaleX = Mathf.Clamp01(v.z / minWidth);
                float scaleY = 0f; if(minHeight != 0f) scaleY = Mathf.Clamp01(v.w / minHeight);
                
                for(int i = 0; i < tempVBOLength; i++)
                {
                    normalizedPosition.x = InverseLerp(bounds.min.x, bounds.max.x, tempVBO[i].position.x);
                    normalizedPosition.y = InverseLerp(bounds.min.y, bounds.max.y, tempVBO[i].position.y);
                    
                    if(normalizedPosition.x <= borderCalc.x && border.x != 0f)
                    {
                        tempVBO[i].position.x = left + normalizedPosition.x * size * scaleX;
                    } else if(normalizedPosition.x >= borderCalc.z && border.z != 0f)
                    {
                        tempVBO[i].position.x = right - (1f - normalizedPosition.x) * size * scaleX;
                    } else {
                        tempVBO[i].position.x = v.x + normalizedPosition.x * v.z;
                    }
                    
                    if(normalizedPosition.y >= borderCalc.w && border.w != 0f)
                    {
                        tempVBO[i].position.y = bottom - (1f - normalizedPosition.y) * size * scaleY;
                    } else if(normalizedPosition.y <= borderCalc.y && border.y != 0f)
                    {
                        tempVBO[i].position.y = top + normalizedPosition.y * size * scaleY;
                    } else {
                        tempVBO[i].position.y = v.y + normalizedPosition.y * v.w;
                    }
                    
                    tempVBO[i].color *= color;
                }
            }
            
            vbo.AddRange(tempVBO);
        }

        protected void GetDefaultMaterial()
        {
            if(_lastVectorGraphics != _vectorGraphics)
            {
                _lastVectorGraphics = _vectorGraphics;
                Clear();
            }

            if(_vectorGraphics != null)
            {
                #if UNITY_EDITOR
                if(_defaultMaterial == null)
                {
                    if(!UnityEditor.EditorApplication.isPlaying)
                    {
                        _defaultMaterial = _vectorGraphics.uiMaskMaterial;
                        SetHideFlags(_defaultMaterial, HideFlags.DontSave);
                    } else {
                        if(!this.m_IncludeForMasking)
                        {
                            _defaultMaterial = _vectorGraphics.sharedUIMaskMaterial;
                        } else {
                            _defaultMaterial = _vectorGraphics.uiMaskMaterial;
                        }
                    }
                }
                #else
                if(_defaultMaterial == null)
                {
                    if(!this.m_IncludeForMasking)
                    {
                        _defaultMaterial = _vectorGraphics.sharedUIMaskMaterial;
                    } else {
                        _defaultMaterial = _vectorGraphics.uiMaskMaterial;
                    }
                }
                #endif
            }
        }

        
        protected void Clear()
        {
            #if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                if(_defaultMaterial != null)
                {
                    if (this.m_IncludeForMasking)
                    {
                        if(_defaultMaterial != m_MaskMaterial)
                        {
                            DestroyObjectInternal(m_MaskMaterial);
                        }
                    }
                    
                    DestroyObjectInternal(_defaultMaterial);
                }
            }
            #endif
            
            _defaultMaterial = null;
        }
#else
        protected override void OnPopulateMesh(Mesh toFill)
        {
            if (sharedMesh == null) { base.OnPopulateMesh(toFill); return; }
            Bounds bounds = sharedMesh.bounds;
            if(m_UsePivot)
            {             
                bounds.center += new Vector3((-0.5f + _vectorGraphics.pivotPoint.x) * bounds.size.x, (0.5f - _vectorGraphics.pivotPoint.y) * bounds.size.y, 0f);
            }
            
            toFill.Clear();
            toFill.vertices = (Vector3[])sharedMesh.vertices.Clone();
            toFill.triangles = (int[])sharedMesh.triangles.Clone();
            toFill.uv = (Vector2[])sharedMesh.uv.Clone();
            toFill.uv2 = (Vector2[])sharedMesh.uv2.Clone();
            toFill.colors32 = (Color32[])sharedMesh.colors32.Clone();
            //toFill.bounds = sharedMesh.bounds;

            int tempVBOLength = toFill.vertexCount;
            
            Vector3[] vertices = toFill.vertices;
            Vector3[] originalVertices = sharedMesh.vertices;
            Color32[] colors = toFill.colors32;
            Color32[] originalColors = sharedMesh.colors32;
            
            if(m_Type == Type.Simple)
            {
                Vector4 v = GetDrawingDimensions(preserveAspect);
                for(int i = 0; i < tempVBOLength; i++)
                {
                    vertices[i].x = v.x + InverseLerp(bounds.min.x, bounds.max.x, originalVertices[i].x) * v.z;
                    vertices[i].y = v.y + InverseLerp(bounds.min.y, bounds.max.y, originalVertices[i].y) * v.w;
                    colors[i] = originalColors[i] * color;
                }
            } else {
                
                Vector4 v = GetDrawingDimensions(false);
                Vector2 normalizedPosition;
                
                // LEFT, BOTTOM, RIGHT, TOP
                Vector4 border = _vectorGraphics.border;
                
                Vector4 borderCalc = new Vector4(border.x + epsilon, border.y + epsilon, 1f - border.z - epsilon, 1f - border.w - epsilon);
                
                float left = v.x;
                float top = v.y;
                float right = v.x + v.z;
                float bottom = v.y + v.w;
                float size = canvas.referencePixelsPerUnit * vectorGraphics.scale * 100f;
                
                float minWidth = (border.x + border.z) * size;
                float minHeight = (border.y + border.w) * size;
                float scaleX = 0f; if(minWidth != 0f) scaleX = Mathf.Clamp01(v.z / minWidth);
                float scaleY = 0f; if(minHeight != 0f) scaleY = Mathf.Clamp01(v.w / minHeight);
                
                for(int i = 0; i < tempVBOLength; i++)
                {
                    normalizedPosition.x = InverseLerp(bounds.min.x, bounds.max.x, originalVertices[i].x);
                    normalizedPosition.y = InverseLerp(bounds.min.y, bounds.max.y, originalVertices[i].y);
                    
                    if(normalizedPosition.x <= borderCalc.x && border.x != 0f)
                    {
                        vertices[i].x = left + normalizedPosition.x * size * scaleX;
                    } else if(normalizedPosition.x >= borderCalc.z && border.z != 0f)
                    {
                        vertices[i].x = right - (1f - normalizedPosition.x) * size * scaleX;
                    } else {
                        vertices[i].x = v.x + normalizedPosition.x * v.z;
                    }
                    
                    if(normalizedPosition.y >= borderCalc.w && border.w != 0f)
                    {
                        vertices[i].y = bottom - (1f - normalizedPosition.y) * size * scaleY;
                    } else if(normalizedPosition.y <= borderCalc.y && border.y != 0f)
                    {
                        vertices[i].y = top + normalizedPosition.y * size * scaleY;
                    } else {
                        vertices[i].y = v.y + normalizedPosition.y * v.w;
                    }
                    
                    colors[i] = originalColors[i] * color;
                }
            }
            
            toFill.vertices = vertices;
            toFill.colors32 = colors;
            toFill.RecalculateBounds();
        }

        protected void GetDefaultMaterial()
        {
            if(_lastVectorGraphics != _vectorGraphics)
            {               
                _lastVectorGraphics = _vectorGraphics;
                Clear();
            }
            
            if(_vectorGraphics != null)
            {
                #if UNITY_EDITOR
                if(_defaultMaterial == null)
                {
                    if(!UnityEditor.EditorApplication.isPlaying)
                    {
                        _defaultMaterial = _vectorGraphics.uiMaskMaterial;
                        SetHideFlags(_defaultMaterial, HideFlags.DontSave);
                    } else {
                        _defaultMaterial = _vectorGraphics.sharedUIMaskMaterial;
                    }
                }
                #else
                if(_defaultMaterial == null)
                {
                    _defaultMaterial = _vectorGraphics.sharedUIMaskMaterial;
                }
                #endif
            }
        }

        protected void Clear()
        {
            #if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                if(_defaultMaterial != null)
                {
                    DestroyObjectInternal(_defaultMaterial);
                }
            }
            #endif
            
            _defaultMaterial = null;
        }
#endif
        
        protected override void UpdateMaterial()
        {
            GetDefaultMaterial();
			base.UpdateMaterial();
        }

        void DestroyObjectInternal(Object obj)
        {
            if(obj == null)
                return;
            
            #if UNITY_EDITOR
            if(!UnityEditor.AssetDatabase.Contains(obj))
            {
                if(UnityEditor.EditorApplication.isPlaying)
                {
                    Destroy(obj);
                } else {
                    DestroyImmediate(obj);
                }
            }
            #else
            Destroy(obj);
            #endif
        }

        void SetHideFlags(UnityEngine.Object target, HideFlags hideFlags)
        {
            if(target == null) return;
            target.hideFlags = hideFlags;
        }
        
        void SetHideFlags(UnityEngine.Object[] target, HideFlags hideFlags)
        {
            if(target == null || target.Length == 0) return;
            for(int i = 0; i < target.Length; i++)
            {
                target[i].hideFlags = hideFlags;
            }
        }
        
        #if DEBUG_MATERIALS && UNITY_EDITOR
        protected void DebugMaterial(Material material)
        {
            if(material == null)
                return;
            if(UnityEditor.Selection.activeGameObject == gameObject)
            {
                Debug.Log("defaultMaterial");
                Debug.Log("Shader Name: "+material.shader.name);            
                Debug.Log("_StencilComp: "+material.GetFloat("_StencilComp"));
                Debug.Log("_Stencil: "+material.GetFloat("_Stencil"));
                Debug.Log("_StencilOp: "+material.GetFloat("_StencilOp"));
                Debug.Log("_StencilWriteMask: "+material.GetFloat("_StencilWriteMask"));
                Debug.Log("_StencilReadMask: "+material.GetFloat("_StencilReadMask"));
                Debug.Log("_ColorMask: "+material.GetFloat("_ColorMask"));
            }
        }
        #endif
    }
}

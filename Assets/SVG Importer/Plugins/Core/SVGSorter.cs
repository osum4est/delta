// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;

namespace SVGImporter
{
    using Utils;
    using Rendering;

    [ExecuteInEditMode]
    [AddComponentMenu("Rendering/SVG Sorter", 20)]
    public class SVGSorter : MonoBehaviour {

        public float depthOffset = -0.01f;
        public int layerIndex = 0;
        public bool sort = true;

        float zOffsetStart;
        int layerIndexStart;

    	void LateUpdate()
        {
            if(sort)
            {
                zOffsetStart = transform.position.z;
                SortRecursive(transform, ref zOffsetStart, ref layerIndexStart);
            }
        }

        void SortRecursive(Transform transform, ref float zOffset, ref int layerIndex)
        {
            int childCount = transform.childCount;
            Transform child;
            SVGRenderer renderer;
            SVGAsset vectorGraphics;
            Bounds bounds;
            Vector3 position;
            for(int i = 0; i < childCount; i++)
            {
                child = transform.GetChild(i);
                renderer = child.GetComponent<SVGRenderer>();
                if(renderer.overrideSorter) continue;
                vectorGraphics = renderer.vectorGraphics;
                if(vectorGraphics == null)
                    continue;

                bounds = vectorGraphics.bounds;
                position = renderer.transform.position;
                position.z = zOffset;
                renderer.transform.position = position;
                zOffset += bounds.size.z + depthOffset;
                renderer.sortingOrder = layerIndex++;

                SortRecursive(child, ref zOffset, ref layerIndex);
            }
        }
    }
}

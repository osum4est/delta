// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;

namespace SVGImporter
{
    public class SVGImporterSettings : ScriptableObject 
    {
        protected static string _version = "1.0.8";
        public static string version
        {
            get {
                return _version;
            }
        }

        public SVGAssetFormat defaultSVGFormat = SVGAssetFormat.Transparent;
        public SVGMeshCompression defaultMeshCompression = SVGMeshCompression.Off;
        public int defaultVerticesPerMeter = 1000;
        public float defaultScale = 0.01f;
        public float defaultDepthOffset = 0.01f;
        public bool defaultCompressDepth = true;
        public bool defaultCustomPivotPoint = false;
        public Vector2 defaultPivotPoint = new Vector2(0.5f, 0.5f);
        public bool defaultGenerateCollider = false;
        public bool defaultKeepSVGFile = true;
        public Texture2D defaultSVGIcon;

        public bool ignoreImportExceptions = true;
    }

}
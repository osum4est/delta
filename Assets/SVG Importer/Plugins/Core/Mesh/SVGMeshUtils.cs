// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SVGImporter.Utils
{
    public class SVGMeshUtils {
    	
    	public enum ColorChannel
    	{
    		RED,
    		GREEN,
    		BLUE,
    		ALPHA
    	}
    	
    	const float PI2 = Mathf.PI * 2;
    	
        public static Mesh Quad()
        {
            return Quad(new Vector2(1f, 1f));
        }
        
        public static Mesh Quad(float size)
        {
            return Quad(new Vector2(size, size));
        }
        
        public static Mesh Quad(Vector2 size)
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[6];
            Vector2[] uv = new Vector2[4];
            Color32[] colors32 = new Color32[4];
            
            vertices[0] = new Vector3(-size.x, size.y, 0f);
            vertices[1] = new Vector3(size.x, size.y, 0f);
            vertices[2] = new Vector3(-size.x, -size.y, 0f);
            vertices[3] = new Vector3(size.x, -size.y, 0f);
            
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;
            
            uv[0] = new Vector2(0f, 1f);
            uv[1] = new Vector2(1f, 1f);
            uv[2] = new Vector2(0f, 0f);
            uv[3] = new Vector2(1f, 0f);
            
            colors32[0] = Color.black;
            colors32[1] = Color.black;
            colors32[2] = Color.black;
            colors32[3] = Color.black;
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.colors32 = colors32;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
            return mesh;
        }
        
        public static Mesh Quad(Vector2 size, int hSegments, int vSegments)
        {
            return Quad(size, hSegments, vSegments, Vector3.zero);
        }
        
        public static Mesh Quad (Vector2 size, int hSegments, int vSegments, Vector3 anchorOffset, Color32 color)
        {
            Mesh m = new Mesh ();
            
            if (hSegments < 1)
                hSegments = 1;
            if (vSegments < 1)
                vSegments = 1;
            
            int widthSegments = hSegments, lengthSegments = vSegments;
            int hCount2 = widthSegments + 1;
            int vCount2 = lengthSegments + 1;
            int numTriangles = widthSegments * lengthSegments * 6;
            int numVertices = hCount2 * vCount2;
            
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            Color32[] colors = new Color32[numVertices];
            int[] triangles = new int[numTriangles];
            
            int index = 0;
            float uvFactorX = 1.0f / widthSegments;
            float uvFactorY = 1.0f / lengthSegments;
            float scaleX = size.x / widthSegments;
            float scaleY = size.y / lengthSegments;
            for (float y = 0.0f; y < vCount2; y++) {
                for (float x = 0.0f; x < hCount2; x++) {
                    vertices [index] = new Vector3 (x * scaleX - size.x / 2f + anchorOffset.x, y * scaleY - size.y / 2f + anchorOffset.y, anchorOffset.z);
                    colors [index] = color;
                    uvs [index++] = new Vector2 (x * uvFactorX, y * uvFactorY);             
                }
            }
            
            index = 0;
            for (int y = 0; y < lengthSegments; y++) {
                for (int x = 0; x < widthSegments; x++) {
                    triangles [index] = (y * hCount2) + x;
                    triangles [index + 1] = ((y + 1) * hCount2) + x;
                    triangles [index + 2] = (y * hCount2) + x + 1;
                    
                    triangles [index + 3] = ((y + 1) * hCount2) + x;
                    triangles [index + 4] = ((y + 1) * hCount2) + x + 1;
                    triangles [index + 5] = (y * hCount2) + x + 1;
                    index += 6;
                }
            }
            
            m.vertices = vertices;
            m.uv = uvs;
            m.colors32 = colors;
            m.triangles = triangles;
            m.RecalculateNormals ();
            m.RecalculateBounds ();
            
            return m;
        }
        
        public static Mesh Quad (Vector2 size, int hSegments, int vSegments, Vector3 anchorOffset)
        {
            return Quad (size, hSegments, vSegments, anchorOffset, (Color32)Color.white);
        }

    	public static Mesh Circle (int circuitSegments, Matrix4x4 meshTransform, Matrix4x4 uvTransform)
    	{
    		circuitSegments = Mathf.Clamp (circuitSegments, 4, int.MaxValue) + 1;
    		int circuitSegmentsMinusOne = circuitSegments - 1;
    		
    		Mesh m = new Mesh ();
    		int totalSegments = circuitSegments + 1;
    		int totalTriangles = circuitSegments * 3;
    		Vector3[] vertices = new Vector3[totalSegments];
    		Vector2[] uv = new Vector2[totalSegments];
    		int[] triangles = new int[totalTriangles];
    		
    		float cs, x, y;
    		Vector2 uvwPoint;
    		
    		for (int i = 0; i < circuitSegments; i++) {
    			cs = (float)i / (float)circuitSegmentsMinusOne;
    			x = Mathf.Cos (cs * PI2) * 0.5f;
    			y = Mathf.Sin (cs * PI2) * 0.5f;
    			
    			vertices [i].x = x;
    			vertices [i].y = y;
    			vertices [i] = meshTransform.MultiplyPoint (vertices [i]);
    			vertices [i].z = 0f;
    			
    			uvwPoint.x = x;
    			uvwPoint.y = y;
    		
                uvwPoint = uvTransform.MultiplyPoint(uvwPoint);
    			uv [i].x = uvwPoint.x + 0.5f;
    			uv [i].y = uvwPoint.y + 0.5f;
    		}

    		vertices [circuitSegments] = meshTransform.MultiplyPoint (vertices [circuitSegments]);
    				
    		uvwPoint.x = uvwPoint.y = 0f;		
            uvwPoint = uvTransform.MultiplyPoint(uvwPoint);
    		uv [circuitSegments].x = uvwPoint.x + 0.5f;
    		uv [circuitSegments].y = uvwPoint.y + 0.5f;
    		
    		int segmentIndex = 0;
    		for (int i = 0; i < totalTriangles; i += 3) {			
    			triangles [i] = segmentIndex;
    			triangles [i + 2] = segmentIndex + 1;
    			triangles [i + 1] = circuitSegments;	
    			segmentIndex++;
    		}
    		
    		m.vertices = vertices;
    		m.uv = uv;
    		m.triangles = triangles;
    		m.RecalculateBounds ();
    		
    		return m;
    	}
    	
        public static Mesh Rectangle (Matrix4x4 meshTransform, Matrix4x4 uvTransform)
    	{
    		Mesh mesh = new Mesh ();
    		Vector3[] vertices = new Vector3[4];
    		int[] triangles = new int[6];
    		Vector2[] uv = new Vector2[4];
    		Vector2 uvwPoint;
    		
    		vertices [0].x = -0.5f;
    		vertices [0].y = 0.5f;				
    		vertices [0] = meshTransform.MultiplyPoint (vertices [0]);
    		vertices [0].z = 0f;
    		
    		vertices [1].x = 0.5f;
    		vertices [1].y = 0.5f;				
    		vertices [1] = meshTransform.MultiplyPoint (vertices [1]);
    		vertices [1].z = 0f;
    		
    		vertices [2].x = -0.5f;
    		vertices [2].y = -0.5f;					
    		vertices [2] = meshTransform.MultiplyPoint (vertices [2]);
    		vertices [2].z = 0f;
    		
    		vertices [3].x = 0.5f;
    		vertices [3].y = -0.5f;		
    		vertices [3] = meshTransform.MultiplyPoint (vertices [3]);
    		vertices [3].z = 0f;
    		
    		triangles [0] = 0;
    		triangles [1] = 1;
    		triangles [2] = 2;
    		triangles [3] = 1;
    		triangles [4] = 3;
    		triangles [5] = 2;
    		
    		uv [0].x = -0.5f;
    		uv [0].y = 0.5f;
    		
            uvwPoint = uvTransform.MultiplyPoint(new Vector3 (uv [0].x, uv [0].y, 0f));
    		uv [0].x = uvwPoint.x + 0.5f;
    		uv [0].y = uvwPoint.y + 0.5f;

    		uv [1].x = 0.5f;
    		uv [1].y = 0.5f;
    		
            uvwPoint = uvTransform.MultiplyPoint(new Vector3 (uv [1].x, uv [1].y, 0f));
    		uv [1].x = uvwPoint.x + 0.5f;
    		uv [1].y = uvwPoint.y + 0.5f;
    		
    		uv [2].x = -0.5f;
    		uv [2].y = -0.5f;
    		
            uvwPoint = uvTransform.MultiplyPoint(new Vector3 (uv [2].x, uv [2].y, 0f));
    		uv [2].x = uvwPoint.x + 0.5f;
    		uv [2].y = uvwPoint.y + 0.5f;
    		
    		uv [3].x = 0.5f;
    		uv [3].y = -0.5f;

            uvwPoint = uvTransform.MultiplyPoint(new Vector3 (uv [3].x, uv [3].y, 0f));
    		uv [3].x = uvwPoint.x + 0.5f;
    		uv [3].y = uvwPoint.y + 0.5f;
    		
    		mesh.vertices = vertices;
    		mesh.triangles = triangles;
    		mesh.uv = uv;
    		mesh.RecalculateBounds ();

    		return mesh;
    	}

        public static Mesh Rectangle ()
        {
            return Rectangle(Matrix4x4.identity, Matrix4x4.identity);
        }

        public static Mesh Line(int tessellation, Vector3[] positions, Color32 color, float size = 1f, bool closeLine = false)
        {
            return Line(tessellation, positions, SVGArrayUtils.CreatePreinitializedArray<Color32>(color, positions.Length), SVGArrayUtils.CreatePreinitializedArray<float>(size, positions.Length), null, closeLine);
        }

        public static Mesh Line(int tessellation, Vector2[] positions, Color32 color, float size = 1f, bool closeLine = false)
        {
            Vector3[] newPositions = new Vector3[positions.Length];
            for(int i = 0; i < positions.Length; i++)
            {
                newPositions[i].x = positions[i].x;
                newPositions[i].y = positions[i].y;
            }
            return Line(tessellation, newPositions, SVGArrayUtils.CreatePreinitializedArray<Color32>(color, positions.Length), SVGArrayUtils.CreatePreinitializedArray<float>(size, positions.Length), null, closeLine);
        }

        public static Mesh Line(int tessellation, Vector2[] positions, Color32[] colors = null, float[] sizes = null, Vector3[] rotations = null, bool closeLine = false)
        {
            Vector3[] newPositions = new Vector3[positions.Length];
            for(int i = 0; i < positions.Length; i++)
            {
                newPositions[i].x = positions[i].x;
                newPositions[i].y = positions[i].y;
            }
            return Line(tessellation, newPositions, colors, sizes, rotations, closeLine);
        }

        public static Vector2 lineUVScale = Vector2.one;
        public static Vector2 lineUVOffset = Vector2.zero;
        public static void ResetLineSettings()
        {
            lineUVScale = Vector2.one;
            lineUVOffset = Vector2.zero;
        }
        public static Mesh Line(int tessellation, Vector3[] positions, Color32[] colors = null, float[] sizes = null, Vector3[] rotations = null, bool closeLine = false)
        {       
            if (positions == null)
            {
#if DEBUG
                Debug.LogWarning("Line / Array is Null!");
#endif
                ResetLineSettings();
                return null;
            }       
            
            if (positions.Length < 2)
            {
#if DEBUG
                Debug.LogWarning("Line / We need at least 2 positions!");
#endif
                ResetLineSettings();
                return null;
            }

            if (tessellation < 1)
                tessellation = 1;
            
            int _tessellation = tessellation * 2;
            float _tessellationProgress;
            
            int _vertexCount = positions.Length * _tessellation;
            int _positionsLength = positions.Length;
            int totalTriangles = (_positionsLength - 1) * (_tessellation - 1) * 6;

            bool useColors = (colors != null && colors.Length == _positionsLength);
            bool useRotations = (rotations != null && rotations.Length == _positionsLength);

            Vector3[] _vertices = new Vector3[_vertexCount];       
            int[] _triangles = new int[totalTriangles];

            Color32[] _colors = null;
            if(useColors)
                _colors = new Color32[_vertexCount];        

            if (sizes == null)
                sizes = SVGArrayUtils.CreatePreinitializedArray<float>(1f, _vertexCount);

            Vector2[] _uvs = new Vector2[_vertexCount];
            Vector3[] normals = new Vector3[_positionsLength];
            float[] magnitudes = new float[_positionsLength];

            int i = 0, j = 0, ij = 0, positionIndex;       
            Vector3 lastPosition = positions [0], currentPosition = positions [0];
			float magnitude = 0f, size = 1f, totalCurveLength = 0f;//currentMagnitude = 0f, 
            Color32 color = Color.white;

            // Pre-Processing
            for (i = 0; i < _positionsLength; i++)
            {
                // Normal direction
                normals [i].x = positions [i].x - lastPosition.x;
                normals [i].y = positions [i].y - lastPosition.y;
                normals [i].z = positions [i].z - lastPosition.z;
                
                // Normal magnitude
                magnitudes [i] = Mathf.Sqrt(normals [i].x * normals [i].x + normals [i].y * normals [i].y + normals [i].z * normals [i].z);
                
                if (magnitudes [i] != 0f)
                {
                    normals [i].x /= magnitudes [i];
                    normals [i].y /= magnitudes [i];
                    normals [i].z /= magnitudes [i];
                }

                if(useRotations)
                {
                    normals [i].x += rotations [i].x;
                    normals [i].y += rotations [i].y;
                    normals [i].z += rotations [i].z;
                }
                
                // Total curve magnitude
                totalCurveLength += magnitudes [i];
                lastPosition = positions [i];
            }
            
            //totalCurveLength = (totalCurveLength * (1f / totalCurveLength)) * 5f;
            
            normals [0] = (positions [1] - positions [0]).normalized;
            if(useRotations)
                normals [0] += rotations [0];

            lastPosition = positions [0];
            
            int tl = 0;
            
            Vector3 currentNormal, rotatedNormal, lastRotatedNormal = Vector3.Cross(normals [0], Vector3.forward);
            
            int normalsLength = normals.Length;
            int normalsLengthMinusOne = normalsLength - 1;
//            int _vertexCountMinuOne = _vertexCount - 1;

            for (i = 0; i < _vertexCount; i+= _tessellation)
            {                       
                positionIndex = i / _tessellation;
                
                currentPosition = positions [positionIndex];
                
                size = sizes [positionIndex] * 0.5f;
                if(useColors)
                    color = colors [positionIndex];

                currentNormal = normals [positionIndex];            
                rotatedNormal = Vector3.Cross(currentNormal, Vector3.forward);
                if (positionIndex < normalsLengthMinusOne)
                    lastRotatedNormal = Vector3.Cross(normals [positionIndex + 1], Vector3.forward);

                magnitude += magnitudes [positionIndex] / totalCurveLength;

                for (j = 0; j < _tessellation; j++)
                {
                    ij = i + j;
                    _tessellationProgress = (float)j / (float)(_tessellation - 1);              
                    _vertices [ij] = currentPosition + Vector3.Lerp(rotatedNormal, lastRotatedNormal, 0.5f).normalized * (-1f + _tessellationProgress * 2f) * size;                
                    if(useColors)
                        _colors [ij] = color;   

                    _uvs [ij].x = _tessellationProgress * lineUVScale.x + lineUVOffset.x;
                    _uvs [ij].y = magnitude * lineUVScale.y + lineUVOffset.y;
                    
                    if (i != 0 && j != 0)
                    {                   
                        _triangles [tl] = ij - _tessellation - 1;
                        _triangles [tl + 1] = ij - 1;
                        _triangles [tl + 2] = ij;
                        
                        _triangles [tl + 3] = ij - _tessellation - 1;
                        _triangles [tl + 4] = ij;
                        _triangles [tl + 5] = ij - _tessellation;
                        tl += 6;
                    }
                }   

                lastRotatedNormal = rotatedNormal;
            }

            if (closeLine)
            {
                int indexA, indexB;
                size = Mathf.Lerp(sizes [0], sizes [_positionsLength - 1], 0.5f) * 0.5f;
                Vector3 normal = Vector3.Cross(Vector3.Lerp(normals [0], normals [_positionsLength - 1], 0.5f).normalized, Vector3.forward);
                Vector3 position = Vector3.Lerp(positions [0], positions [_positionsLength - 1], 0.5f);
                Vector3 posA = position - normal * size;
                Vector3 posB = position + normal * size;

                float progress;
                float tesselationMinusOne = _tessellation - 1;
                for (i = 0; i < _tessellation; i++)
                {
                    progress = (float)i / tesselationMinusOne;

                    indexA = _vertexCount - _tessellation + i;
                    indexB = i;

                    _vertices [indexA] = _vertices [indexB] = Vector3.Lerp(posA, posB, progress);

                    if (useColors)
                        _colors [indexA] = _colors [indexB] = Color32.Lerp(_colors [indexA], _colors [indexB], .5f);
                }  
            } else
            {
				int indexA;//, indexB;
                size = sizes [_positionsLength - 1] * 0.5f;
                Vector3 normal = Vector3.Cross(normals [_positionsLength - 1], Vector3.forward);
                Vector3 position = positions [_positionsLength - 1];
                Vector3 posA = position - normal * size;
                Vector3 posB = position + normal * size;
                
                float progress;
                float tesselationMinusOne = _tessellation - 1;
                for (i = 0; i < _tessellation; i++)
                {
                    progress = (float)i / tesselationMinusOne;
                    
                    indexA = _vertexCount - _tessellation + i;                
                    _vertices [indexA] = Vector3.Lerp(posA, posB, progress);
                    
                    if (useColors)
                        _colors [indexA] = _colors [indexA];
                }  
            }

            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;
            mesh.triangles = _triangles;
            mesh.uv = _uvs;
            if(useColors)
                mesh.colors32 = _colors;

            ResetLineSettings();
            return mesh;
        }

        public static Mesh VectorLine(Vector2[] positions, Color32 color, float size, bool closeLine = false)
        {        
            if (positions == null)
            {
#if DEBUG
                Debug.LogWarning("Line / Array is Null!");
#endif
                return null;
            }       
            
            if (positions.Length < 2)
            {
#if DEBUG
                Debug.LogWarning("Line / We need at least 2 positions!");
#endif
                return null;
            }
                    
            int _tessellation = 2;
            float _tessellationProgress;
            size *= 0.5f;

            int _vertexCount = positions.Length * _tessellation;
            int _positionsLength = positions.Length;
            int totalTriangles = (_positionsLength - 1) * (_tessellation - 1) * 6;

            Vector3[] _vertices = new Vector3[_vertexCount];       
            int[] _triangles = new int[totalTriangles];
            
            Color32[] _colors = null;
            _colors = new Color32[_vertexCount];        

            Vector3[] normals = new Vector3[_positionsLength];
            float[] magnitudes = new float[_positionsLength];
            
            int i = 0, j = 0, ij = 0, positionIndex;       
            Vector3 lastPosition = positions [0], currentPosition = positions [0];
			float magnitude = 0f, totalCurveLength = 0f;//currentMagnitude = 0f, 
            
            // Pre-Processing
            for (i = 0; i < _positionsLength; i++)
            {
                // Normal direction
                normals [i].x = positions [i].x - lastPosition.x;
                normals [i].y = positions [i].y - lastPosition.y;
                
                // Normal magnitude
                magnitudes [i] = Mathf.Sqrt(normals [i].x * normals [i].x + normals [i].y * normals [i].y + normals [i].z * normals [i].z);
                
                if (magnitudes [i] != 0f)
                {
                    normals [i].x /= magnitudes [i];
                    normals [i].y /= magnitudes [i];
                    normals [i].z /= magnitudes [i];
                }

                // Total curve magnitude
                totalCurveLength += magnitudes [i];
                lastPosition = positions [i];
            }
            
            //totalCurveLength = (totalCurveLength * (1f / totalCurveLength)) * 5f;
            
            normals [0] = (positions [1] - positions [0]).normalized;
            lastPosition = positions [0];
            
            int tl = 0;
            
            Vector3 currentNormal, rotatedNormal, lastRotatedNormal = Vector3.Cross(normals [0], Vector3.forward);
            
            int normalsLength = normals.Length;
            int normalsLengthMinusOne = normalsLength - 1;
//            int _vertexCountMinuOne = _vertexCount - 1;
            
            for (i = 0; i < _vertexCount; i+= _tessellation)
            {                       
                positionIndex = i / _tessellation;
                
                currentPosition = positions [positionIndex];

                currentNormal = normals [positionIndex];            
                rotatedNormal = Vector3.Cross(currentNormal, Vector3.forward);
                if (positionIndex < normalsLengthMinusOne)
                    lastRotatedNormal = Vector3.Cross(normals [positionIndex + 1], Vector3.forward);
                
                magnitude += magnitudes [positionIndex] / totalCurveLength;
                
                for (j = 0; j < _tessellation; j++)
                {
                    ij = i + j;
                    _tessellationProgress = (float)j / (float)(_tessellation - 1);              
                    _vertices [ij] = currentPosition + Vector3.Lerp(rotatedNormal, lastRotatedNormal, 0.5f).normalized * (-1f + _tessellationProgress * 2f) * size;                
                    _colors [ij] = color;

                    if (i != 0 && j != 0)
                    {                   
                        _triangles [tl] = ij - _tessellation - 1;
                        _triangles [tl + 1] = ij - 1;
                        _triangles [tl + 2] = ij;
                        
                        _triangles [tl + 3] = ij - _tessellation - 1;
                        _triangles [tl + 4] = ij;
                        _triangles [tl + 5] = ij - _tessellation;
                        tl += 6;
                    }
                }   
                
                lastRotatedNormal = rotatedNormal;
            }
            
            if (closeLine)
            {
                int indexA, indexB;
                Vector3 normal = Vector3.Cross(Vector3.Lerp(normals [0], normals [_positionsLength - 1], 0.5f).normalized, Vector3.forward);
                Vector3 position = Vector3.Lerp(positions [0], positions [_positionsLength - 1], 0.5f);
                Vector3 posA = position - normal * size;
                Vector3 posB = position + normal * size;
                
                float progress;
                float tesselationMinusOne = _tessellation - 1;
                for (i = 0; i < _tessellation; i++)
                {
                    progress = (float)i / tesselationMinusOne;
                    
                    indexA = _vertexCount - _tessellation + i;
                    indexB = i;
                    
                    _vertices [indexA] = _vertices [indexB] = Vector3.Lerp(posA, posB, progress);
                    _colors [indexA] = _colors [indexB] = Color32.Lerp(_colors [indexA], _colors [indexB], .5f);
                }  
            } else
            {
				int indexA;//, indexB;
                Vector3 normal = Vector3.Cross(normals [_positionsLength - 1], Vector3.forward);
                Vector3 position = positions [_positionsLength - 1];
                Vector3 posA = position - normal * size;
                Vector3 posB = position + normal * size;
                
                float progress;
                float tesselationMinusOne = _tessellation - 1;
                for (i = 0; i < _tessellation; i++)
                {
                    progress = (float)i / tesselationMinusOne;
                    
                    indexA = _vertexCount - _tessellation + i;                
                    _vertices [indexA] = Vector3.Lerp(posA, posB, progress);
                    _colors [indexA] = _colors [indexA];
                }  
            }
            
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;
            mesh.triangles = _triangles;
            mesh.colors32 = _colors;
            
            return mesh;
        }

    	public static void ChangeMeshUV1 (Mesh mesh, Vector2 uv)
    	{
    		Vector2[] uvs = mesh.uv;
    		int length = mesh.vertexCount;
    		if (uvs.Length != length)
    			uvs = new Vector2[length];
    		
    		for (int i = 0; i < length; i++) {
    			uvs [i].x = uv.x;
    			uvs [i].y = uv.y;
    		}
    		mesh.uv = uvs;
    	}
    	
    	public static void ChangeMeshUV2 (Mesh mesh, Vector2 uv)
    	{
            int length = mesh.vertices.Length;
            Vector2[] uvs = new Vector2[length];
    		for (int i = 0; i < length; i++) {
    			uvs [i].x = uv.x;
    			uvs [i].y = uv.y;
    		}
    		mesh.uv2 = uvs;
    	}
    	/*
    	public static void ChangeMeshUV3 (Mesh mesh, Vector2 uv)
    	{
            int length = mesh.vertices.Length;
            Vector2[] uvs = new Vector2[length];
            for (int i = 0; i < length; i++) {
                uvs [i].x = uv.x;
                uvs [i].y = uv.y;
            }
            mesh.uv3 = uvs;
    	}
    	*/
    	public static void ChangeMeshColor (Mesh mesh, Color32 color)
    	{
    		Color32[] colors = mesh.colors32;
    		int length = mesh.vertexCount;
    		if (colors.Length != length)
    			colors = new Color32[length];
    		
    		for (int i = 0; i < length; i++) {
    			colors [i].r = color.r;
    			colors [i].g = color.g;
    			colors [i].b = color.b;
    			colors [i].a = color.a;
    		}
    		mesh.colors32 = colors;
    	}
    	
    	public static void ChangeMeshColor (Mesh mesh, ColorChannel channel, byte value)
    	{
    		Color32[] colors = mesh.colors32;
    		int length = mesh.vertexCount;
    		if (colors.Length != length)
    			colors = new Color32[length];
    		
    		switch (channel) {
    		case ColorChannel.RED:
    			for (int i = 0; i < length; i++) {
    				colors [i].r = value;
    			}
    			break;
    		case ColorChannel.GREEN:
    			for (int i = 0; i < length; i++) {
    				colors [i].g = value;
    			}
    			break;	
    		case ColorChannel.BLUE:
    			for (int i = 0; i < length; i++) {
    				colors [i].b = value;
    			}
    			break;
    		case ColorChannel.ALPHA:
    			for (int i = 0; i < length; i++) {
    				colors [i].a = value;
    			}
    			break;	
    		}		
    		mesh.colors32 = colors;
    	}
    	
    	public static void ChangeMeshColor (Mesh mesh, ColorChannel channel, float value)
    	{
    		ChangeMeshColor (mesh, channel, (byte)Mathf.RoundToInt (Mathf.Lerp (0, 255, value)));
    	}
    	
    	public static void ChangeMeshColor (Mesh mesh, Color color)
    	{
    		ChangeMeshColor (mesh, (Color32)color);
    	}
        
        public static void ChengeMeshPosition(Mesh mesh, Vector3 offset)
        {
            Vector3[] vertices = mesh.vertices;
            int verticesLength = vertices.Length;
            for(int i = 0; i < verticesLength; i++)
            {
                vertices[i].x += offset.x;
                vertices[i].y += offset.y;
                vertices[i].z += offset.z;
            }
            mesh.vertices = vertices;
        }
        
        public static void ChangeMeshRotation(Mesh mesh, Quaternion rotation)
        {
            Vector3[] vertices = mesh.vertices;
            int verticesLength = vertices.Length;
            for(int i = 0; i < verticesLength; i++)
            {
                vertices[i] = rotation * vertices[i];
            }
            mesh.vertices = vertices;
        }

    	public static void ChangeMeshScale (Mesh mesh, Vector3 scale)
    	{
    		if (mesh == null)
    			return;
    		
    		if (scale == Vector3.one)
    			return;
    		
    		Vector3[] vertices = mesh.vertices;
    		int length = mesh.vertexCount;
    		
    		for (int i = 0; i < length; i++) {
    			vertices [i].x *= scale.x;
    			vertices [i].y *= scale.y;
    			vertices [i].z *= scale.z;
    		}
    		
    		mesh.vertices = vertices;
    	}
    	
    	public static void ChangeMeshScale (Mesh mesh, float scale)
    	{
    		if (scale == 1f)
    			return;
    		
    		ChangeMeshScale (mesh, new Vector3 (scale, scale, scale));
    	}
    	
    	public static void AutoWeldVertices (Mesh mesh, float threshold)
    	{
    		Bounds meshBounds = mesh.bounds;
    		
    		float thresholdPowerTwo = threshold * threshold;
    		Vector3[] vertices = mesh.vertices;
            Color32[] colors = mesh.colors32;
            Vector2[] uv = mesh.uv;
            Vector2[] uv2 = mesh.uv2;
     
    		// Build new vertex buffer and remove "duplicate" verticies
    		// that are within the given threshold.
    		List<int> newVerts = new List<int> ();
    		
    		int vertsLength = vertices.Length, newVertsLength = 0;
			int i = 0, j = 0;//, k = 0;
    		
    		float distance = 0f;
    		bool addVertex = false;
    		Vector3 tempVector;
    		
    		for (i = 0; i < vertsLength; i++) {
    			addVertex = true;
    			
    			for (j = 0; j < newVertsLength; j++) {		
                    tempVector.x = vertices[newVerts [j]].x - vertices [i].x;
                    tempVector.y = vertices[newVerts [j]].y - vertices [i].y;
                    tempVector.z = vertices[newVerts [j]].z - vertices [i].z;				
    				distance = tempVector.x * tempVector.x + tempVector.y * tempVector.y + tempVector.z * tempVector.z;
    				
    				if (distance <= thresholdPowerTwo) {
    					addVertex = false;
    					break;
    				}
    			}
    			
    			if (addVertex) {
    				newVerts.Add (i);
    				newVertsLength = newVerts.Count;
    			}
    		}
    		
    		// Rebuild triangles using new verticies
    		int[] tris = mesh.triangles;
    		for (i = 0; i < tris.Length; ++i) {
    			// Find new vertex point from buffer
    			for (j = 0; j < newVerts.Count; ++j) {
                    tempVector.x = vertices[newVerts [j]].x - vertices [tris [i]].x;
                    tempVector.y = vertices[newVerts [j]].y - vertices [tris [i]].y;
                    tempVector.z = vertices[newVerts [j]].z - vertices [tris [i]].z;				
    				distance = tempVector.x * tempVector.x + tempVector.y * tempVector.y + tempVector.z * tempVector.z;
    				if (distance <= thresholdPowerTwo) {
    					tris [i] = j;
    					break;
    				}
    			}
    		}
    		
    		// Update mesh!
    		mesh.Clear ();

            int finalLength = newVerts.Count;
            Vector3[] finalVerices = new Vector3[finalLength];
            Color32[] finalColors = new Color32[finalLength];
            Vector2[] finalUv = new Vector2[finalLength];
            Vector2[] finalUv2 = new Vector2[finalLength];

            int index;
            for(i = 0; i < finalLength; i++)
            {
                index = newVerts[i];
                finalVerices[i] = vertices[index];
                finalColors[i] = colors[index];
                finalUv[i] = uv[index];
                finalUv2[i] = uv2[index];
            }

            mesh.vertices = finalVerices;
    		mesh.triangles = tris;
            mesh.colors32 = finalColors;
            mesh.uv = finalUv;
            mesh.uv2 = finalUv2;
    		mesh.bounds = meshBounds;
    	}

        public static GameObject MergeMeshes(GameObject source)
        {
            string originalName = source.name;
//            Transform originalTransform = source.transform;
            Transform originalParentTransform = null;
            if (source.transform.parent != null)
                originalParentTransform = source.transform.parent.transform;
            
            source.transform.parent = null;

            GameObject go = (GameObject)GameObject.Instantiate(source, source.transform.position, source.transform.rotation);        
            source.transform.parent = originalParentTransform;
            
            // select children mesh filters
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            Material mat = null;
            int i = 0;
            
            for (i = 0; i < meshRenderers.Length; i++)
            {
                if (meshRenderers [i] == null)
                    continue;
                if (meshRenderers [i].sharedMaterial == null)
                    continue;
                
                mat = meshRenderers [i].sharedMaterial;
                break;
            }
            
            // prepare combine instance
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            
            i = 0;
            while (i < meshFilters.Length)
            {
                MeshFilter meshFilter = meshFilters [i];
                combine [i].mesh = meshFilter.sharedMesh;
                combine [i].transform = meshFilter.transform.localToWorldMatrix;
                i++;
            }
            
            //combine them        
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf == null)
                mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = go.AddComponent<MeshRenderer>();
            
            mr.sharedMaterial = mat;
            mf.sharedMesh = new Mesh();
            mf.sharedMesh.CombineMeshes(combine);
            mf.sharedMesh.RecalculateBounds();

            Vector3 posDiff = mf.sharedMesh.bounds.center;
            
            int vc = mf.sharedMesh.vertexCount;
            Vector3[] vertices = mf.sharedMesh.vertices;
            for (i = 0; i < vc; i++)
            {
                vertices [i] -= posDiff;
            }
            mf.sharedMesh.vertices = vertices;
            mf.sharedMesh.RecalculateBounds();

            Transform[] children = go.GetComponentsInChildren<Transform>();
            for (i = 0; i < children.Length; i++)
            {
                if (children [i] == null)
                    continue;
                if (children [i].gameObject != go)
                    GameObject.Destroy(children [i].gameObject);
            }
            
//            Vector3 worldPossDiff = go.transform.TransformPoint(posDiff);        
            go.transform.position = posDiff;
            go.transform.localScale = Vector3.one;
            go.transform.parent = originalParentTransform;
            go.name = originalName;

            return go;
        }

        public static Mesh Clone(Mesh mesh)
        {
            if(mesh == null)
                return null;

            Mesh output = new Mesh();
            output.name = mesh.name;
            output.vertices = (Vector3[])mesh.vertices.Clone();
            output.triangles = (int[])mesh.triangles.Clone();
            if(output.colors32 != null)
                output.colors32 = (Color32[])mesh.colors32.Clone();
            if(output.uv != null)
                output.uv = (Vector2[])mesh.uv.Clone();
            if(output.uv2 != null)
                output.uv2 = (Vector2[])mesh.uv2.Clone();
/*
            if(output.uv3 != null)
                output.uv3 = (Vector2[])mesh.uv3.Clone();
            if(output.uv4 != null)
                output.uv4 = (Vector2[])mesh.uv4.Clone();
*/                
            if(output.normals != null)
                output.normals = (Vector3[])mesh.normals.Clone();
            if(output.tangents != null)
                output.tangents = (Vector4[])mesh.tangents.Clone();

            int subMeshCount = mesh.subMeshCount;
            output.subMeshCount = subMeshCount;
            if(subMeshCount > 0)
            {
                for(int i = 0; i < subMeshCount; i++)
                {
                    output.SetTriangles(mesh.GetTriangles(i), i);
                }
            }

            output.bounds = mesh.bounds;
            return output;
        }

        public static Mesh CloneMesh(Mesh original)
        {
            if(original == null)
                return null;
            
            Mesh tempMesh = new Mesh();
            tempMesh.name = original.name+" Instance "+tempMesh.GetInstanceID();
            tempMesh.vertices = (Vector3[])original.vertices.Clone();
            tempMesh.triangles = (int[])original.triangles.Clone();
            
            if(original.uv != null && original.uv.Length > 0)
                tempMesh.uv = (Vector2[])original.uv.Clone();
            if(original.uv2 != null && original.uv2.Length > 0)
                tempMesh.uv2 = (Vector2[])original.uv2.Clone();
            if(original.colors32 != null && original.colors32.Length > 0)
                tempMesh.colors32 = (Color32[])original.colors32.Clone();
            
            int submeshCount = original.subMeshCount;
            tempMesh.subMeshCount = submeshCount;
            for(int i = 0; i < submeshCount; i++)
            {
                tempMesh.SetTriangles(original.GetTriangles(i), i);
            }
            tempMesh.bounds = original.bounds;
            
            return tempMesh;
        }
        
        public static Material CloneMaterial(Material original)
        {
            if(original == null) return null;
            Material material = new Material(original.shader);
            material.CopyPropertiesFromMaterial(original);
            return material;
        }
    }
}

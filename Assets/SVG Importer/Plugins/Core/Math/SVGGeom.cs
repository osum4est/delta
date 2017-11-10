// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SVGImporter.Utils
{
    using Rendering;
    using ClipperLib;

    public class SVGGeom
    {
        const int decimalPointInt = 100000;
        //const int decimalPointInt = 1000;
        const float decimalPointFloat = 1f / (float)decimalPointInt;

        public static List<IntPoint> ConvertFloatToInt(List<Vector2> polygon)
        {
            int polygonCount = polygon.Count, i;
            List<IntPoint> polygonInt = new List<IntPoint>(polygonCount);
            for (i = 0; i < polygonCount; i++)
            {
                polygonInt.Add(new IntPoint((int)(polygon [i].x * decimalPointInt), (int)(polygon [i].y * decimalPointInt)));
            }
            return polygonInt;
        }

        public static List<Vector2> ConvertIntToFloat(List<IntPoint> polygonInt)
        {
            int polygonCount = polygonInt.Count, i;
            List<Vector2> polygon = new List<Vector2>(polygonCount);
            for (i = 0; i < polygonCount; i++)
            {
                polygon.Add(new Vector2((float)polygonInt [i].X * decimalPointFloat, (float)polygonInt [i].Y * decimalPointFloat));
            }        
            return polygon;
        }

    	public static List<List<Vector2>> SimplifyPolygon(List<Vector2> polygon, PolyFillType polyFillType = PolyFillType.pftNonZero)
        {
            if (polygon == null || polygon.Count == 0)
                return null;

            List<IntPoint> polygonInt = ConvertFloatToInt(polygon);

    		List<List<IntPoint>> polygonsInt = Clipper.SimplifyPolygon(polygonInt, polyFillType);
			int polygonsIntCount = polygonsInt.Count, i;//, j, polygonCount;
            List<List<Vector2>> polygons = new List<List<Vector2>>(polygonsIntCount);
            for (i = 0; i < polygonsIntCount; i++)
            {
                //polygonCount = polygonsInt [i].Count;
                polygons.Add(ConvertIntToFloat(polygonsInt [i]));
            }

            if (polygons == null || polygons.Count == 0)
                return null;

            return polygons;
        }	

        public static List<List<Vector2>> MergePolygon(List<List<Vector2>> polygon)
        {
            if (polygon == null || polygon.Count == 0)
                return null;

            List<List<IntPoint>> solution = new List<List<IntPoint>>(){ConvertFloatToInt(polygon [0])};
            for (int i = 1; i < polygon.Count; i++)
            {
                solution = MergePolygon(solution, ConvertFloatToInt(polygon [i]));                
            }

            List<List<Vector2>> output = new List<List<Vector2>>();
            for(int i = 0; i < solution.Count; i++)
            {
                output.Add(ConvertIntToFloat(solution[i]));
            }

            return output;
        }

        public static List<List<IntPoint>> MergePolygon(List<List<IntPoint>> polygonA, List<IntPoint> polygonB)
        {
            Clipper clipper = new Clipper();
            clipper.AddPaths(polygonA, PolyType.ptSubject, true);
            clipper.AddPath(polygonB, PolyType.ptClip, true);            
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctUnion, solution);
            return solution;
        }

        public static List<List<Vector2>> ClipPolygon(List<List<Vector2>> polygon, List<List<Vector2>> clipPath)
        {
            if (polygon == null || polygon.Count == 0) return null;
            if(clipPath == null || clipPath.Count == 0) return polygon;
            
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            List<List<IntPoint>> clippedPolygon;

            int i, j;
            for (i = 0; i < polygon.Count; i++)
            {
                for (j = 0; j < clipPath.Count; j++)
                {
                    clippedPolygon = ClipPolygon(ConvertFloatToInt(polygon [i]), ConvertFloatToInt(clipPath [j]));
                    if(clippedPolygon != null && clippedPolygon.Count > 0)
                    {
                        solution.AddRange(clippedPolygon);
                    }
                }
            }

            List<List<Vector2>> output = new List<List<Vector2>>();
            for(i = 0; i < solution.Count; i++)
            {
                output.Add(ConvertIntToFloat(solution[i]));
            }
            
            return output;
        }

        public static List<List<IntPoint>> ClipPolygon(List<IntPoint> polygon, List<IntPoint> clipPath)
        {
            Clipper clipper = new Clipper();
            clipper.AddPath(polygon, PolyType.ptSubject, true);
            clipper.AddPath(clipPath, PolyType.ptClip, true);            
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctIntersection, solution);
            return solution;
        }
        /*
        public static List<SVGClipPath> ClipPolygon(List<List<Vector2>> polygon, List<List<Vector2>> holes)
        {
            PolyFillType polyFillType = PolyFillType.pftNonZero;

            if (polygon == null || polygon.Count == 0)
                return null;

            List<SVGClipPath> output = new List<SVGClipPath>();

            if (holes!= null && holes.Count > 0)
            {
                Clipper clipper = new Clipper();
                for (int i = 0; i < polygon.Count; i++)
                {
                    List<List<IntPoint>> intPaths = Clipper.SimplifyPolygon(ConvertFloatToInt(polygon [i]), polyFillType);
                    if(intPaths == null || intPaths.Count == 0)
                        continue;

                    for(int j = 0; j < intPaths.Count; j++)
                    {
                        if(intPaths[j] == null || intPaths[j].Count == 0)
                            continue;

                        clipper.AddPath(intPaths[j], PolyType.ptSubject, true);
                    }
                }
                for (int i = 0; i < holes.Count; i++)
                {
                    List<List<IntPoint>> intPaths = Clipper.SimplifyPolygon(ConvertFloatToInt(holes [i]), polyFillType);
                    if(intPaths == null || intPaths.Count == 0)
                        continue;
                    
                    for(int j = 0; j < intPaths.Count; j++)
                    {
                        if(intPaths[j] == null || intPaths[j].Count == 0)
                            continue;
                        
                        clipper.AddPath(intPaths[j], PolyType.ptClip, true);
                    }
                }

                PolyTree polytree = new PolyTree();
                bool clippingSuccess = clipper.Execute(ClipType.ctXor, polytree, polyFillType, polyFillType);
                if (!clippingSuccess)
                {
                    // TODO Clipping is not working!!!
                    Debug.LogWarning("Clipping failed!");
                    return null;
                }

                List<PolyNode> nodes = polytree.Childs;

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes [i].Contour == null || nodes [i].Contour.Count < 3)
                        continue;

                    SVGClipPath clipPath = new SVGClipPath();
                    if (nodes [i].IsHole)
                    {
                        if (clipPath.holes == null)
                            clipPath.holes = new List<List<Vector2>>();
                        clipPath.holes.Add(ConvertIntToFloat(nodes [i].Contour));
                    } else
                    {
                        if (clipPath.path == null)
                            clipPath.path = new List<Vector2>();
                        clipPath.path = ConvertIntToFloat(nodes [i].Contour);
                    }

                    List<PolyNode> childs = nodes [i].Childs;
                    if (childs != null && childs.Count > 0)
                    {
                        for (int j = 0; j < childs.Count; j++)
                        {
                            if (childs [j] == null || childs [j].Contour.Count < 3)
                                continue;

                            if (childs [j].IsHole)
                            {
                                if (clipPath.holes == null)
                                    clipPath.holes = new List<List<Vector2>>();
                                clipPath.holes.Add(ConvertIntToFloat(childs [j].Contour));
                            }
                        }
                    }

                    if (clipPath.path != null && clipPath.path.Count > 3)
                    {
                        output.Add(clipPath);
                    }
                }
            } else
            {
                for(int i = 0; i < polygon.Count; i++)
                {
                    List<List<IntPoint>> intPaths = Clipper.SimplifyPolygon(ConvertFloatToInt(polygon [i]), polyFillType);
                    if(intPaths == null || intPaths.Count == 0)
                        continue;

                    for(int j = 0; j < intPaths.Count; j++)
                    {
                        SVGClipPath clipPath = new SVGClipPath(ConvertIntToFloat(intPaths[j]));
                        if(clipPath != null && clipPath.path != null && clipPath.path.Count > 3)
                        {
                            output.Add(clipPath);
                        }
                    }
                }
            }

            return output;
        }
        */
        public static List<Vector2> SimplifyPolygonTemp(List<Vector2> polygon)
        {
            ///*
            if (polygon == null || polygon.Count == 0)
                return null;
            
            int polygonCount = polygon.Count, polygonsIntCount, i, j;
            List<IntPoint> polygonInt = new List<IntPoint>(polygonCount);
            for (i = 0; i < polygonCount; i++)
            {
                polygonInt.Add(new IntPoint((int)(polygon [i].x * decimalPointInt), (int)(polygon [i].y * decimalPointInt)));
            }
            
            List<List<IntPoint>> polygonsInt = Clipper.SimplifyPolygon(polygonInt, PolyFillType.pftEvenOdd);
            polygonsIntCount = polygonsInt.Count;
            List<List<Vector2>> polygons = new List<List<Vector2>>(polygonsIntCount);
            for (i = 0; i < polygonsIntCount; i++)
            {
                polygonCount = polygonsInt [i].Count;
                polygons.Add(new List<Vector2>(polygonCount));
                for (j = 0; j < polygonCount; j++)
                {
                    polygons [i].Add(new Vector2((float)polygonsInt [i] [j].X * decimalPointFloat, (float)polygonsInt [i] [j].Y * decimalPointFloat));
                }
            }
            
            if (polygons == null || polygons.Count == 0)
                return null;
            
            if (polygons.Count > 1)
            {
                Debug.Log("Num of polygons: " + polygons.Count);
            }

            return polygons [0];
        }   
    }
}

// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SVGImporter.Utils
{    

    public enum StrokeLineJoin
    {
        miter,
        miterClip,
        round,
        bevel
    }
    
    public enum StrokeLineCap
    {
        butt,
        round,
        square
    }

    public struct StrokeSegment
    {
        public Vector2 startPoint;
        public Vector2 endPoint;

        public Vector2 direction;
        public Vector2 directionNormalized;
        public Vector2 directionNormalizedRotated;
        public float length;

        public StrokeSegment(Vector2 startPoint, Vector2 endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            this.direction = endPoint - startPoint;
            this.length = direction.magnitude;
            if(this.length != 0f)
            {
                this.directionNormalized.x = direction.x / this.length;
                this.directionNormalized.y = direction.y / this.length;
                directionNormalizedRotated = Quaternion.Euler(0f, 0f, 90f) * directionNormalized;
            } else {
                this.directionNormalized = directionNormalizedRotated = Vector2.zero;
            }
        }
    }

    public class SVGLineUtils {

        public static bool LineLineIntersection(out Vector2 intersection, Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End){
            
            intersection = Vector2.zero;
            
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = line1End.x - line1Start.x;     s1_y = line1End.y - line1Start.y;
            s2_x = line2End.x - line2Start.x;     s2_y = line2End.y - line2Start.y;
            
            float s, t;
            s = (-s1_y * (line1Start.x - line2Start.x) + s1_x * (line1Start.y - line2Start.y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = ( s2_x * (line1Start.y - line2Start.y) - s2_y * (line1Start.x - line2Start.x)) / (-s2_x * s1_y + s1_x * s2_y);

            intersection.x = line1Start.x + (t * s1_x);
            intersection.y = line1Start.y + (t * s1_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                return true;
            }
            
            return false; // No collision
        }

        public static List<Vector2> Stroke(StrokeSegment[] segments, float thickness, Color32 color, StrokeLineJoin lineJoin, StrokeLineCap lineCap, float miterLimit = 4f, bool closeLine = false, float roundQuality = 10f)
        {
            if(segments == null || segments.Length == 0)
                return null;
            
            if(closeLine && segments.Length == 1)
                closeLine = false;
            
            List<Vector2> innerPoints = new List<Vector2>();
            List<Vector2> outerPoints = new List<Vector2>();
            
            if(closeLine)
            {
                List<StrokeSegment> tempSegments = new List<StrokeSegment>(segments);
                tempSegments.Add(new StrokeSegment(segments[segments.Length -1].endPoint, segments[0].startPoint));
                tempSegments.Add(new StrokeSegment(segments[0].startPoint, segments[0].endPoint));
                segments = tempSegments.ToArray();
            }

            miterLimit = (miterLimit - 1f) * thickness * 2f;
            if(miterLimit < 1f) miterLimit = 1f;

            int i, i1, j, segmentsLength = segments.Length;
            
            float   segmentsAngle, segmentsAngleRotated, halfWidth = thickness * 0.5f, 
            angleProgress, radAngle = 0f, miterClipHalf = miterLimit * 0.5f, miterClipHalfDouble = miterClipHalf * miterClipHalf;
            Vector2 segmentLeftStartA, segmentLeftEndA = Vector2.zero, segmentRightStartA, segmentRightEndA = Vector2.zero,
            segmentLeftEndB, segmentRightEndB,
            intersectionLeft, intersectionRight, segmentStartCenter;
            
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0f, 0f, 90f), Vector2.one);
            
            if(lineCap == StrokeLineCap.butt || closeLine)
            {                
                innerPoints.AddRange(new Vector2[]{
                    segments[0].startPoint - segments[0].directionNormalizedRotated * halfWidth,
                    segments[0].startPoint + segments[0].directionNormalizedRotated * halfWidth,
                });
            } else if(lineCap == StrokeLineCap.round)
            {
                segmentStartCenter = Vector2.Lerp(segments[0].startPoint - segments[0].directionNormalizedRotated * halfWidth, 
                                                  segments[0].startPoint + segments[0].directionNormalizedRotated * halfWidth, 0.5f);
                
                radAngle = Mathf.Atan2(segments[0].directionNormalizedRotated.y, segments[0].directionNormalizedRotated.x);
                
                float roundSegmentsfAlt = roundQuality * thickness;
                float roundSegmentsfAltMinusOne = roundSegmentsfAlt - 1;
                if(roundSegmentsfAltMinusOne > 0)
                {
                    for(j = 0; j <= roundSegmentsfAlt; j++)
                    {
                        angleProgress = 1f - Mathf.Clamp01(j / roundSegmentsfAltMinusOne);
                        innerPoints.Add(segmentStartCenter + new Vector2(Mathf.Cos(radAngle + angleProgress * Mathf.PI) * halfWidth, Mathf.Sin(radAngle + angleProgress * Mathf.PI) * halfWidth));
                    }
                }
                
                innerPoints.AddRange(new Vector2[]{
                    segments[0].startPoint + segments[0].directionNormalizedRotated * halfWidth,
                });
                
                outerPoints.AddRange(new Vector2[]{
                    segments[0].startPoint - segments[0].directionNormalizedRotated * halfWidth,
                });
                
            } else if(lineCap == StrokeLineCap.square)
            {
                innerPoints.AddRange(new Vector2[]{
                    segments[0].startPoint - segments[0].directionNormalized * halfWidth - segments[0].directionNormalizedRotated * halfWidth,
                    segments[0].startPoint - segments[0].directionNormalized * halfWidth + segments[0].directionNormalizedRotated * halfWidth,
                });
            }
            
            if(segmentsLength > 1)
            {
                for(i = 1; i < segmentsLength; i++)
                {
                    i1 = i - 1;
                    /*
                    if(segments[i1].length == 0f || segments[i].length == 0)
                    {
                        continue;
                    }
                    */
                    segmentsAngle = Vector2.Dot(segments[i].directionNormalized, segments[i1].directionNormalized);
                    segmentsAngleRotated = Vector2.Dot(segments[i].directionNormalized, segments[i1].directionNormalizedRotated);
                    
                    float miterLength = (1f / Mathf.Sin((Mathf.PI - Mathf.Acos(segmentsAngle)) * 0.5f)) * thickness;
                    float miterLengthHalf = miterLength * 0.5f;
                    Vector2 miterVector = Vector2.Lerp(segments[i1].directionNormalizedRotated, segments[i].directionNormalizedRotated, 0.5f).normalized;
                    Vector2 miterVectorLengthHalf = miterVector * miterLengthHalf;
                    Vector2 miterVectorRotated = rotationMatrix.MultiplyVector(miterVector);
                    
                    segmentLeftStartA = segments[i].startPoint - segments[i].directionNormalizedRotated * halfWidth;
                    segmentLeftEndA = segments[i].endPoint - segments[i].directionNormalizedRotated * halfWidth;
                    segmentRightStartA = segments[i].startPoint + segments[i].directionNormalizedRotated * halfWidth;
                    segmentRightEndA = segments[i].endPoint + segments[i].directionNormalizedRotated * halfWidth;
                    
                    //segmentLeftStartB = segments[i1].startPoint - segments[i1].directionNormalizedRotated * halfWidth;
                    segmentLeftEndB = segments[i1].endPoint - segments[i1].directionNormalizedRotated * halfWidth;
                    //segmentRightStartB = segments[i1].startPoint + segments[i1].directionNormalizedRotated * halfWidth;
                    segmentRightEndB = segments[i1].endPoint + segments[i1].directionNormalizedRotated * halfWidth;

                    if(lineJoin == StrokeLineJoin.miter)
                    {
                        if(miterLimit < miterLength)
                            lineJoin = StrokeLineJoin.bevel;
                    }

                    if(lineJoin == StrokeLineJoin.miter || lineJoin == StrokeLineJoin.miterClip)
                    {            
                        if(segmentsAngle == 1f || segmentsAngle == -1f)
                        {
                            innerPoints.AddRange(new Vector2[]{
                                segmentRightEndB,
                                segmentRightStartA,
                            });
                            
                            outerPoints.AddRange(new Vector2[]{
                                segmentLeftEndB,
                                segmentLeftStartA,
                            });
                        } else {
                            if(segmentsAngleRotated < 0)
                            {
                                if(miterLimit <= miterLength)
                                {
                                    Vector2 a = segments[i1].endPoint + miterVector * miterClipHalf;
                                    Vector2 b = segments[i1].endPoint + miterVectorLengthHalf;
                                    Vector2 c = a + miterVectorRotated;
                                    
                                    LineLineIntersection(out intersectionLeft, b, segmentRightEndB, a, c);
                                    LineLineIntersection(out intersectionRight, b, segmentRightStartA, a, c);
                                    
                                    if(miterClipHalfDouble <= (Vector2.Lerp(segmentRightEndB, segmentRightStartA, 0.5f) - segments[i1].endPoint).sqrMagnitude)
                                    {
                                        intersectionLeft = segmentRightEndB;
                                        intersectionRight = segmentRightStartA;
                                    }
                                    
                                    innerPoints.AddRange(new Vector2[]{
                                        intersectionLeft,
                                        intersectionRight,
                                    });
                                    
                                    outerPoints.AddRange(new Vector2[]{
                                        segmentLeftEndB,
                                        segmentLeftStartA,
                                    });
                                } else {
                                    intersectionRight = segments[i1].endPoint + miterVectorLengthHalf;
                                    
                                    innerPoints.AddRange(new Vector2[]{
                                        intersectionRight,
                                    });
                                    
                                    outerPoints.AddRange(new Vector2[]{
                                        segmentLeftEndB,
                                        segmentLeftStartA,
                                    });
                                }
                                
                            } else {
                                if(miterLimit <= miterLength)
                                {
                                    Vector2 a = segments[i1].endPoint - miterVector * miterClipHalf;
                                    Vector2 b = segments[i1].endPoint - miterVectorLengthHalf;
                                    Vector2 c = a + miterVectorRotated;
                                    
                                    LineLineIntersection(out intersectionLeft, b, segmentLeftStartA, a, c);
                                    LineLineIntersection(out intersectionRight, b, segmentLeftEndB, a, c);
                                    
                                    if(miterClipHalfDouble <= (Vector2.Lerp(segmentLeftStartA, segmentLeftEndB, 0.5f) - segments[i1].endPoint).sqrMagnitude)
                                    {
                                        intersectionLeft = segmentLeftStartA;
                                        intersectionRight = segmentLeftEndB;
                                    }
                                    
                                    outerPoints.AddRange(new Vector2[]{
                                        intersectionRight,
                                        intersectionLeft,
                                    });
                                    
                                    innerPoints.AddRange(new Vector2[]{
                                        segmentRightEndB,
                                        segmentRightStartA,
                                    });
                                } else {
                                    intersectionLeft = segments[i1].endPoint - miterVectorLengthHalf;
                                    
                                    outerPoints.AddRange(new Vector2[]{
                                        intersectionLeft,
                                    });
                                    
                                    innerPoints.AddRange(new Vector2[]{
                                        segmentRightEndB,
                                        segmentRightStartA,
                                    });
                                }
                            }
                            
                        }
                    } else if(lineJoin == StrokeLineJoin.bevel) {
                        innerPoints.AddRange(new Vector2[]{
                            segmentRightEndB,
                            segmentRightStartA,
                        });
                        
                        outerPoints.AddRange(new Vector2[]{
                            segmentLeftEndB,
                            segmentLeftStartA,
                        });
                    } else if(lineJoin == StrokeLineJoin.round)
                    {
                        if(segmentsAngle == 1f)
                        {
                            innerPoints.AddRange(new Vector2[]{
                                segmentRightEndB,
                                segmentRightStartA,
                            });
                            
                            outerPoints.AddRange(new Vector2[]{
                                segmentLeftEndB,
                                segmentLeftStartA,
                            });
                        } else {
                            if(segmentsAngleRotated < 0)
                            {
                                innerPoints.AddRange(new Vector2[]{
                                    segmentRightEndB,
                                });
                                
                                outerPoints.AddRange(new Vector2[]{
                                    segmentLeftEndB,
                                    segmentLeftStartA,
                                });
                                
                                segmentStartCenter = segments[i].startPoint;
                                Vector2 dir = segments[i1].directionNormalizedRotated;
                                
                                float angle = Mathf.Acos(Vector2.Dot(segments[i1].directionNormalized, segments[i].directionNormalized));
                                radAngle = Mathf.Atan2(dir.y, dir.x);
                                
                                float roundSegmentsfAlt = roundQuality * thickness * (Mathf.Acos(segmentsAngle)/ Mathf.PI);
                                if(roundSegmentsfAlt < 1) roundSegmentsfAlt = 1f;
                                float roundSegmentsfAltMinusOne = roundSegmentsfAlt;
                                if(roundSegmentsfAltMinusOne > 0)
                                {
                                    for(j = 0; j < roundSegmentsfAlt; j++)
                                    {
                                        angleProgress = Mathf.Clamp01(j / roundSegmentsfAltMinusOne);
                                        innerPoints.Add(segmentStartCenter + new Vector2(Mathf.Cos(radAngle - angleProgress * angle) * halfWidth, Mathf.Sin(radAngle - angleProgress * angle) * halfWidth));
                                    }
                                }
                                
                                innerPoints.AddRange(new Vector2[]{
                                    segmentRightStartA,
                                });
                            } else {
                                innerPoints.AddRange(new Vector2[]{
                                    segmentRightEndB,
                                    segmentRightStartA,
                                });
                                
                                outerPoints.AddRange(new Vector2[]{
                                    segmentLeftEndB,
                                    
                                });
                                
                                segmentStartCenter = segments[i].startPoint;
                                Vector2 dir = -segments[i].directionNormalizedRotated;
                                
                                float angle = Mathf.Acos(Vector2.Dot(segments[i1].directionNormalized, segments[i].directionNormalized));
                                radAngle = Mathf.Atan2(dir.y, dir.x);
                                
                                float roundSegmentsfAlt = roundQuality * thickness * (Mathf.Acos(segmentsAngle)/ Mathf.PI);
                                if(roundSegmentsfAlt < 1) roundSegmentsfAlt = 1f;
                                float roundSegmentsfAltMinusOne = roundSegmentsfAlt;
                                if(roundSegmentsfAltMinusOne > 0)
                                {
                                    for(j = 0; j < roundSegmentsfAlt; j++)
                                    {
                                        angleProgress = Mathf.Clamp01(1f -(j / roundSegmentsfAltMinusOne));
                                        outerPoints.Add(segmentStartCenter + new Vector2(Mathf.Cos(radAngle - angleProgress * angle) * halfWidth, Mathf.Sin(radAngle - angleProgress * angle) * halfWidth));
                                    }
                                }
                                
                                outerPoints.AddRange(new Vector2[]{
                                    segmentLeftStartA,
                                });
                            }
                        }
                    }
                }
            }
            
            int lastSegmentIndex = segments.Length - 1;
            
            segmentLeftStartA = segments[lastSegmentIndex].startPoint - segments[lastSegmentIndex].directionNormalizedRotated * halfWidth;
            segmentLeftEndA = segments[lastSegmentIndex].endPoint - segments[lastSegmentIndex].directionNormalizedRotated * halfWidth;
            segmentRightStartA = segments[lastSegmentIndex].startPoint + segments[lastSegmentIndex].directionNormalizedRotated * halfWidth;
            segmentRightEndA = segments[lastSegmentIndex].endPoint + segments[lastSegmentIndex].directionNormalizedRotated * halfWidth;
            
            if(!closeLine)
            {                
                if(lineCap == StrokeLineCap.butt)
                {
                    innerPoints.AddRange(new Vector2[]{
                        segmentRightEndA,
                        segmentLeftEndA,
                    });
                } else if(lineCap == StrokeLineCap.round)
                {
                    innerPoints.AddRange(new Vector2[]{
                        segmentRightEndA
                    });
                    
                    outerPoints.AddRange(new Vector2[]{
                        segmentLeftEndA,
                    });
                    
                    segmentStartCenter = Vector2.Lerp(segmentLeftEndA, segmentRightEndA, 0.5f);
                    
                    radAngle = Mathf.Atan2(-segments[lastSegmentIndex].directionNormalizedRotated.y, -segments[lastSegmentIndex].directionNormalizedRotated.x);
                    
                    float roundSegmentsfAlt = roundQuality * thickness;
                    float roundSegmentsfAltMinusOne = roundSegmentsfAlt - 1;
                    if(roundSegmentsfAltMinusOne > 0)
                    {
                        for(j = 0; j <= roundSegmentsfAlt; j++)
                        {
                            angleProgress = 1f - Mathf.Clamp01(j / roundSegmentsfAltMinusOne);
                            innerPoints.Add(segmentStartCenter + new Vector2(Mathf.Cos(radAngle + angleProgress * Mathf.PI) * halfWidth, Mathf.Sin(radAngle + angleProgress * Mathf.PI) * halfWidth));
                        }
                    }                    
                } else if(lineCap == StrokeLineCap.square)
                {
                    Vector2 lastSegmentOffset = segments[lastSegmentIndex].directionNormalized * halfWidth;
                    
                    innerPoints.AddRange(new Vector2[]{
                        segmentRightEndA + lastSegmentOffset,
                        segmentLeftEndA + lastSegmentOffset,
                    });
                }
            }
            
            if(closeLine && lineJoin == StrokeLineJoin.miter || lineJoin == StrokeLineJoin.miterClip)
            {
                innerPoints.AddRange(new Vector2[]{
                    segmentRightEndA,
                    segmentLeftEndA,
                });
            }
            
            outerPoints.Reverse();
            innerPoints.AddRange(outerPoints);

            return innerPoints;
        }

        public static Mesh StrokeMesh(StrokeSegment[] segments, float thickness, Color32 color, StrokeLineJoin lineJoin, StrokeLineCap lineCap, float miterLimit = 4f, float[] dashArray = null, float dashOffset = 0f, bool closeLine = false, float roundQuality = 10f)
        {           
            if(segments == null || segments.Length == 0)
                return null;

            List<List<Vector2>> finalPoints = StrokeShape(new List<StrokeSegment[]>(){segments}, thickness, color, lineJoin, lineCap, miterLimit, dashArray, dashOffset, closeLine, roundQuality);
            return TessellateStroke(finalPoints, color);
        }

        public static List<List<Vector2>> StrokeShape(List<StrokeSegment[]> segments, float thickness, Color32 color, StrokeLineJoin lineJoin, StrokeLineCap lineCap, float miterLimit = 4f, float[] dashArray = null, float dashOffset = 0f, bool closeLine = false, float roundQuality = 10f)
        {
            if(segments == null || segments.Count == 0)
                return null;
            
            float totalCurveLength = 0f;
            int i, j;
            
            for(i = 0; i < segments.Count; i++)
            { 
                if(segments[i] == null)
                    continue;
                for(j = 0; j < segments[i].Length; j++)
                {
                    totalCurveLength += segments[i][j].length;
                }
            }

            if(totalCurveLength == 0f)
                return null;
            
            bool useDash;
            ProcessDashArray(ref dashArray, out useDash);
            
            bool closeSegments = closeLine;
            List<StrokeSegment[]> finalSegments = new List<StrokeSegment[]>();

            for(i = 0; i < segments.Count; i++)
            {
                if(segments[i] == null || segments[i].Length == 0)
                    continue;

                if(!useDash)
                {
                    finalSegments.Add(segments[i]);
                } else {
                    finalSegments.AddRange(CreateDashedStroke(segments[i], dashArray, dashOffset, ref closeSegments));
                }
            }

            if(finalSegments.Count > 0)
            {
                List<List<Vector2>> finalPoints = new List<List<Vector2>>();
                for(i = 0; i < finalSegments.Count; i++)
                {
                    List<Vector2> points = Stroke(finalSegments[i], thickness, color, lineJoin, lineCap, miterLimit, closeSegments, roundQuality);
                    if(points == null || points.Count < 2)
                        continue;
                    
                    finalPoints.Add(points);
                }
                return finalPoints;
            } else {
                return null;
            }
        }

        public static Mesh StrokeMesh(List<StrokeSegment[]> segments, float thickness, Color32 color, StrokeLineJoin lineJoin, StrokeLineCap lineCap, float miterLimit = 4f, float[] dashArray = null, float dashOffset = 0f, bool closeLine = false, float roundQuality = 10f)
        {           
            List<List<Vector2>> finalPoints = StrokeShape(segments, thickness, color, lineJoin, lineCap, miterLimit, dashArray, dashOffset, closeLine, roundQuality);
            return TessellateStroke(finalPoints, color);
        }

        protected static List<StrokeSegment[]> CreateDashedStroke(StrokeSegment[] segments, float[] dashArray, float dashOffset, ref bool closeLine)
        {
            if(closeLine)
            {
                System.Array.Resize<StrokeSegment>(ref segments, segments.Length + 1);
                segments[segments.Length - 1] = new StrokeSegment(segments[segments.Length - 2].endPoint, segments[0].startPoint);
                closeLine = false;
            }
            
            List<StrokeSegment[]> finalSegments = new List<StrokeSegment[]>();
            
            int dashArrayLength = dashArray.Length;
            int dashArrayIndex = 0;
            int segmentsLength = segments.Length;
            
            float dashElapsed = dashOffset, lengthA, lengthB;
            
            List<StrokeSegment> strokeSegments = new List<StrokeSegment>();
            
            int i = 0;
            while(i < segmentsLength)
            {
                if(dashArrayIndex % 2 == 0)
                {
                    lengthA = Mathf.Clamp(dashElapsed, 0f, segments[i].length);
                    lengthB = Mathf.Clamp(dashElapsed + dashArray[dashArrayIndex], 0f, segments[i].length);
                    if(lengthB - lengthA > 0f)
                    {
                        strokeSegments.Add(new StrokeSegment(
                            segments[i].startPoint + segments[i].directionNormalized * lengthA,
                            segments[i].startPoint + segments[i].directionNormalized * lengthB
                            ));
                    }
                } else {
                    if(strokeSegments.Count > 0) {finalSegments.Add(strokeSegments.ToArray()); strokeSegments.Clear(); }
                }
                
                if(dashElapsed + dashArray[dashArrayIndex] < segments[i].length)
                {
                    dashElapsed += dashArray[dashArrayIndex];
                    dashArrayIndex = (dashArrayIndex + 1) % dashArrayLength;
                } else {
                    dashElapsed -= segments[i].length;
                    i++;
                }
            }
            
            if(strokeSegments.Count > 0) {finalSegments.Add(strokeSegments.ToArray()); strokeSegments.Clear(); }

            return finalSegments;
        }

        protected static void ProcessDashArray(ref float[] dashArray, out bool useDash)
        {
            useDash = dashArray != null && dashArray.Length > 0;
            float dashLength = 0f;
            if(useDash)
            {
                int dashArrayLength = dashArray.Length;
                int  i, j;
                if(dashArrayLength % 2 == 1)
                {
                    System.Array.Resize<float>(ref dashArray, dashArrayLength * 2);
                    j = 0;
                    for(i = dashArrayLength; i < dashArray.Length; i++)
                    {
                        dashArray[i] = dashArray[j++];
                    }
                    dashArrayLength = dashArray.Length;
                }

                for(i = 0; i < dashArray.Length; i++)
                {
                    if(dashArray[i] < 0) dashArray[i] = 0f;
                    dashLength += dashArray[i];
                }
            }
            
            if(dashLength == 0f)
                useDash = false;
        }

        public static Mesh TessellateStroke(List<List<Vector2>> finalPoints, Color32 color)
        {            
            if(finalPoints == null || finalPoints.Count == 0)
                return null;

            int i, j;

            LibTessDotNet.Tess tesselation = new LibTessDotNet.Tess();
            
            LibTessDotNet.ContourVertex[] path;
            for(i = 0; i < finalPoints.Count; i++)
            {
                if(finalPoints[i] == null || finalPoints[i].Count < 2)
                    continue;
                
                path = new LibTessDotNet.ContourVertex[finalPoints[i].Count];
                for(j = 0; j < finalPoints[i].Count; j++)
                {
                    path[j].Position = new LibTessDotNet.Vec3{X = finalPoints[i][j].x, Y = finalPoints[i][j].y, Z = 0f };
                }
                tesselation.AddContour(path);
            }
            
            tesselation.Tessellate(LibTessDotNet.WindingRule.Positive, LibTessDotNet.ElementType.Polygons, 3);
            if(tesselation.Vertices == null || tesselation.Vertices.Length == 0)
                return null;
            
            Mesh mesh = new Mesh();
            
            int numVertices = tesselation.Vertices.Length;
            int numTriangles = tesselation.ElementCount * 3;
            
            int[] trianglesFin = new int[numTriangles];
            Vector3[] verticesFin = new Vector3[numVertices];
            Color32[] colorsFin = new Color32[numVertices];
            
            for(i = 0; i < numVertices; i++)
            {
                verticesFin[i] = new Vector3(tesselation.Vertices[i].Position.X, tesselation.Vertices[i].Position.Y, 0f);
                colorsFin[i] = color;
            }
            for (i = 0; i < numTriangles; i += 3)
            {
                trianglesFin[i] = tesselation.Elements[i];
                trianglesFin[i + 1] = tesselation.Elements[i + 1];
                trianglesFin[i + 2] = tesselation.Elements[i + 2];
            }
            
            mesh.vertices = verticesFin;
            mesh.triangles = trianglesFin;
            mesh.colors32 = colorsFin;
            
            return mesh;
        }

        public static float DeltaAngleRad(float current, float target)
        {
            float num = Mathf.Repeat(target - current, Mathf.PI * 2f);
            if (num > Mathf.PI)
            {
                num -= Mathf.PI * 2f;
            }
            return num;
        }
    }
}
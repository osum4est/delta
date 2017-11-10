// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace SVGImporter.Document
{
    using Rendering;
    using Utils;

    public enum SVGNodeName
    {
        Rect,
        Line,
        Circle,
        Ellipse,
        PolyLine,
        Polygon,
        Path,
        SVG,
        G,
        LinearGradient,
        RadialGradient,
        ConicalGradient,
        Defs,
        Title,
        Desc,
        Stop,
        Symbol,
        ClipPath,
        Mask,
        Image,
        Use,
        Style
    }

    public class Node
    {
        public Node parent;
        public List<Node> children;
        public SVGNodeName name;
        public AttributeList attributes;
        public int depth;
        public string content;

        public Node(SVGNodeName name, AttributeList attributes, int depth)
        {
            this.parent = null;
            this.children = new List<Node>();
            this.name = name;
            this.attributes = attributes;
            this.depth = depth;
        }

        public List<Node> GetNodes()
        {
            List<Node> nodes = new List<Node>();
            GetNodesInternal(this, nodes);
            return nodes;
        }
        
        protected void GetNodesInternal(Node node, List<Node> nodes)
        {
            if (node == null)
                return;
            
            nodes.Add(node);
            int nodeChildrenCount = node.children.Count;
            for (int i = 0; i < nodeChildrenCount; i++)
            {
                GetNodesInternal(node.children [i], nodes);
            }

            if (node is BlockOpenNode)
            {
                //Lookup(name), )
                Node endNode = new BlockCloseNode(node.name, new AttributeList(), node.depth);
                nodes.Add(endNode);
            }
        }
    }

    public class InlineNode : Node
    {
        public InlineNode(SVGNodeName name, AttributeList attributes, int depth) : base(name, attributes, depth)
        {
        }
    }

    public class BlockOpenNode : Node
    {
        public BlockOpenNode(SVGNodeName name, AttributeList attributes, int depth) : base(name, attributes, depth)
        {
        }
    }

    public class BlockCloseNode : Node
    {
        public BlockCloseNode(SVGNodeName name, AttributeList attributes, int depth) : base(name, attributes, depth)
        {
        }
    }

    public class SVGParser : SmallXmlParser.IContentHandler
    {
        public static Dictionary<string, Node> _defs;
        private SmallXmlParser _parser = new SmallXmlParser();
        private int _currentDepth = 0;
        private Node _lastParent;
    
        private static string STYLE_BLOCK;

        public SVGParser()
        {
        }

        public static void Clear()
        {
            if (_defs != null)
            {
                _defs.Clear();
                _defs = null;
            }
        }

        public static void Init()
        {
            if (_defs == null)
                _defs = new Dictionary<string, Node>();
        }

        public SVGParser(string text)
        {
            _parser.Parse(new StringReader(text), this);
        }

        public List<Node> nodes = new List<Node>();
        private int idx = 0;

        public Node node
        {
            get { return nodes [idx]; }
        }

        public bool Next()
        {
            idx++;
            return !isEOF;
        }

        public bool isEOF
        {
            get
            {
                return idx >= nodes.Count;
            }
        }

        public void OnStartParsing(SmallXmlParser parser)
        {
            idx = 0;
            _currentDepth = 0;
            _lastParent = null;

            if (dontPutInNodes == null)
            {
                dontPutInNodes = new List<SVGNodeName>();
            } else
            {
                dontPutInNodes.Clear();
            }
        }

        static List<SVGNodeName> dontPutInNodes = new List<SVGNodeName>();

        public void OnInlineElement(string name, AttributeList attrs)
        {
            Node node = new InlineNode(Lookup(name), new AttributeList(attrs), _currentDepth);
            node.parent = _lastParent;
            if (_lastParent != null)
            {
                _lastParent.children.Add(node);
            }

            string definitionID = node.attributes.GetValue("id");
            if (!string.IsNullOrEmpty(definitionID))
            {
                if (_defs.ContainsKey(definitionID))
                {
                    _defs [definitionID] = node;
                    Debug.LogWarning("Element: " + node.name + ", ID: " + definitionID + " already exists! Overwriting with new element!");
                } else
                {
                    _defs.Add(definitionID, node);
                }
            }

            if (node.name == SVGNodeName.Stop)
            {
                nodes.Add(node);
                return;
            }

            if (dontPutInNodes.Count == 0)
                nodes.Add(node);
        }

        public void OnStartElement(string name, AttributeList attrs)
        {
            Node node = new BlockOpenNode(Lookup(name), new AttributeList(attrs), _currentDepth++);
            node.parent = _lastParent;
            if (_lastParent != null)
            {
                _lastParent.children.Add(node);
            }
            _lastParent = node;

            string definitionID = node.attributes.GetValue("id");
            if (!string.IsNullOrEmpty(definitionID))
            {
                if (_defs.ContainsKey(definitionID))
                {
                    _defs [definitionID] = node;
                    Debug.LogWarning("Element: " + node.name + ", ID: " + definitionID + " already exists! Overwriting with new element!");
                } else
                {
                    _defs.Add(definitionID, node);
                }
            }

            switch (node.name)
            {
                case SVGNodeName.LinearGradient:
                case SVGNodeName.RadialGradient:
                case SVGNodeName.ConicalGradient:
                    nodes.Add(node);
                    return;                
                case SVGNodeName.Defs:
                    dontPutInNodes.Add(SVGNodeName.Defs);
                    break;
                case SVGNodeName.Symbol:
                    dontPutInNodes.Add(SVGNodeName.Symbol);
//                              Debug.LogError ("Unsupported Element type Symbol");
                    /*
                    #if UNITY_EDITOR
                    if(!SVGAssetImport.errors.Contains(SVGError.Symbol))
                        SVGAssetImport.errors.Add(SVGError.Symbol);
                    #endif
                    */
                    break;
                case SVGNodeName.Image:
                    dontPutInNodes.Add(SVGNodeName.Image);
//                              Debug.LogError ("Unsupported Element type Image");
                    #if UNITY_EDITOR
                    if(!SVGAssetImport.errors.Contains(SVGError.Image))
                        SVGAssetImport.errors.Add(SVGError.Image);
                    #endif
                    break;                
                case SVGNodeName.ClipPath:
                    dontPutInNodes.Add(SVGNodeName.ClipPath);
//                              Debug.LogError ("Unsupported Element type Clip Path");
                    #if UNITY_EDITOR
                    if(!SVGAssetImport.errors.Contains(SVGError.ClipPath))
                        SVGAssetImport.errors.Add(SVGError.ClipPath);
                    #endif
                    break;
                case SVGNodeName.Mask:
                    dontPutInNodes.Add(SVGNodeName.Mask);
//                              Debug.LogError ("Unsupported Element type Mask");
                    #if UNITY_EDITOR
                    if(!SVGAssetImport.errors.Contains(SVGError.Mask))
                        SVGAssetImport.errors.Add(SVGError.Mask);
                    #endif
                    break;
                default:
                    if (dontPutInNodes.Count == 0)
                        nodes.Add(node);
                    break;
            }

            //Debug.Log("OnStartElement: "+node.name+", depth: "+node.depth);
        }

        public void OnEndElement(string name)
        {
            Node node = new BlockCloseNode(Lookup(name), new AttributeList(), --_currentDepth);

            if (_lastParent != null)
            {
                _lastParent = _lastParent.parent;
            } else
            {
                _lastParent = null;
            }

            node.parent = _lastParent;

            //Debug.Log("OnEndElement: "+node.name+", depth: "+node.depth);

            switch (node.name)
            {
                case SVGNodeName.LinearGradient:
                case SVGNodeName.RadialGradient:
                case SVGNodeName.ConicalGradient:
                    nodes.Add(node);
                    return;                
                case SVGNodeName.Defs:
                    dontPutInNodes.RemoveAt(dontPutInNodes.Count - 1);
                    break;
                case SVGNodeName.Symbol:
                    dontPutInNodes.RemoveAt(dontPutInNodes.Count - 1);
                    break;
                case SVGNodeName.Image:
                    dontPutInNodes.RemoveAt(dontPutInNodes.Count - 1);
                    break;
                case SVGNodeName.ClipPath:
                    dontPutInNodes.RemoveAt(dontPutInNodes.Count - 1);
                    break;
                case SVGNodeName.Mask:
                    dontPutInNodes.RemoveAt(dontPutInNodes.Count - 1);
                    break;
                default:
                    if (dontPutInNodes.Count == 0)
                        nodes.Add(node);
                    break;
            }

        }

        public void OnStyleElement(string name, AttributeList attrs, string style)
        {
            Node node = new InlineNode(Lookup(name), new AttributeList(attrs), _currentDepth);
            node.content = style;
            node.parent = _lastParent;
            if (_lastParent != null)
            {
                _lastParent.children.Add(node);
            }

            nodes.Add(node);
        }

        public void GetElementList(List<object> elementList, SVGPaintable paintable, SVGTransformList summaryTransformList)
        {
            bool exitFlag = false;
            while (!exitFlag && Next())
            {
                //while (Next())
                if (node is BlockCloseNode)
                {
                    exitFlag = true;
                    continue;
                }

//                Debug.Log(node.name);
                switch (node.name)
                {
                    case SVGNodeName.Rect:
                        {
                            elementList.Add(new SVGRectElement(node,
                                                       summaryTransformList,
                                                       paintable));
                            break;
                        }
                    case SVGNodeName.Line:
                        {
                            elementList.Add(new SVGLineElement(node,
                                                       summaryTransformList,
                                                       paintable));
                            break;
                        }
                    case SVGNodeName.Circle:
                        {
                            elementList.Add(new SVGCircleElement(node,
                                                         summaryTransformList,
                                                         paintable));
                            break;
                        }
                    case SVGNodeName.Ellipse:
                        {
                            elementList.Add(new SVGEllipseElement(node,
                                                          summaryTransformList,
                                                          paintable));
                            break;
                        }
                    case SVGNodeName.PolyLine:
                        {
                            elementList.Add(new SVGPolylineElement(node,
                                                           summaryTransformList,
                                                           paintable));
                            break;
                        }
                    case SVGNodeName.Polygon:
                        {
                            elementList.Add(new SVGPolygonElement(node,
                                                          summaryTransformList,
                                                          paintable));
                            break;
                        }
                    case SVGNodeName.Path:
                        {
                            elementList.Add(new SVGPathElement(node,
                                                       summaryTransformList,
                                                       paintable));
                            break;
                        }
                    case SVGNodeName.SVG:
                        {
                            elementList.Add(new SVGElement(this,
                                                          summaryTransformList,
                                                          paintable));
                            break;
                        }
                    case SVGNodeName.Symbol:
                        {
                            elementList.Add(new SVGElement(this,
                                                          summaryTransformList,
                                                          paintable));
                            break;
                        }
                    case SVGNodeName.G:
                        {
                            elementList.Add(new SVGElement(this,
                                                    summaryTransformList,
                                                    paintable));
                            break;
                        }
                        /*
                    case SVGNodeName.ClipPath:
                        {
                            paintable.AppendClipPath(new SVGClipPathElement(this, node));
                            break;
                        }
                        */
                    case SVGNodeName.LinearGradient:
                        {
                            paintable.AppendLinearGradient(new SVGLinearGradientElement(this, node));
                            break;
                        }
                    case SVGNodeName.RadialGradient:
                        {
                            paintable.AppendRadialGradient(new SVGRadialGradientElement(this, node));
                            break;
                        }
                    case SVGNodeName.ConicalGradient:
                        {
                            paintable.AppendConicalGradient(new SVGConicalGradientElement(this, node));
                            break;
                        }
                    case SVGNodeName.Defs:
                        {
                            GetElementList(elementList, paintable, summaryTransformList);
                            break;
                        }
                    case SVGNodeName.Title:
                        {
                            GetElementList(elementList, paintable, summaryTransformList);
                            break;
                        }
                    case SVGNodeName.Desc:
                        {
                            GetElementList(elementList, paintable, summaryTransformList);
                            break;
                        }
                    case SVGNodeName.Style:
                        {                                        
                            paintable.AddCSS(node.content);                                         
                            break;
                        }
                    case SVGNodeName.Use:
                        {
//                            Debug.Log("Begin Use Command: " + node.attributes.GetValue("id"));
                            string xlink = node.attributes.GetValue("xlink:href");
                            if (!string.IsNullOrEmpty(xlink))
                            {
                                if (xlink [0] == '#')
                                    xlink = xlink.Remove(0, 1);

                                if (_defs.ContainsKey(xlink))
                                {
                                    Node definitionNode = _defs [xlink];
                                    if (definitionNode != null && definitionNode != node)
                                    {
                                        List<Node> injectNodes = definitionNode.GetNodes();
                                        if (injectNodes != null && injectNodes.Count > 0)
                                        {
                                            nodes [idx] = new BlockOpenNode(SVGNodeName.Use, node.attributes, node.depth);
                                            injectNodes.Add(new BlockCloseNode(SVGNodeName.Use, new AttributeList(), node.depth));
                                            nodes.InsertRange(idx + 1, injectNodes);

                                            elementList.Add(new SVGElement(this,
                                                                        summaryTransformList,
                                                                        paintable));
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }

        protected void InjectNode()
        {

        }

        private static SVGNodeName Lookup(string name)
        {
            SVGNodeName retVal = SVGNodeName.G;
            switch (name.ToLower())
            {
                case "rect":
                    retVal = SVGNodeName.Rect;
                    break;
                case "line":
                    retVal = SVGNodeName.Line;
                    break;
                case "circle":
                    retVal = SVGNodeName.Circle;
                    break;
                case "ellipse":
                    retVal = SVGNodeName.Ellipse;
                    break;
                case "polyline":
                    retVal = SVGNodeName.PolyLine;
                    break;
                case "polygon":
                    retVal = SVGNodeName.Polygon;
                    break;
                case "path":
                    retVal = SVGNodeName.Path;
                    break;
                case "svg":
                    retVal = SVGNodeName.SVG;
                    break;
                case "g":
                    retVal = SVGNodeName.G;
                    break;
                case "lineargradient":
                    retVal = SVGNodeName.LinearGradient;
                    break;
                case "radialgradient":
                    retVal = SVGNodeName.RadialGradient;
                    break;
                case "conicalgradient":
                    retVal = SVGNodeName.ConicalGradient;
                    break;
                case "defs":
                    retVal = SVGNodeName.Defs;
                    break;
                case "title":
                    retVal = SVGNodeName.Title;
                    break;
                case "desc":
                    retVal = SVGNodeName.Desc;
                    break;
                case "stop":
                    retVal = SVGNodeName.Stop;
                    break;
                case "symbol":
                    retVal = SVGNodeName.Symbol;
                    break;
                case "clippath":
                    retVal = SVGNodeName.ClipPath;
                    break;
                case "mask":
                    retVal = SVGNodeName.Mask;
                    break;
                case "image":
                    retVal = SVGNodeName.Image;
                    break;
                case "use":
                    retVal = SVGNodeName.Use;
                    break;
                case "style":
                    retVal = SVGNodeName.Style;
                    break;
                default:
                    retVal = SVGNodeName.G;
#if UNITY_EDITOR
//                    Debug.LogError("Unknown element type '" + name + "'!");
//                    SVGAssetImport.errors.Add(SVGError.Unknown);
#endif
                    break;
            }
            return retVal;
        }
    }
}

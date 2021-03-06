﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2017/04/23 19:32
// License Copyright (c) Daniele Giardini

using System;
using System.Collections.Generic;
using DG.DemiLib;
using UnityEditor;

namespace DG.DemiEditor.DeGUINodeSystem.Core
{
    /// <summary>
    /// Stores cloned nodes for pasting
    /// </summary>
    internal class NodesClipboard
    {
        public readonly List<IEditorGUINode> nodes = new List<IEditorGUINode>();
        public readonly Dictionary<IEditorGUINode,string> nodeToOriginalId = new Dictionary<IEditorGUINode,string>();
        public readonly Dictionary<IEditorGUINode,IEditorGUINode> nodeToOriginalNode = new Dictionary<IEditorGUINode,IEditorGUINode>();
        public readonly Dictionary<IEditorGUINode,NodeGUIData> nodeToOriginalGuiData = new Dictionary<IEditorGUINode,NodeGUIData>();
        public readonly Dictionary<IEditorGUINode,NodeConnectionOptions> nodeToConnectionOptions = new Dictionary<IEditorGUINode,NodeConnectionOptions>();
        public bool hasContent { get { return EditorGUIUtility.systemCopyBuffer == _copyBufferId && nodes.Count > 0; } }

        readonly string _copyBufferId; // Store in OS clipboard to indicate that there's nodes ready to be pasted for this process
        NodeProcess _process;

        #region CONSTRUCTOR

        public NodesClipboard(NodeProcess process)
        {
            _process = process;
            _copyBufferId = "[[[[!?NODES?!]]]]" + Guid.NewGuid().ToString();
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            nodes.Clear();
            nodeToOriginalId.Clear();
            nodeToOriginalNode.Clear();
            nodeToOriginalGuiData.Clear();
            nodeToConnectionOptions.Clear();
        }

        public void Add(IEditorGUINode node, IEditorGUINode originalNode, NodeConnectionOptions connectionOptions)
        {
            nodes.Add(node);
            nodeToOriginalId.Add(node, originalNode.id);
            nodeToOriginalNode.Add(node, originalNode);
            nodeToOriginalGuiData.Add(node, _process.nodeToGUIData[originalNode]);
            nodeToConnectionOptions.Add(node, connectionOptions);
            EditorGUIUtility.systemCopyBuffer = _copyBufferId;
        }

        public IEditorGUINode GetClipboardCloneByOriginalId(string id)
        {
            foreach (IEditorGUINode node in nodes) {
                if (nodeToOriginalId[node] == id) return node;
            }
            return null;
        }

        #endregion
    }
}
﻿/*
 * Copyright (c) MDD4All.de, Dr. Oliver Alt
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class NodeManipulationExtensions
    {
		public static void AddChildNode(this Node node, Node childNode)
		{
			if(node.Nodes == null)
			{
				node.Nodes = new List<Node>();
			}
			node.Nodes.Add(childNode);
		}

		public static void AddChildNode(this Node node, Resource childResource)
        {
			if (node.Nodes == null)
			{
				node.Nodes = new List<Node>();
			}
			Node childNode = new Node();

			childNode.ResourceReference = new Key(childResource.ID, childResource.Revision);

			node.Nodes.Add(childNode);
		}
        
    }
}

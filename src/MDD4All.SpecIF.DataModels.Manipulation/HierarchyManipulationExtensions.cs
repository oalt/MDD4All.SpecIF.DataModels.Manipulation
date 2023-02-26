/*
 * Copyright (c) MDD4All.de, Dr. Oliver Alt
 */

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class HierarchyManipulationExtensions
    {
		public static Node GetNodeByID(this Node hierarchy, string id)
		{
			Node result = null;

			if (hierarchy.ID == id)
			{
				result = hierarchy;
			}
			else
			{
				foreach (Node rootNode in hierarchy.Nodes)
				{
					FindNodeRecursively(rootNode, id, ref result);
					if (result != null)
					{
						break;
					}

				}
			}

			return result;
		}

        public static Node GetParentNode(this SpecIF specif, string childNode)
        {
            Node result = null;

            foreach (Node hierarchy in specif.Hierarchies)
            {

                FindParentNodeRecusrsively(hierarchy, childNode, ref result);
                if (result != null)
                {
                    break;
                }
            }
            
            return result;
        }

        private static void FindNodeRecursively(Node node, string id, ref Node result)
		{
			if(node.ID == id)
			{
				result = node;
			}
			else
			{
				if (node.Nodes != null)
				{
					foreach (Node child in node.Nodes)
					{
						FindNodeRecursively(child, id, ref result);
					}
				}
			}
		}

        public static void FindParentNodeRecusrsively(Node currentNode, string id, ref Node result)
        {
            foreach (Node childNode in currentNode.Nodes)
            {
                if (childNode.ID == id)
                {
                    result = currentNode;
                    break;
                }
            }

            if (result == null)
            {
                foreach (Node childNode in currentNode.Nodes)
                {
                    FindParentNodeRecusrsively(childNode, id, ref result);
                }
            }

        }

        //public static string GetResourceIdentifierPrefix(this Hierarchy hierarchy, ISpecIfMetadataReader dataProvider)
        //{
        //	string result = "";

        //	try
        //	{
        //		string prefix = hierarchy.Properties.FirstOrDefault(prop => prop.Title == "identifierPrefix")?.GetStringValue(dataProvider);

        //		if (prefix != null)
        //		{
        //			result = prefix;
        //		}
        //	}
        //	catch(Exception)
        //	{

        //	}

        //	return result;
        //}
    }
}

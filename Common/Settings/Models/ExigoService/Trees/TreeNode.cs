using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class TreeNode : ITreeNode
    {
        public Guid NodeID
        {
            get
            {
                if (_nodeID == Guid.Empty) _nodeID = Guid.NewGuid();
                return _nodeID;
            }
        }
        private Guid _nodeID;
        public int CustomerID { get; set; }

        public Guid ParentNodeID { get; set; }
        public int ParentCustomerID { get; set; }

        public int Level { get; set; }
        public int PlacementID { get; set; }
        public int IndentedSort { get; set; }

        public int ChildNodeCount { get; set; }
        public bool HasChildren
        {
            get { return ChildNodeCount > 0; }
        }

        public bool IsNullPosition { get; set; }
        public bool IsOpenPosition { get; set; }
    }
}
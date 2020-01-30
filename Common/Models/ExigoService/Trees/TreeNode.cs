using System;

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
        public int Volume14 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MainCountry { get; set; }
        public string Status { get; set; }
        public string NodeLabel
        {
            get
            {
                return string.Format("<span class='k-sprite {3}'></span> {0} - {1} {2}",
                    CustomerID.ToString(),
                    FirstName,
                    LastName,
                    Status = Volume14 > 0 ? "active" : ""
                    );
            }
        }
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

        public bool IsNullPosition
        {
            get { return (this.CustomerID == 0 && this.ParentCustomerID == 0) ? true : false; }
            set { }
        }
        public bool IsOpenPosition
        {
            get { return (this.CustomerID == 0 && this.ParentCustomerID != 0) ? true : false; }
            set { }
        }
    }
}
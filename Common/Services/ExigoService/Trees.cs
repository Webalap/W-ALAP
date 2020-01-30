using Common;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        #region Downline Trees
        // Getting tree nodes
        public static IEnumerable<T> GetUniLevelTree<T>(GetTreeRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"GetUniLevelTree {0},{1},{2},{3},{4}", request.CustomerID, request.TopCustomerID, request.CustomerTypeID, request.Levels, PeriodTypes.Monthly);
                nodes = context.Query<T>(sqlProcedure).ToList();
            }

            return nodes;
        }
        public static IEnumerable<T> GetBinaryTree<T>(GetTreeRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            var rowcount = 50;
            var lastResultCount = rowcount;
            var callsMade = 0;

            while (lastResultCount == rowcount)
            {
                //var query = Exigo.OData().BinaryTree
                //    .Where(c => c.TopCustomerID == request.TopCustomerID);

                //// Filter by level
                //var levels = (request.Levels != 0) ? request.Levels : 15;
                //query = query.Where(c => c.Level <= levels);

                //// Filter by legs
                //if (request.Legs != 0)
                //{
                //    query = query.Where(c => c.Placement <= request.Legs);
                //}

                //// Get the data
                //var results = query
                //    .OrderBy(c => c.IndentedSort)
                //    .Skip(callsMade * rowcount)
                //    .Take(rowcount)
                //    .Select(c => c)
                //    .ToList();


                //results.ForEach(c =>
                //{
                //    var node = (T)Activator.CreateInstance(typeof(T));
                //    node.CustomerID = c.CustomerID;
                //    node.ParentCustomerID = c.ParentID;
                //    node.Level = c.Level;
                //    node.PlacementID = c.Placement;
                //    node.IndentedSort = c.IndentedSort;
                //    node.ChildNodeCount = c.ChildCount;

                //    nodes.Add(node);
                //});
                var levels = (request.Levels != 0) ? request.Levels : 15;
                var skip = callsMade * rowcount;
                var take = rowcount;
                var placement = request.Legs != 0 ? request.Legs : (int?)null;

                using (SqlConnection context = Exigo.Sql())
                {
                    context.Open();
                    SqlCommand cmd = new SqlCommand("dbo.GetBinaryTree", context);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@topcustomerid", request.TopCustomerID));
                    cmd.Parameters.Add(new SqlParameter("@level", levels));
                    cmd.Parameters.Add(new SqlParameter("@skip", skip));
                    cmd.Parameters.Add(new SqlParameter("@take", take));
                    cmd.Parameters.Add(new SqlParameter("@placement", placement));


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var node = (T)Activator.CreateInstance(typeof(T));
                            node.CustomerID = (int)rdr["DownLines"];
                            node.ParentCustomerID = (int)rdr["ParentID"];
                            node.Level = (int)rdr["Level"];
                            node.PlacementID = (int)rdr["Placement"];
                            node.IndentedSort = (int)rdr["IndentedSort"];
                            node.ChildNodeCount = (int)rdr["ChildCount"];

                            nodes.Add(node);
                        }
                    }

                    callsMade++;
                    lastResultCount = nodes.Count;
                }
            }

            return nodes;
        }
        public static IEnumerable<T> GetEnrollerTree<T>(GetTreeRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            var rowcount = 50;
            var lastResultCount = rowcount;
            var callsMade = 0;

            while (lastResultCount == rowcount)
            {
                //var query = Exigo.OData().EnrollerTree
                //    .Where(c => c.TopCustomerID == request.TopCustomerID);

                //// Filter by level
                var levels = (request.Levels != 0) ? request.Levels : 15;
                //query = query.Where(c => c.Level <= levels);

                //// Get the data
                //var results = query
                //    .OrderBy(c => c.IndentedSort)
                //    .Skip(callsMade * rowcount)
                //    .Take(rowcount)
                //    .Select(c => c)
                //    .ToList();

                //results.ForEach(c =>
                //{
                //    var node = (T)Activator.CreateInstance(typeof(T));
                //    node.CustomerID = c.CustomerID;
                //    node.ParentCustomerID = c.EnrollerID;
                //    node.Level = c.Level;
                //    node.PlacementID = 0;
                //    node.IndentedSort = c.IndentedSort;
                //    node.ChildNodeCount = c.ChildCount;

                //    nodes.Add(node);
                //});
                var skip = callsMade * rowcount;
                var take = rowcount;
                //var customerTypeID = (int)CustomerTypes.IndependentStyleAmbassador;
                using (SqlConnection context = Exigo.Sql())
                {
                    context.Open();
                    SqlCommand cmd = new SqlCommand("dbo.GetDownLineTree", context);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@topcustomerid", request.TopCustomerID));
                    cmd.Parameters.Add(new SqlParameter("@level", levels));
                    cmd.Parameters.Add(new SqlParameter("@skip", skip));
                    cmd.Parameters.Add(new SqlParameter("@take", take));


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var node = (T)Activator.CreateInstance(typeof(T));
                            node.CustomerID = (int)rdr["DownLines"]; 
                            node.ParentCustomerID = (int)rdr["ParentID"];
                            node.Level = (int)rdr["Level"];
                            node.PlacementID = 0;
                            node.IndentedSort = (int)rdr["IndentedSort"];
                            node.ChildNodeCount = (int)rdr["ChildCount"];

                            nodes.Add(node);
                        }
                    }

                    callsMade++;
                    lastResultCount = nodes.Count;
                }
            }

            return nodes;
        }
        public static IEnumerable<T> GetGenealogyTree<T>(GetTreeRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            var rowcount = 50;
            var lastResultCount = rowcount;
            var callsMade = 0;

            while (lastResultCount == rowcount)
            {
                //var query = Exigo.OData().EnrollerTree
                //    .Where(c => c.TopCustomerID == request.TopCustomerID);

                //// Filter by level
                //var levels = (request.Levels != 0) ? request.Levels : 10;
                //query = query
                //    .Where(c => c.Level <= levels);

                //// Filter by customer types
                //var allowableCustomerTypes = new List<int>() { (int)CustomerTypes.IndependentStyleAmbassador };
                //query = query
                //    .Where(allowableCustomerTypes.ToOrExpression<EnrollerNode, int>("Customer.CustomerTypeID"));

                //// Get the data
                //var results = query
                //    .OrderBy(c => c.IndentedSort)
                //    .Skip(callsMade * rowcount)
                //    .Take(rowcount)
                //    .Select(c => c)
                //    .ToList();

                //results.ForEach(c =>
                //{
                //    var node = (T)Activator.CreateInstance(typeof(T));
                //    node.CustomerID = c.CustomerID;
                //    node.ParentCustomerID = c.EnrollerID;
                //    node.Level = c.Level;
                //    node.PlacementID = 0;
                //    node.IndentedSort = c.IndentedSort;
                //    node.ChildNodeCount = c.ChildCount;

                //    nodes.Add(node);
                //});

                //callsMade++;
                //lastResultCount = results.Count;
                var levels = (request.Levels != 0) ? request.Levels : 15;
                var skip = callsMade * rowcount;
                var take = rowcount;
                var customerTypeID = (int)CustomerTypes.IndependentStyleAmbassador;

                using (SqlConnection context = Exigo.Sql())
                {
                    context.Open();
                    SqlCommand cmd = new SqlCommand("dbo.GetDownLineTree", context);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@topcustomerid", request.TopCustomerID));
                    cmd.Parameters.Add(new SqlParameter("@level", levels));
                    cmd.Parameters.Add(new SqlParameter("@skip", skip));
                    cmd.Parameters.Add(new SqlParameter("@take", take));
                    cmd.Parameters.Add(new SqlParameter("@customertypeid", customerTypeID));


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var node = (T)Activator.CreateInstance(typeof(T));
                            node.CustomerID = (int)rdr["DownLines"];
                            node.ParentCustomerID = (int)rdr["ParentID"];
                            node.Level = (int)rdr["Level"];
                            node.PlacementID = 0;
                            node.IndentedSort = (int)rdr["IndentedSort"];
                            node.ChildNodeCount = (int)rdr["ChildCount"];

                            nodes.Add(node);
                        }
                    }

                    callsMade++;
                    lastResultCount = nodes.Count;
                }
            
            
            }

            return nodes;
        }

        // Getting Bottom Nodes of Trees
        public static TreeNode GetBottomUniLevelTreeNode(GetBottomTreeNodeRequest request)
        {
            var bottomNode = new TreeNode();
            using (var context = Exigo.Sql())
            {
                bottomNode = context.Query<TreeNode>(@"
                        With tree (CustomerID, SponsorID, placement,  nestedlevel)
                        as
                        (
                            Select CustomerID, SponsorID, placement, nestedlevel
                            from unileveltree 
                            where  customerid = @customerID 
                            union all
                            select u.CustomerID, u.SponsorID, u.placement, u.nestedlevel
                            from unileveltree u
                            inner join tree t on t.customerid = u.sponsorid
                                    and u.placement = @placementid
                        )
                        select top 1 CustomerID From tree  order by nestedlevel desc
                        option (maxrecursion 0)
                        ", new
                         {
                             customerID = request.TopCustomerID,
                             placementid = request.PlacementID
                         }).FirstOrDefault();
            }
            return bottomNode;
        }
        public static TreeNode GetBottomBinaryTreeNode(GetBottomTreeNodeRequest request)
        {
            var bottomNode = new TreeNode();
            using (var context = Exigo.Sql())
            {
                bottomNode = context.Query<TreeNode>(@"
                        With tree (CustomerID, ParentID, placement,  nestedlevel)
                        as
                        (
                            Select CustomerID, ParentID, placement, nestedlevel
                            from BinaryTree 
                            where  customerid = @customerID 
                            union all
                            select u.CustomerID, u.ParentID, u.placement, u.nestedlevel
                            from BinaryTree u
                            inner join tree t on t.customerid = u.ParentID
                                    and u.placement = @placementid
                        )
                        select top 1 CustomerID From tree order by nestedlevel desc
                        option (maxrecursion 0)
                        ", new
                         {
                             customerID = request.TopCustomerID,
                             placementid = request.PlacementID
                         }).FirstOrDefault();
            }
            return bottomNode;
        }

        // Validators
        public static bool IsPersonallyEnrolled(int topCustomerID, int customerID) 
        {
            //return Exigo.OData().EnrollerTree
            //    .Where(c => c.TopCustomerID == topCustomerID)
            //    .Where(c => c.CustomerID == customerID)
            //    .Count() != 0;

            List<GetEnrollerTree> nodes = new List<GetEnrollerTree>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"IsDownlineTree {0},{1}", topCustomerID, customerID);
                nodes = context.Query<GetEnrollerTree>(sqlProcedure).ToList();
            }

            return nodes.Count != 0;
        }
        public static bool IsCustomerInUniLevelTree(int customerID)
        {
            //return Exigo.OData().UniLevelTree
            //    .Where(c => c.CustomerID == customerID)
            //    .Count() != 0;
            int topCustomerID = customerID;
            List<GetEnrollerTree> nodes = new List<GetEnrollerTree>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"IsDownlineTree {0}", topCustomerID);
                nodes = context.Query<GetEnrollerTree>(sqlProcedure).ToList();
            }

            return nodes.Count != 0;
        }

        ////////////////////////////////////////////////////
        ///// This is the same as IsPersonallyEnrolled//////
        ////////////////////////////////////////////////////
        //public static bool IsCustomerInUniLevelDownline(int topCustomerID, int customerID)
        //{
        //    if (customerID == topCustomerID) return true;

        //    return Exigo.OData().UniLevelTree
        //        .Where(c => c.TopCustomerID == topCustomerID)
        //        .Where(c => c.CustomerID == customerID)
        //        .Count() != 0;
        //}
        public static bool IsCustomerInBinaryTree(int customerID)
        {
            //return Exigo.OData().BinaryTree
            //    .Where(c => c.TopCustomerID == customerID)
            //    .Count() != 0;

            int topCustomerID = customerID;
            List<GetEnrollerTree> nodes = new List<GetEnrollerTree>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"IsDownlineBinaryTree {0},{1}", topCustomerID);
                nodes = context.Query<GetEnrollerTree>(sqlProcedure).ToList();
            }
            return nodes.Count != 0;
        }
        public static bool IsCustomerInBinaryDownline(int topCustomerID, int customerID)
        {
            //if (customerID == topCustomerID) return true;

            //return Exigo.OData().BinaryTree
            //    .Where(c => c.TopCustomerID == topCustomerID)
            //    .Where(c => c.CustomerID == customerID)
            //    .Count() != 0;

            List<GetEnrollerTree> nodes = new List<GetEnrollerTree>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"IsDownlineBinaryTree {0},{1}", topCustomerID, customerID);
                nodes = context.Query<GetEnrollerTree>(sqlProcedure).ToList();
            }

            return nodes.Count != 0;
            
        }
        #endregion

        #region Upline Trees
        public static IEnumerable<T> GetEnrollerUplineTree<T>(GetUplineRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            using (var context = Exigo.Sql())
            {
                nodes = context.Query<T>(@"
                SELECT 
                        up.CustomerID,
                        ParentCustomerID = up.EnrollerID,
                        up.Level
                FROM EnrollerUpline up
                WHERE 
                    up.UplineCustomerID = @bottomcustomerid
                ORDER BY Level DESC",
                    new
                    {
                        bottomcustomerid = request.BottomCustomerID,
                        topcustomerid = request.TopCustomerID
                    }).ToList();
            }

            // Filter out the nodes that don't belong
            var isFound = false;
            var filteredNodes = new List<T>();
            foreach (var node in nodes)
            {
                filteredNodes.Add(node);
                if (node.CustomerID == request.TopCustomerID)
                {
                    isFound = true;
                    break;
                }
            }
            if (!isFound) return new List<T>();


            // Re-order the nodes (by level ascending)
            nodes = filteredNodes.OrderBy(c => c.Level).ToList();


            // Set the levels
            var nodeCount = nodes.Count - 1;
            foreach (var node in nodes)
            {
                node.Level = node.Level + nodeCount;
            }

            return nodes;
        }
        public static IEnumerable<T> GetUniLevelUplineTree<T>(GetUplineRequest request) where T : ITreeNode
        {
            // Get the nodes
            var nodes = new List<T>();
            using (var context = Exigo.Sql())
            {
                nodes = context.Query<T>(@"
                SELECT 
                        up.CustomerID,
                        ParentCustomerID = up.SponsorID,
                        up.Level
                FROM UniLevelUpline up
                WHERE 
                    up.UplineCustomerID = @bottomcustomerid
                ORDER BY Level DESC",
                    new
                    {
                        bottomcustomerid = request.BottomCustomerID,
                        topcustomerid = request.TopCustomerID
                    }).ToList();
            }

            // Filter out the nodes that don't belong
            var isFound = false;
            var filteredNodes = new List<T>();
            foreach (var node in nodes)
            {
                filteredNodes.Add(node);
                if (node.CustomerID == request.TopCustomerID)
                {
                    isFound = true;
                    break;
                }
            }
            if (!isFound) return new List<T>();


            // Re-order the nodes (by level ascending)
            nodes = filteredNodes.OrderBy(c => c.Level).ToList();


            // Set the levels
            var nodeCount = nodes.Count - 1;
            foreach (var node in nodes)
            {
                node.Level = node.Level + nodeCount;
            }

            return nodes;
        }
        #endregion

        #region Tree Placements
        public static void PlaceUniLevelCustomer(PlaceUniLevelCustomerRequest request)
        {
            // Create our request
            var apiRequest = new PlaceUniLevelNodeRequest()
            {
                CustomerID = request.CustomerID,
                ToSponsorID = request.ToSponsorID,
                Reason = request.Reason,
                OptionalFindAvailable = request.FindNextAvailablePlacement
            };

            // Handle the optional filters
            if (request.Placement != null)
            {
                apiRequest.OptionalPlacement = (int)request.Placement;
            }
            if (request.BuildTypeID != null)
            {
                apiRequest.OptionalUnilevelBuildTypeID = (int)request.BuildTypeID;
            }


            // Place the node
            Exigo.WebService().PlaceUniLevelNode(apiRequest);


            // Update the customer's field to indicate they have been moved.
            Exigo.WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = request.CustomerID,
                Field1 = DateTime.Now.ToUniversalTime().ToString()
            });
        }
        #endregion

        #region Waiting Room
        public static IEnumerable<WaitingRoomNode> GetCustomerWaitingRoom(GetCustomerWaitingRoomRequest request)
        {
            #region ODATA Call
            //var customers = Exigo.OData().EnrollerTree
            //    .Where(c => c.TopCustomerID == request.EnrollerID)
            //    .Where(c => c.Customer.SponsorID == null)
            //    .Where(c => c.Customer.CustomerStatusID == CustomerStatuses.Active)
            //    .Where(c => c.Customer.Field1 == string.Empty)
            //    .OrderByDescending(c => c.Customer.CreatedDate)
            //    .Select(c => new WaitingRoomNode()
            //    {
            //        CustomerID = c.CustomerID,
            //        FirstName = c.Customer.FirstName,
            //        LastName = c.Customer.LastName,
            //        EnrollerID = c.EnrollerID,
            //        EnrollmentDate = c.Customer.CreatedDate,
            //        City = c.Customer.MainCity,
            //        State = c.Customer.MainState,
            //        Country = c.Customer.MainCountry,
            //        Phone = c.Customer.Phone,
            //        Email = c.Customer.Email
            //    })
            //    .ToList();

            //// Filter out the customer that are no longer placeable
            //customers = customers.Where(c => c.EnrollmentDate.AddDays(request.GracePeriod) > DateTime.Now).ToList();
            #endregion
            #region SQL Procedure
            var customers = new List<WaitingRoomNode>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomerWaitingRoom {0},{1}", request.EnrollerID,request.GracePeriod);
                customers = context.Query<WaitingRoomNode>(sqlProcedure).ToList();
            }
            #endregion
            return customers;
        }
        public static void RemoveCustomerFromWaitingRoom(int customerID)
        {
            Exigo.WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                Field1 = DateTime.Now.ToString()
            });
        }
        public static bool IsCustomerInWaitingRoom(int customerID)
        {
            return !IsCustomerInUniLevelTree(customerID);
        }
        #endregion

        #region Placement Preferences
        public static void SetCustomerBinaryPlacementPreference(int customerID, BinaryPlacementType placementType)
        {
            Exigo.WebService().SetBinaryPreference(new SetBinaryPreferenceRequest
            {
                CustomerID = customerID,
                PlacementType = placementType
            });
        }
        #endregion



        #region Tree Helpers
        public static IEnumerable<T> OrganizeNestedTreeNodes<T>(IEnumerable<T> nodes, GetTreeRequest request) where T : INestedTreeNode<T>
        {
            var topNode = nodes.FirstOrDefault();
            if (topNode == null) yield return topNode;

            topNode.Children.AddRange(GetChildren<T>(topNode, nodes.Where(c => c.Level > topNode.Level), request).OrderBy(c => c.PlacementID));

            // Populate null positions if applicable
            if (request.IncludeNullPositions)
            {
                PopulateNullPositions<T>(topNode, request);
            }

            yield return topNode;
        }
        private static IEnumerable<T> GetChildren<T>(T parentNode, IEnumerable<T> nodes, GetTreeRequest request) where T : INestedTreeNode<T>
        {
            var parentNodeChildren = new List<T>();
            var currentLevel = parentNode.Level + 1;


            foreach (var node in nodes)
            {
                // Is this a child?
                var isChild =
                        node.Level == currentLevel
                    && node.ParentCustomerID == parentNode.CustomerID;

                if (isChild)
                {
                    node.ParentNodeID = parentNode.NodeID;
                    node.Children.AddRange(GetChildren(node, nodes.Where(c => c.Level > node.Level), request));
                    parentNodeChildren.Add(node);
                }
            }


            // Insert open position nodes if applicable
            if (request.IncludeOpenPositions && request.Legs > 0 && currentLevel < request.Levels && parentNodeChildren.Count < request.Legs)
            {
                var placementID = 0;
                while (placementID < request.Legs)
                {
                    if (!parentNodeChildren.Any(c => c.PlacementID == placementID))
                    {
                        // Create an empty node
                        parentNodeChildren.Add((T)CreateOpenPositionTreeNode<T>(parentNode, currentLevel, placementID));
                    }

                    placementID++;
                }
            }


            return parentNodeChildren.OrderBy(c => c.PlacementID);
        }
        private static void PopulateNullPositions<T>(T parentNode, GetTreeRequest request) where T : INestedTreeNode<T>
        {
            if (parentNode.Children.Count() < request.Legs)
            {
                var placementID = 0;
                while (placementID < request.Legs)
                {
                    if (!parentNode.Children.Any(c => c.PlacementID == placementID))
                    {
                        var nullNode = Activator.CreateInstance<T>();
                        if (parentNode.IsOpenPosition) nullNode = CreateNullPositionTreeNode<T>(parentNode, parentNode.Level + 1, placementID);
                        else nullNode = CreateOpenPositionTreeNode<T>(parentNode, parentNode.Level + 1, placementID);
                        parentNode.Children.Add(nullNode);
                    }

                    placementID++;
                }
            }


            foreach (var child in parentNode.Children)
            {
                if ((child.IsOpenPosition || child.IsNullPosition) && child.Level < request.Levels)
                {
                    var placementID = 0;
                    while (placementID < request.Legs)
                    {
                        var nullNode = CreateNullPositionTreeNode<T>(child, child.Level + 1, placementID);
                        child.Children.Add(nullNode);

                        placementID++;
                    }
                }
            }

            parentNode.Children = parentNode.Children.OrderBy(c => c.PlacementID).ToList();


            if (parentNode.Level + 1 < request.Levels)
            {
                foreach (var child in parentNode.Children)
                {
                    PopulateNullPositions<T>((T)child, request);
                }
            }
        }
        private static T CreateOpenPositionTreeNode<T>(ITreeNode parentNode, int level, int placementID) where T : INestedTreeNode<T>
        {
            var node = Activator.CreateInstance<T>();
            node.ParentNodeID = parentNode.NodeID;
            node.ParentCustomerID = parentNode.CustomerID;
            node.CustomerID = 0;
            node.Level = level;
            node.PlacementID = placementID;

            return node;
        }
        private static T CreateNullPositionTreeNode<T>(ITreeNode parentNode, int level, int placementID) where T : INestedTreeNode<T>
        {
            var node = Activator.CreateInstance<T>();
            node.ParentNodeID = parentNode.NodeID;
            node.ParentCustomerID = 0;
            node.CustomerID = 0;
            node.Level = level;
            node.PlacementID = placementID;

            return node;
        }
        #endregion
    }
}
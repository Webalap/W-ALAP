using Common.SforceServices;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Common.Services
{
    public  class SalesForceLeads
    {
        public  void CreateSalesForceLead(Lead lead)
        {
            SforceService SfdcBinding = SalesForceSession();
            Lead sfdcLead = new Lead();
            sfdcLead = lead;           
            try
            {
                //Campaign compaign = new Campaign();
                //compaign.Name="IndiaHicks";
                //sfdcLead.c = compaign;
               
                SaveResult[] saveResults = SfdcBinding.create(new sObject[] { sfdcLead });
                if (saveResults[0].success)
                {
                    string Id = "";
                    Id = saveResults[0].id;
                }
                else
                {
                    string result = "";
                    result = saveResults[0].errors[0].message;
                }
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SalesForceQualifiedAmbassadors()
        {
            SforceService SfdcBinding = SalesForceSession();
            // GET LIST OF Qualified Ambassadors FROM DATABASE AND INSERT INTO sALESFORCE 

            List<QualifiedAmbassadors> customers = new List<QualifiedAmbassadors>();
            using (var context = Exigo.Sql())
            {
                string sqlQuery = string.Format(@"select * from dbo.AmbassadorsList order by customerid asc ");
                customers = context.Query<QualifiedAmbassadors>(sqlQuery).ToList();
            }
            if (customers.Count() == 0) return;
            List<Ambassador__c> Ambassador = new List<Ambassador__c>();
            foreach (QualifiedAmbassadors Customer in customers)
            {
                Ambassador__c node = new Ambassador__c();
                node.AmbassadorID__c = Customer.CustomerID.ToString();
                node.Name = Customer.FirstName + " " + Customer.LastName;
                if (!string.IsNullOrEmpty(Customer.JoinDate))
                {
                    
                    try
                    {
                        node.JoinDate__cSpecified = true;
                        node.JoinDate__c = DateTime.Parse(Customer.JoinDate);

                    }
                    catch (Exception) { }

                }
                if (!string.IsNullOrEmpty(Customer.LeaveDate))
                {
                    try
                    {
                        node.LeaveDate__cSpecified = true;
                        node.LeaveDate__c = DateTime.Parse(Customer.LeaveDate);
                    }
                    catch (Exception) { }
                }
                node.webalias__c = Customer.WebAlias;
                node.Q_Ambassador_Email__c = Customer.email;
                node.Zip_Code__c = Customer.MainZip;
                node.Phone__c = Customer.phone;
                node.Mobile_Phone__c = Customer.MobilePhone;
                node.SponsorID__cSpecified = true;
                node.SponsorID__c = double.Parse(Customer.EnrollerID.ToString());
                node.Available_To_Receive_Leads__cSpecified = true;
                node.Available_To_Receive_Leads__c = true;
                node.CreatedDate = DateTime.Now;
                //   node.CreatedById = "00536000000zbwiAAA";
                Ambassador.Add(node);

            }
            if (Ambassador.Count > 0)
            {
                try
                {

                    IEnumerable<List<Ambassador__c>> listambassadorlist = splitList(Ambassador.ToList());
                    foreach (List<Ambassador__c> list in listambassadorlist)
                    {
                        SaveResult[] saveResults = SfdcBinding.create(list.ToArray());
                        if (saveResults[0].success)
                        {
                            string Id = "";
                            Id = saveResults[0].id;
                        }
                        else
                        {
                            string result = "";
                            result = saveResults[0].errors[0].message;
                        }
                    }

                }
                catch (Exception ex )
                {

                    throw;
                }
            }

        }
        public void UpdateSalesForceQualifiedAmbassadors()
        {
            SforceService SfdcBinding = SalesForceSession();
            QueryResult qr = CurrentSalesForceQualifiedAmbassadorslist();
            Common.SforceServices.sObject[] records = qr.records;
            if (records == null) return;
            // GET LIST OF Qualified Ambassadors FROM DATABASE AND INSERT INTO sALESFORCE 

            List<QualifiedAmbassadors> customers = new List<QualifiedAmbassadors>();
            using (var context = Exigo.Sql())
            {
                string sqlQuery = string.Format(@"select * from dbo.AmbassadorsList order by customerid asc ");
                customers = context.Query<QualifiedAmbassadors>(sqlQuery).ToList();
            }
            if (customers.Count()==0) return;
            List<Ambassador__c> Ambassador = new List<Ambassador__c>();
            
                foreach (Ambassador__c node in records)
                {

                    QualifiedAmbassadors Customer = customers.Where(s => s.CustomerID == int.Parse(node.AmbassadorID__c)).FirstOrDefault();
                    if (Customer.CustomerID == 0) break;
                    node.Name = Customer.FirstName + " " + Customer.LastName;
                    if (!string.IsNullOrEmpty(Customer.JoinDate))
                    {
                        try
                        {
                            node.JoinDate__c = DateTime.Parse(Customer.JoinDate);
                        }
                        catch (Exception) { }
                    }
                    if (!string.IsNullOrEmpty(Customer.LeaveDate))
                    {
                        try
                        {
                            node.LeaveDate__c = DateTime.Parse(Customer.LeaveDate);
                        }
                        catch (Exception) { }
                    }
                    node.webalias__c = Customer.WebAlias;
                    node.Q_Ambassador_Email__c = Customer.email;
                    node.Zip_Code__c = Customer.MainZip;
                    node.Phone__c = Customer.phone;
                    node.Mobile_Phone__c = Customer.MobilePhone;
                    node.SponsorID__cSpecified = true;
                    node.SponsorID__c = double.Parse(Customer.EnrollerID.ToString());
                    //   node.CreatedById = "00536000000zbwiAAA";
                    Ambassador.Add(node);
                }
            if (Ambassador.Count > 0)
            {
                try
                {
                    
                    IEnumerable<List<Ambassador__c>> listambassadorlist = splitList(Ambassador.ToList());
                    foreach (List<Ambassador__c> list in listambassadorlist)
                    {
                        SaveResult[] saveResults = SfdcBinding.update(list.ToArray());
                        if (saveResults[0].success)
                        {
                            string Id = "";
                            Id = saveResults[0].id;
                        }
                        else
                        {
                            string result = "";
                            result = saveResults[0].errors[0].message;
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public QueryResult GetSalesForceQualifiedAmbassadorslist(string whereClause="")
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();

            try
            {
                if (!string.IsNullOrEmpty(whereClause))
                {
                    whereClause = whereClause.Replace("QualifiedAmbassadorID", "AmbassadorID__c");
                    whereClause = whereClause.Replace("ReceivedLeads", "Available_To_Receive_Leads__c");
                    whereClause = whereClause.Replace("Zip", "Zip_Code__c");
                    whereClause = whereClause.Replace("Email", "Q_Ambassador_Email__c");
                    whereClause = whereClause.Replace("Mobile", "Mobile_Phone__c");
                    whereClause = whereClause.Replace("Phone", "Phone__c");
                    whereClause = whereClause.Replace("Sponsorid", "SponsorID__c");
                    whereClause = whereClause.Replace("LeaveDate", "LeaveDate__c");
                    whereClause = whereClause.Replace("JoinDate", "JoinDate__c");
                whereClause = whereClause.Replace("''", "'");
                }
                string strQuery = (@"select id, Name, AmbassadorID__c, Mobile_Phone__c, Phone__c,JoinDate__c,LeaveDate__c, SponsorID__c, Q_Ambassador_Email__c, Zip_Code__c, Available_To_Receive_Leads__c, CreatedById from Ambassador__c where id !='' " + whereClause + @" order by AmbassadorID__c ");
                qr = SfdcBinding.query(strQuery);
                return qr;
            }
            catch (Exception)
            {
                return qr;
            }

        }
        public static IEnumerable<List<Ambassador__c>> splitList(List<Ambassador__c> locations, int nSize = 200)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
        public QueryResult CurrentSalesForceQualifiedAmbassadorslist()
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();

            try
            {
                string strQuery = (@"select id,AmbassadorID__c from Ambassador__c where id !='' order by AmbassadorID__c ");
                qr = SfdcBinding.query(strQuery);
                return qr;
            }
            catch (Exception)
            {
                return qr;
            }

        }
        public string GetSalesForceQualifiedAmbassadorsID(string Sponsorid = "")
        {
            QueryResult qr = new QueryResult();
            string Ambassador= string.Empty;
            SforceService SfdcBinding = SalesForceSession();

            try
            {
                string strQuery = (@"select id from Ambassador__c where AmbassadorID__c ='" + Sponsorid + "'");
                qr = SfdcBinding.query(strQuery);
                Common.SforceServices.sObject[] records = qr.records;
                if (records != null)
                {
                    foreach (Ambassador__c node in records)                    
                    {
                        Ambassador = node.Id;
                       break;
                    }
                }
                return Ambassador;
            }
            catch (Exception)
            {
                return Ambassador;
            }

        }
        public void UpateSalesForceQualifiedAmbassador(Ambassador__c QualifiedAmbassador)
        {
            SforceService SfdcBinding = SalesForceSession();

            Ambassador__c sfdcQualifiedAmbassador = new Ambassador__c();


            if (QualifiedAmbassador.Available_To_Receive_Leads__cSpecified)
            {
                sfdcQualifiedAmbassador = QualifiedAmbassador;
                sfdcQualifiedAmbassador.Available_To_Receive_Leads__c = true;
                sfdcQualifiedAmbassador.Available_To_Receive_Leads__cSpecified = true;


            }
            else {
                sfdcQualifiedAmbassador.Id = QualifiedAmbassador.Id;
                sfdcQualifiedAmbassador.Name = QualifiedAmbassador.Name;
                sfdcQualifiedAmbassador.Zip_Code__c = QualifiedAmbassador.Zip_Code__c;
                sfdcQualifiedAmbassador.Q_Ambassador_Email__c = QualifiedAmbassador.Q_Ambassador_Email__c;
                sfdcQualifiedAmbassador.Available_To_Receive_Leads__c = false;
                sfdcQualifiedAmbassador.Available_To_Receive_Leads__cSpecified = true;
            }
            try
            {
                //Campaign compaign = new Campaign();
                //compaign.Name="IndiaHicks";
                //sfdcLead.c = compaign;

                SaveResult[] saveResults = SfdcBinding.update(new sObject[] { sfdcQualifiedAmbassador });
                if (saveResults[0].success)
                {
                    string Id = "";
                    Id = saveResults[0].id;



                }
                else
                {
                    string result = "";
                    result = saveResults[0].errors[0].message;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void CreateSalesForceQualifiedAmbassador(Ambassador__c QualifiedAmbassador)
        {
            SforceService SfdcBinding = SalesForceSession();
            try
            {
                //Campaign compaign = new Campaign();
                //compaign.Name="IndiaHicks";
                //sfdcLead.c = compaign;

                SaveResult[] saveResults = SfdcBinding.create(new sObject[] { QualifiedAmbassador });
                if (saveResults[0].success)
                {
                    string Id = "";
                    Id = saveResults[0].id;



                }
                else
                {
                    string result = "";
                    result = saveResults[0].errors[0].message;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public QueryResult GetSalesForceQualifiedAmbassadorDetail(string Id)
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();

            try
            {
                string strQuery = (@"select id, Name, AmbassadorID__c, Mobile_Phone__c, Phone__c, SponsorID__c, Q_Ambassador_Email__c, Zip_Code__c, Available_To_Receive_Leads__c, CreatedById from Ambassador__c where Id='" + Id + "'");
                qr = SfdcBinding.query(strQuery);
                return qr;
            }
            catch (Exception)
            {
                return qr;
            }

        }
        public void UpateSalesForceLead(Lead lead)
        {
            SforceService SfdcBinding = SalesForceSession();
            Lead sfdcLead = new Lead();
            sfdcLead = lead;
            try
            {
                //Campaign compaign = new Campaign();
                //compaign.Name="IndiaHicks";
                //sfdcLead.c = compaign;

                SaveResult[] saveResults = SfdcBinding.update(new sObject[] { sfdcLead });
                if (saveResults[0].success)
                {
                    string Id = "";
                    Id = saveResults[0].id;
                }
                else
                {
                    string result = "";
                    result = saveResults[0].errors[0].message;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteSalesForceLead(string leadID)
        {
            SforceService SfdcBinding = SalesForceSession();
            List<string> ids = new List<string>();
            ids.Add(leadID);
            DeleteResult[] deleteResults = SfdcBinding.delete(ids.ToArray());

            
                if (deleteResults[0].success)
                {
                    string Id = "";
                    Id = deleteResults[0].id;
                }
                else
                {
                    string result = "";
                    result = deleteResults[0].errors[0].message;
                }

           

        }

        private static SforceService SalesForceSession()
        {
            string userName;
            string password;
            string SecurityToken;
            userName = ConfigurationManager.AppSettings["SalesForceEmail"];
            password = ConfigurationManager.AppSettings["SalesForcePassword"];
            SecurityToken = ConfigurationManager.AppSettings["SalesForceSecurityToken"];
            //password =  password+securityToken
            password = password + SecurityToken;//"786allah8SEZFhlgUY05CgtgmylgMR3m";
            SforceService SfdcBinding = null;
            LoginResult CurrentLoginResult = null;
            SfdcBinding = new SforceService();
            try
            {
                CurrentLoginResult = SfdcBinding.login(userName, password);
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                // This is likley to be caused by bad username or password
                SfdcBinding = null;
                throw (e);
            }

            catch (Exception e)
            {
                // This is something else, probably comminication
                SfdcBinding = null;
                throw (e);
            }
            //Change the binding to the new endpoint
            SfdcBinding.Url = CurrentLoginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            SfdcBinding.SessionHeaderValue = new SessionHeader();
            SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
            return SfdcBinding;
        }
        public QueryResult GetSalesForceLead(String ID, string whereClause = "")
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();
            
            try
            {
                whereClause = whereClause.Replace("CreatedDate","Created_Date_DB__c");

                string strQuery = ("SELECT Id, FirstName,Account_Notes__c,LastName,Created_Date_DB__c,Status,Email,Phone,Address,LeadSource from Lead where Ambassador__c ='" + ID + "' " + whereClause);
                qr = SfdcBinding.query(strQuery);

                return qr;


            }
            catch (Exception)
            {
                return qr;
            }

        }
        public QueryResult GetSalesForceLeadByEmail(String email,string phone)
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();

            try
            {
                string strQuery = ("SELECT Id, FirstName,LastName,Email,Phone,Address,Street,City,State,Country,PostalCode from Lead where Email = '" + email + "' and Phone='"+phone+"'");
                // string strQuery = ("SELECT Id, FirstName,Account_Notes__c,LastName,Created_Date_DB__c,Status,Email,Phone,Address,LeadSource from Lead where Sponsor__c ='" + FirstLastID + "' " + whereClause);
                qr = SfdcBinding.query(strQuery);

                return qr;


            }
            catch (Exception)
            {
                return qr;
            }

        }
        public  int SalesForceLeadsCount(String FirstLastName )
        {
            try
            {

                QueryResult qr = new QueryResult();
                
                int Leadcount = 0;
                DataTable dt = new DataTable();
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("SalesForceBannerLeadFlag", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@Name", FirstLastName));
                    PubsConn.Open();
                    SqlDataReader myReader = testCMD.ExecuteReader();
                    dt.Load(myReader);
                    PubsConn.Close();
                }
                if (dt.Rows.Count == 0)
                {
                    SforceService SfdcBinding = SalesForceSession();
                    int SalesForcecount = 0;
                    string strQuery = ("SELECT Id from Lead where Sponsor__c ='" + FirstLastName + "'");
                    qr = SfdcBinding.query(strQuery);
                    SalesForcecount = qr.records.Count();
                    if (SalesForcecount > 0)
                    {
                        using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                        {
                            SqlCommand testCMD = new SqlCommand("InsertSalesForceLeadCount", PubsConn);
                            testCMD.CommandType = CommandType.StoredProcedure;
                            testCMD.Parameters.Add(new SqlParameter("@Name", FirstLastName));
                            testCMD.Parameters.Add(new SqlParameter("@Count", SalesForcecount));
                            PubsConn.Open();

                            SqlDataReader myReader = testCMD.ExecuteReader();
                            dt = new DataTable();
                            dt.Load(myReader);

                            PubsConn.Close();
                            foreach (DataRow row in dt.Rows)
                            {
                                Leadcount = int.Parse(row["Leads"].ToString());
                            }
                            return Leadcount;
                        }
                    }
                    return SalesForcecount;
                }
                foreach (DataRow row in dt.Rows)
                {
                    Leadcount = int.Parse(row["CountDiffernece"].ToString());
                }
                return Leadcount;
            }
            catch (Exception)
            {
                return 0;
            }

        
        }
        public QueryResult GetLeadDetail(string Id)
        {
            QueryResult qr = new QueryResult();
            SforceService SfdcBinding = SalesForceSession();
            
          
            try
            {
                string strQuery = ("SELECT Id,Created_Date_DB__c,FirstName,Account_Notes__c,LastName,Status,Email,MobilePhone,Phone,Address,LeadSource from Lead  where Id='" + Id + "'");
                qr = SfdcBinding.query(strQuery);

                return qr;


            }
            catch (Exception)
            {
                return qr;
            }

        }

        public void CreateLeadeTask(Task task) {
            SforceService SfdcBinding = SalesForceSession();
            SaveResult[] results = null;
            try
            {
                results = SfdcBinding.create(new Task[] {
                task
            });
            }
            catch (Exception ce)
            {
            }
        
        }
        public void UpdateLeadeTask(Task task)
        {
            SforceService SfdcBinding = SalesForceSession();
            SaveResult[] results = null;
            try
            {
                results = SfdcBinding.update(new Task[] {
                task
            });
            }
            catch (Exception ce)
            {
            }

        }
        public void CreateLeadEvent(Common.SforceServices.Event evnt)
        {
            SforceService SfdcBinding = SalesForceSession();
            SaveResult[] results = null;
            try
            {
                results = SfdcBinding.create(new Event[] {
                    evnt
                });
            }
            catch (Exception ce)
            {
            }
        
        }
        public void UpdateLeadEvent(Common.SforceServices.Event evnt)
        {
            SforceService SfdcBinding = SalesForceSession();
            SaveResult[] results = null;
            try
            {
                results = SfdcBinding.update(new Event[] {
                    evnt
                });
            }
            catch (Exception ce)
            {
            }

        }

        //public void PopulateCustomeObject()
        //{ 
        //         var customers = ExigoService.Exigo.OData().Customers.Where(i=>i.RankID>=15 && i.CustomerTypeID.Equals((int)CustomerTypes.IndependentStyleAmbassador) && i.CanLogin==true).ToList();
        //                      string userName;
        //          string password;
        //          string SecurityToken;
        //          userName = ConfigurationManager.AppSettings["SalesForceEmail"];
        //          password = ConfigurationManager.AppSettings["SalesForcePassword"];
        //          SecurityToken = ConfigurationManager.AppSettings["SalesForceSecurityToken"];
        //          //password =  password+securityToken
        //          password = password + SecurityToken;
        //          SforceService SfdcBinding = null;
        //          LoginResult CurrentLoginResult = null;
        //          SfdcBinding = new SforceService();
        //          try
        //          {
        //              CurrentLoginResult = SfdcBinding.login(userName, password);
        //          }
        //          catch (System.Web.Services.Protocols.SoapException e)
        //          {
        //              // This is likley to be caused by bad username or password
        //              SfdcBinding = null;
        //              throw (e);
        //          }
        //          catch (Exception e)
        //          {
        //              // This is something else, probably comminication
        //              SfdcBinding = null;
        //              throw (e);
        //          }
        //          //Change the binding to the new endpoint
        //          SfdcBinding.Url = CurrentLoginResult.serverUrl;
  
        //          //Create a new session header object and set the session id to that returned by the login
        //          SfdcBinding.SessionHeaderValue = new SessionHeader();
        //          SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
        //          List<Ambassador__c> lstambssador = new List<Ambassador__c>();
        //           foreach (var item in customers)
        //           {
        //                                     lstambssador.Add(new Ambassador__c 
        //              {
        //                  FirstName__c=item.FirstName,
        //                  LastName__c =item.LastName,
        //                  ZIP__c = item.MainZip,
        //                  RankID__c=(int)item.RankID,
        //                  RankID__cSpecified=true,
        //                  Email__c=item.Email,
        //                  CreatedDate__c=item.CreatedDate,
        //                  CreatedDate__cSpecified=true
        //              });
        //          }
        //          try
        //          {
        //              SaveResult[] saveResults = SfdcBinding.create(lstambssador.ToArray());
        //              if (saveResults[0].success)
        //              {
        //                  string Id = "";
        //                  Id = saveResults[0].id;
        //              }
        //              else
        //              {
        //                  string result = "";
        //                  result = saveResults[0].errors[0].message;
        //              }
  
        //          }
        //          catch (Exception)
        //          {
  
        //              throw;
        //           }

        
        //}


        public void PopulateLeadPickList()
        {
            //var customers = ExigoService.Exigo.OData().Customers.Where(i => i.RankID >= 15 && i.CustomerTypeID.Equals((int)CustomerTypes.IndependentStyleAmbassador) && i.CanLogin == true).ToList();
            var customers = ExigoService.Exigo.GetCustomer().Where(i => i.HighestAchievedRankId >= 15 && i.CustomerTypeID.Equals((int)CustomerTypes.IndependentStyleAmbassador) && i.CanLogin == true).ToList();
            Common.SFMDS.MetadataService service = createService();
            Common.SFMDS.CustomField customField = new Common.SFMDS.CustomField();
                  customField.fullName = "lead.Sponsor__c";
                  customField.label = "SA";
                  customField.type = Common.SFMDS.FieldType.Picklist;
                  customField.typeSpecified = true;
                  Common.SFMDS.Picklist pt = new Common.SFMDS.Picklist();
                  pt.sorted = false;
                  List<Common.SFMDS.PicklistValue> lstPickList = new List<Common.SFMDS.PicklistValue>();
                               foreach (var item in customers)
                   {
                       Common.SFMDS.PicklistValue node = new Common.SFMDS.PicklistValue();
                   //  node.fullName = string.Format("{0} {1}", item.FirstName, item.LastName);
                     node.fullName = item.FirstName+" "+item.LastName+" ("+item.CustomerID+")";
                    node.@default = false;
                    lstPickList.Add(node);
                    }
                 lstPickList = lstPickList.GroupBy(i => i.fullName).Select(i => i.First()).OrderBy(i=>i.fullName).ToList();
                  pt.picklistValues = lstPickList.ToArray();
                  customField.picklist = pt;
                  var results = service.updateMetadata(new Common.SFMDS.Metadata[] { customField });
              
                   



        }

        public PicklistEntry[] GetLeadSourcePicklist()
        {
            SforceService SfdcBinding = SalesForceSession();
            PicklistEntry[] picklistValues = null;
            DescribeSObjectResult[] describeSObjectResults = SfdcBinding.describeSObjects(new string[] {"lead"});
            Field[] fields = describeSObjectResults[0].fields;
                foreach (Field field in fields)
                {
                    if (field.type == fieldType.picklist && field.name == "Status")
                    {
                       picklistValues = field.picklistValues;
                       break;
                    }
                }
          
            return picklistValues;
        }


                  public  Common.SFMDS.MetadataService createService()
          {
              string userName;
              string password;
              string SecurityToken;
              userName = ConfigurationManager.AppSettings["SalesForceEmail"];
              password = ConfigurationManager.AppSettings["SalesForcePassword"];
              SecurityToken = ConfigurationManager.AppSettings["SalesForceSecurityToken"];
              password = password + SecurityToken;//"786allah8SEZFhlgUY05CgtgmylgMR3m";
              SforceService SfdcBinding = null;
              LoginResult CurrentLoginResult = null;
              SfdcBinding = new SforceService();
              try
              {
                  CurrentLoginResult = SfdcBinding.login(userName, password);
              }
              catch (System.Web.Services.Protocols.SoapException e)
              {
                  // This is likley to be caused by bad username or password
                  SfdcBinding = null;
                  throw (e);
              }
              catch (Exception e)
              {
                  // This is something else, probably comminication
                  SfdcBinding = null;
                  throw (e);
              }
              Common.SFMDS.MetadataService service = new Common.SFMDS.MetadataService();

              service.SessionHeaderValue = new Common.SFMDS.SessionHeader();
              service.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
              return service;
           }

    }
}
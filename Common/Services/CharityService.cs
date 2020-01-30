using ExigoService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Charity = Common.Api.ExigoOData.Rewards.Charity;
namespace Common.Services
{

    public class CharityService
    {
        public List<CharityModel> GetCharityList(string Search)
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("SelectCharities", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@param", Search));                 
                    PubsConn.Open();

                    SqlDataReader myReader = testCMD.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myReader);
                    
                    PubsConn.Close();
                    CharityList = (from rw in dt.AsEnumerable()
                                   select new CharityModel()
                                     {
                                         CharityID = Convert.ToString(rw["EIN"]),
                                         CharityName = Convert.ToString(rw["CharityName"])
                                     }).ToList();
                }
            }
            catch (Exception)
            {
                
            }
            return CharityList;


        }

        public List<AddCharityModel> GetCharityListForEdit(string Search)
        {

            List<AddCharityModel> CharityList = new List<AddCharityModel>();
            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("SelectCharitiesForEdit", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@param", Search));
                    PubsConn.Open();

                    SqlDataReader myReader = testCMD.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    PubsConn.Close();
                    CharityList = (from rw in dt.AsEnumerable()
                                   select new AddCharityModel()
                                   {
                                       EIN = Convert.ToString(rw["EIN"]),
                                       Name = Convert.ToString(rw["Name"]),
                                       City = Convert.ToString(rw["City"]),
                                       State = Convert.ToString(rw["State"]),
                                       Country = Convert.ToString(rw["Country"]),
                                       Deductibility = Convert.ToString(rw["Deductibility"])
                                   }).ToList();
                }
            }
            catch (Exception)
            {

            }
            return CharityList;


        }
        public void InsertCharityEvent(int EventID)
        {
        
            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("InsertCharityEvent", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@EventID", EventID));
                    PubsConn.Open();
                     var dd= testCMD.ExecuteScalar();
                    PubsConn.Close();
                }
            }
            catch (Exception)
            {
            }
            
        }
        public bool CheckCharityEvent(int EventID)
        {
            bool  charityEvent = false;

            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("CheckCharitableEvent", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@param", EventID));
                    PubsConn.Open();

                    SqlDataReader myReader = testCMD.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    PubsConn.Close();

                    if (dt.Rows.Count > 0) charityEvent = true;
                }
            }
            catch (Exception)
            {
               
            }
            return charityEvent;
        }



        public List<CharityModel> GetCharityName(int eventId)
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                using (var context = Exigo.Sql())
                {

                    var SqlProcedure = string.Format("GetCharityName {0}", eventId);

                    CharityList = context.Query<CharityModel>(SqlProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return CharityList;

            }
            return CharityList;


        }
        public List<CharityModel> GetCharities()
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                string alpha = "ABCDEFGHIJKLMNOPQRSTUVQXYZ";
                foreach (char c in alpha)
                {
                    CharityModel node = new CharityModel();
                    node.CharityID = c.ToString();
                    node.CharityName = c.ToString();
                    CharityList.Add(node);
                }
            }
            catch (Exception ex)
            {

            }
            return CharityList;


        }
        public List<CharityModel> CheckCharitableEvent(int eventId)
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                using (var context = Exigo.Sql())
                {

                    var sqlProcedure = string.Format("CheckCharitableEvent {0}", eventId);

                    CharityList = context.Query<CharityModel>(sqlProcedure).ToList();
                }
            }
            catch (Exception)
            {

            }
            return CharityList;


        }
        public List<CharityModel> GetCharityNameEIN(string  charityID)
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("SelectCharityByEIN", PubsConn);
                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@param", charityID));
                    PubsConn.Open();

                    SqlDataReader myReader = testCMD.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myReader);

                    PubsConn.Close();
                    CharityList = (from rw in dt.AsEnumerable()
                                   select new CharityModel()
                                   {
                                       CharityID = Convert.ToString(rw["EIN"]),
                                       CharityName = Convert.ToString(rw["CharityName"])
                                   }).ToList();
                }
            }
            catch (Exception)
            {

            }
            return CharityList;

        }
        public int GetCustomerExtendedDetailID(int eventId)
        {

            List<CharityModel> CharityList = new List<CharityModel>();
            try
            {
                using (var context = Exigo.Sql())
                {
                    // here we replace EIN with CharityID to use same model to every where

                    var SqlProcedure = string.Format("GetCharityExtendedDetailID {0}", eventId);

                    CharityList = context.Query<CharityModel>(SqlProcedure).ToList();
                }
            }
            catch (Exception)
            {

            }
            return int.Parse(CharityList.FirstOrDefault().CharityID);

        }
        public IhCuurentCharityContribution GetIHCharityContribution()
        {

            IhCuurentCharityContribution CharityContributionList = new IhCuurentCharityContribution();
            try
            {
                using (var context = Exigo.Sql())
                {
                    // here we get current Charity contribution from Indiahicks

                    var SqlProcedure = string.Format("GetIHCharityContribution");

                    CharityContributionList = context.Query<IhCuurentCharityContribution>(SqlProcedure).FirstOrDefault();
                    if (CharityContributionList == null)
                    {
                        CharityContributionList = new IhCuurentCharityContribution();
                        CharityContributionList.CharityContribution = 5; }
                        
                }
            }
            catch (Exception)
            {

            }
            return CharityContributionList;

        }
        public String AddIHCharity(AddCharityModel charity) {
            string charityEvent = "";

            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("InsertCharity", PubsConn);

                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@EIN", charity.EIN));
                    testCMD.Parameters.Add(new SqlParameter("@Name", charity.Name));
                    testCMD.Parameters.Add(new SqlParameter("@City", charity.City));
                    testCMD.Parameters.Add(new SqlParameter("@State", charity.State));
                    testCMD.Parameters.Add(new SqlParameter("@Country", charity.Country));
                    testCMD.Parameters.Add(new SqlParameter("@Deductibility", charity.Deductibility));
                    PubsConn.Open();
                    var result= testCMD.ExecuteNonQuery();


                    if (result.ToString()=="1") charityEvent = "SuccessFully Save";
                }
            }
            catch (Exception ex)
            {
                charityEvent = "Please Try Again";
            }
            return charityEvent;
        
        }
        public String EditIHCharity(AddCharityModel charity)
        {
            string charityEvent = "";

            try
            {
                using (var PubsConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IHCharity"].ConnectionString.ToString()))
                {
                    SqlCommand testCMD = new SqlCommand("EditCharity", PubsConn);

                    testCMD.CommandType = CommandType.StoredProcedure;
                    testCMD.Parameters.Add(new SqlParameter("@EIN", charity.EIN));
                    testCMD.Parameters.Add(new SqlParameter("@Name", charity.Name));
                    testCMD.Parameters.Add(new SqlParameter("@City", charity.City));
                    testCMD.Parameters.Add(new SqlParameter("@State", charity.State));
                    testCMD.Parameters.Add(new SqlParameter("@Country", charity.Country));
                    testCMD.Parameters.Add(new SqlParameter("@Deductibility", charity.Deductibility));
                    testCMD.Parameters.Add(new SqlParameter("@EditEin", charity.EditEin));
                    testCMD.Parameters.Add(new SqlParameter("@EditName", charity.EditName));
                    PubsConn.Open();
                    var result = testCMD.ExecuteNonQuery();


                    if (result.ToString() == "1") charityEvent = "SuccessFully Update";
                }
            }
            catch (Exception ex)
            {
                charityEvent = "Please Try Again";
            }
            return charityEvent;

        }
        public void AddCharity(string FilePath)
        {


            using (StreamReader file = new StreamReader(FilePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] fields = line.Split('|');

                    if (fields[4].ToString() == "United States" && fields.Length > 0)
                    {
                        Charity Charity = new Charity();
                        Charity.EIN = fields[0].ToString();
                        Charity.Name = fields[1].ToString();
                        Charity.City = fields[2].ToString();
                        Charity.State = fields[3].ToString();
                        Charity.Country = fields[4].ToString();
                        Charity.Deductibility = fields[5].ToString();
                        Exigo.AddToCharity(Charity);
                    }

                }
            }
        }
    }
    public class CharityModel
    {
        public string CharityID { get; set; }
        public string CharityStatus { get; set; } 
        public string CharityName { get; set; }
        public string CharityAddress { get; set; }
        public string CharityAddress2 { get; set; }
        public string CharityCity { get; set; }
        public string CharityState { get; set; }
        public string CharityZip { get; set; }
        public string CharityCountry { get; set; }
        public string CharityContribution { get; set; }
        public string IHContribution { get; set; }
        public string CurrentIHCharityContribution { get; set; }
    }
    public class IhCuurentCharityContribution
    {
        public decimal  CharityContribution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }  
    }
    public class AddCharityModel
    {
        [Required]            
        public string EIN{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Deductibility { get; set; }
        public string AddEdit { get; set; }
        public string EditEin { get; set; }
        public string EditName { get; set; }
    }

}
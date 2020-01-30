using ExigoService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Common.Services
{
    public class CvsDateTable
    {
        public static DataTable GenerateDateTable(string filePath)
        {

            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;

        }
    }

    public class CustomerTypeDescriptionID
    {
        public int CustomerTypeID { get; set; }
        public string CustomerTypeDescription { get; set; }
        public static List<CustomerTypeDescriptionID> GetCustomerTypeDescriptionID()
        {

            List<CustomerTypeDescriptionID> list = new List<CustomerTypeDescriptionID>();

            try
            {
                using (var context = Exigo.Sql())
                {

                    var SqlProcedure = string.Format("GetCustomerTypes");

                    list = context.Query<CustomerTypeDescriptionID>(SqlProcedure).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                return list;
            }

        }
    }


}
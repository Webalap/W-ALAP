using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TimeZone = Common.Models.ExigoService.CountryRegions.TimeZone;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<Country> GetCountries()
        {
            List<Country> records = new List<Country>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("Exec GetCountry");
                records = context.Query<Country>(sqlProcedure).ToList();
            }
            return records;
        }
        public static IEnumerable<Region> GetRegions(string CountryCode)
        {
            //var context = Exigo.OData();
            var regions = new List<Region>();

            List<CountryRegionsModel> results = null;
            using (var context = Sql())
            {
                var sqlProcedure = string.Format("GetCountryRegions '{0}'", CountryCode);
                results = context.Query<CountryRegionsModel>(sqlProcedure).Where(c => c.CountryCode == CountryCode).ToList();
            }
            regions = results.Select(c => new Region()
            {
                RegionCode = c.RegionCode,
                RegionName = c.RegionDescription
            }).ToList();

            return regions;
        }
        
        public static CountryRegionCollection GetCountryRegions(string CountryCode="")
        {
            var result = new CountryRegionCollection();
            //calling procedure
            try
            {
                using (var context = Sql())
                {
                    var sqlProcedure = string.Format("GetCountryRegions '{0}'", CountryCode);
                    var lstRegions = context.Query<CountryRegionsModel>(sqlProcedure).ToList();
                    result.Countries = GetCountries().Select(c => new Country()
                    {
                        CountryCode = c.CountryCode,
                        CountryName = c.CountryName
                    });

                    result.Regions = lstRegions.Select(c => new Region()
                    {
                        RegionCode = c.RegionCode,
                        RegionName = c.RegionDescription
                    });
                }
            }
            catch (Exception ex) {  }

            return result;
        }
        public static IEnumerable<TimeZone> GetTimeZonesByCountryCode(string countryCode)
        {
            List<TimeZone> TimeZones = new List<TimeZone>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("Exec GetTimeZonesByCountryCode {0}", countryCode);
                TimeZones = context.Query<TimeZone>(sqlProcedure).ToList();
            }
            return TimeZones;
        }
    }
}
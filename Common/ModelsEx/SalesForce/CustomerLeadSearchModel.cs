namespace ExigoService
{

    public class CustomerLeadSearchModel
    {
        public string LeadID { get; set; }
        public string utm_Fields { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string MobilePhone { get; set; }
        public string WebAlias { get; set; }
    }
    public class Salesforce_UtmFields
    {
        public string utm_medium { get; set; }
        public string utm_source { get; set; }
        public string utm_campaign { get; set; }
        public string utm_adgroup { get; set; }
        public string utm_content { get; set; }
        public string utm_adid { get; set; }
        public string utm_placement { get; set; }
        public string utm_adtest { get; set; }
        public string utm_SFID { get; set; }
        public string ConversionGoogleClientId { get; set; }
        public string value_campaign { get; set; }
        public string value_creative { get; set; }
        public string value_keyword { get; set; }

    }
}

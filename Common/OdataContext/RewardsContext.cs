using System;

namespace Common.Api.ExigoOData.Rewards
{

    public partial class AnnouncementBanner
    {

        public int AnnouncementBannerID { get; set; }
        public string Image { get; set; }
        public Nullable<int> SiteType { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }

        public string Text3 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
    public partial class CCContact
    {

        public int ID { get; set; }
        public int EnrollerID { get; set; }
        public string ContactList { get; set; }
        public string LastPageLink { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.Charity in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CharityID
    /// </KeyProperties>
    public partial class Charity
    {

        public int CharityID { get; set; }
        public string EIN { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Deductibility { get; set; }
    }
    public partial class DisplayCategory
    {
        public int DisplayCategoryID { get; set; }
        public int ID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string SiteType { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SortOrder { get; set; }
        public Nullable<bool> ShowOnCorporateSite { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.EBReward in the schema.
    /// </summary>
    /// <KeyProperties>
    /// EBRewardID
    /// </KeyProperties>
    public partial class EBReward
    {
        public Guid EBRewardID { get; set; }
        public int Phase { get; set; }
        public string ItemCode { get; set; }
        public decimal SalesThreshhold { get; set; }
        public decimal PercentDiscount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.EmailCampaign in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    public partial class EmailCampaign
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ReplyToEmail { get; set; }
        public string TemplateTypeString { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> LastRunDate { get; set; }
        public Nullable<DateTime> NextRunDateDate { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsViewAsWebPageEnabled { get; set; }
        public string PermissionReminderText { get; set; }
        public string ViewAsWebPageText { get; set; }
        public string ViewAsWebPageLinkText { get; set; }
        public string GreetingSalutations { get; set; }
        public string GreetingNameString { get; set; }
        public string PermanentLink { get; set; }
        public string GreetingString { get; set; }
        public string EmailContent { get; set; }
        public string EmailContentFormatString { get; set; }
        public string TextContent { get; set; }
        public string StyleSheet { get; set; }
        public string TrackingSummaryID { get; set; }
        public string MessageFooter { get; set; }
        public string IsVisibleInUI { get; set; }
        public string SentContactList { get; set; }
        public string ClickThroughDetails { get; set; }
        public string ClickActivity { get; set; }
        public string ForwardActivity { get; set; }
        public string OpenActivity { get; set; }
        public string SendActivity { get; set; }
        public string OptOutActivity { get; set; }
        public string BounceActivity { get; set; }
        public string ClickActivityLink { get; set; }
        public string ForwardActivityLink { get; set; }
        public string OpenActivityLink { get; set; }
        public string SendActivityLink { get; set; }
        public string OptOutActivityLink { get; set; }
        public string BounceActivityLink { get; set; }
        public string CampignLink { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.EmailDetails in the schema.
    /// </summary>
    /// <KeyProperties>
    /// EmailDetailID
    /// </KeyProperties>
    public partial class EmailDetails
    {

        public int EmailDetailID { get; set; }
        public string EmailTemplateName { get; set; }
        public int EmailThresholdDays { get; set; }
        public string SQLQuery { get; set; }
        public string EmailSubject { get; set; }
    }

    public partial class FiredEvents
    {
        public int ID { get; set; }
        public int ScheduleID { get; set; }
        public int SnifferID { get; set; }
        public DateTime FiredTime { get; set; }
        public int CustomerID { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string EmailAddress { get; set; }
    }

    public partial class GiftPromo
    {
        public int GiftPromoID { get; set; }
        public string ItemCode { get; set; }
        public decimal DiscountPrecent { get; set; }
        public decimal SalesThreshold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int DiscountType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.HostSpecial in the schema.
    /// </summary>
    /// <KeyProperties>
    /// HostSpecialID
    /// </KeyProperties>
    public partial class HostSpecial
    {
        public Guid HostSpecialID { get; set; }
        public string ItemCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal SalesThreshold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.IHCharityContribution in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    public partial class IHCharityContribution
    {

        public int ID { get; set; }
        public decimal CharityContribution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.JoinUs in the schema.
    /// </summary>
    /// <KeyProperties>
    /// JoinId
    /// </KeyProperties>
    public partial class JoinUs
    {

        public int JoinId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string BecomingAnAmbassador { get; set; }
        public string GettingMoreInformation { get; set; }
        public string HostingaGetToGether { get; set; }
        public Nullable<int> AmbassadorId { get; set; }
        public string Notes { get; set; }
        public string UTMMedium { get; set; }
        public string UTMSource { get; set; }
        public string UTMCampaign { get; set; }
        public string UTMContent { get; set; }
        public string UTMTerm { get; set; }
        public Nullable<DateTime> DateCreated { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.LimitedEditionProducts in the schema.
    /// </summary>
    /// <KeyProperties>
    /// LimitedEditionProductsID
    /// </KeyProperties>
    public partial class LimitedEditionProducts
    {
        public int LimitedEditionProductsID { get; set; }
        public string ItemCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.MobileShoppingCart in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    public partial class MobileShoppingCart
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string ItemCode { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.NewProductsLaunchReward in the schema.
    /// </summary>
    /// <KeyProperties>
    /// NewProductsLaunchRewardID
    /// </KeyProperties>
    public partial class NewProductsLaunchReward
    {
        public int NewProductsLaunchRewardID { get; set; }
        public int Credits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ThresholdPeriodStart { get; set; }
        public DateTime ThresholdPeriodEnd { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PRVThresholdMin { get; set; }
        public decimal PRVThresholdMax { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.PointAccountExpiration in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PointAccountExpirationID
    /// </KeyProperties>
    public partial class PointAccountExpiration
    {
        public int PointAccountExpirationID { get; set; }
        public decimal ThresholdDays { get; set; }
        public int PointAccountID { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.PromoCode in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PromoCodeID
    /// </KeyProperties>
    public partial class PromoCode
    {
        public int PromoCodeID { get; set; }
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Email { get; set; }
        public bool IsCoupon { get; set; }
        public int CustomerID { get; set; }
        public int? UsedCount { get; set; }
        public int? EligibleCount { get; set; }
        public string ApplyToItemCodes { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.PromotionGift in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Promotion_ID
    /// </KeyProperties>
    public partial class PromotionGift
    {
        public int Promotion_ID { get; set; }
        public string Item_Code { get; set; }
        public string Description { get; set; }
        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }
        public int? DiscountedValue { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.RetailPromo in the schema.
    /// </summary>
    /// <KeyProperties>
    /// RetailPromoRewardID
    /// </KeyProperties>
    public partial class RetailPromo
    {
        public string RetailPromoRewardID { get; set; }
        public int? DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? WebCategoryID { get; set; }
        public string ItemCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BV { get; set; }
        public decimal? CV { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.SavedCart in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    public partial class SavedCart
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string PropertyBag { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.ScheduleDetails in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ScheduleID
    /// </KeyProperties>
    public partial class ScheduleDetails
    {
        public int ScheduleID { get; set; }
        public string JobName { get; set; }
        public int SnifferID { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int? EmailAttachement { get; set; }
        public decimal PRVThreshold { get; set; }
        public int? FreshRecruits { get; set; }
        public bool? ThresholdCondition { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.SnifferDetails in the schema.
    /// </summary>
    /// <KeyProperties>
    /// SnifferID
    /// </KeyProperties>
    public partial class SnifferDetails
    {
        public int SnifferID { get; set; }
        public int TimeSpan { get; set; }
        public bool State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.StyleAmbassadorReward in the schema.
    /// </summary>
    /// <KeyProperties>
    /// StyleAmbassadorRewardID
    /// </KeyProperties>
    public partial class StyleAmbassadorReward
    {
        public Guid StyleAmbassadorRewardID { get; set; }
        public decimal PersonalRetailVolumeThreshold { get; set; }
        public int HalfOffCreditsJoining { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

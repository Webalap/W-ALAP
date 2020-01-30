using AutoMapper;
using Common.Api.ExigoWebService;
using Common.ModelsEx.Base;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx.Base;
using ExigoService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.ServicesEx
{
    public class EventService : ServiceBase, IEventService
    {
        #region Dependencies
        
        [Inject]
        public IRewardService RewardService { get; set; }
        
        #endregion

        #region Get Events

        Event[] IEventService.GetEvents(int customerId)
        {

            List<Event> events = new List<Event>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetEvents {0},{1},{2}", (int)CustomerExtendedGroup.PartyDetails, CustomerTypes.Party, customerId);
                 events = context.Query<Event>(sqlProcedure).ToList();
                // get new ly Created Events from database 
                int topEventID = events.Count()==0?0:events.OrderByDescending(i => i.Id).Take(1).FirstOrDefault().Id;
                string sqlProcedureNewGt = string.Format("GetNewEvents {0},{1}", customerId, topEventID);
                events.AddRange(context.Query<Event>(sqlProcedureNewGt).ToList());
            }
            return events.OrderByDescending(s => s.StartDate).ToArray();
        }

        #endregion

        #region Get Event Details
        public Event GetEventDetailsService(int eventId, bool ApiCall = false)
        {
            if (ApiCall) return ApiGetEventDetails(eventId);
            //getting event details for selected event
            Event EventDetails = new Event();
                using (var Context = Exigo.Sql())
                {
                    Context.Open();
                    string sqlProcedure = string.Format("GetEventDetails {0}", eventId);
                    EventDetails = Context.Query<Event>(sqlProcedure).FirstOrDefault();
                    Context.Close();

                }
            return EventDetails;
        }


        public Event GetEventDetails(int eventId)
        {
            
            Customer eventCustomer = Exigo.GetCustomerByIDandType(eventId, CustomerTypes.Party);

            //using (var context = Exigo.Sql())
            //{
            //    var sql = string.Format("exec GetCustomerByIDandType {0}, {1}", eventId, CustomerTypes.Party);
            //    eventCustomer = context.Query<Customer>(sql).FirstOrDefault();
            //}

            //if (null == eventCustomer)
            //{           
            //    throw new ApplicationException("The provided event ID does not exist.");
            //}

            var eventCustomerSiteResponse = Exigo.GetCustomerSite(eventId);

            var eventCustomerEx = Exigo.GetCustomerExtendedDetails(eventId, (int)CustomerExtendedGroup.PartyDetails).FirstOrDefault();

            var creator = new Customer(); 
            int creatorCustomerId = 0;
            if (int.TryParse(eventCustomerEx.Field5, out creatorCustomerId))
            {
                creator = Exigo.GetCustomerByIDandType(creatorCustomerId, CustomerTypes.IndependentStyleAmbassador);
                creator.WebAlias = Exigo.GetCustomerWebAlias(creator.CustomerID);
            }

            var host = new Customer();
            int hostCustomerId = 0;
            if (int.TryParse(eventCustomerEx.Field6, out hostCustomerId))
            {
                host = Exigo.GetCustomer(hostCustomerId);
            }

            // Retrieve the party sales total
            var salesTotal = GetPointAccountBalance(eventId, PointAccounts.PartySalesTotal);

            // Retieve the Host' rewards point accounts
            var pointAccounts = GetHostRewardPointAccounts(eventId);

            //var bookingRewards = GetBookingRewards();

            //int bookingRewardsOwnerId = 0;
            //int.TryParse(eventCustomerEx.Field7, out bookingRewardsOwnerId);

            // Create Missing Host Specials
            //CreateMissngHostSpecials(eventCustomerEx);


            var @event = new Event()
            {
                Id = eventCustomer.CustomerID,
                Name = eventCustomer.FirstName,
                Location = eventCustomer.MainAddress,
                Creator = creator,
                Host = host,
                TotalSales = salesTotal,
                EventPointAccounts = null != pointAccounts ? pointAccounts.ToArray() : null,
                //EventBookingRewards = bookingRewards,
                //BookingRewardsOwnerId = bookingRewardsOwnerId,
                WebAlias = eventCustomerSiteResponse.WebAlias,
                StartDate = Convert.ToDateTime(eventCustomerEx.Field1),
                ActualDate = Convert.ToDateTime(eventCustomerEx.Field2),
                EventStatus = eventCustomer.CustomerStatusID == 0 ? "Close" : Convert.ToDateTime(eventCustomerEx.Field4) < DateTime.Now ? "Close" : "Open",
                CloseDate = Convert.ToDateTime(eventCustomerEx.Field3),
                LockoutDate = Convert.ToDateTime(eventCustomerEx.Field4),
                DeletedEvents=eventCustomer.CustomerStatusID==0?true:false,
                //HostSpecialReward = (HostSpecialDiscount)eventCustomerEx,
            };

            if ((@event.LockoutDate - @event.CloseDate).TotalDays <= 2.00)
            {
                @event.LockoutDate = @event.CloseDate.AddDays(7);
            }
            return @event;
        }
        private Event ApiGetEventDetails(int eventId)
        {
            // TODO: Fix this hack.  Dependency injection is no longer working on AJAX calls?
            if (null == Api)
            {
                Api = ExigoService.Exigo.WebService();
            }

            var eventCustomerResponse = Api.GetCustomers(new GetCustomersRequest
            {
                CustomerID = eventId,
                CustomerTypes = new int[] { (int)CustomerTypes.Party }
            });

            Customer eventCustomer = null;
            if (null != eventCustomerResponse &&
                null != eventCustomerResponse.Customers &&
                0 != eventCustomerResponse.Customers.Length)
            {
                eventCustomer = Mapper.Map<Customer>(eventCustomerResponse.Customers[0]);
            }
            else
            {
                throw new ApplicationException("The provided event ID does not exist.");
            }

            var eventCustomerSiteResponse = Api.GetCustomerSite(new GetCustomerSiteRequest
            {
                CustomerID = eventId
            });

            if (ResultStatus.Failure.Equals(eventCustomerSiteResponse.Result.Status))
            {
                throw new ApplicationException("There an error while retrieving the details of the event ID provided.");
            }

            var eventCustomerExResponse = Api.GetCustomerExtended(new GetCustomerExtendedRequest
            {
                CustomerID = eventId,
                ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails, // Party Extended Fields
            });

            CustomerExtendedResponse eventCustomerEx = null;
            if (ResultStatus.Success.Equals(eventCustomerExResponse.Result.Status) &&
                null != eventCustomerExResponse.Items &&
                0 != eventCustomerExResponse.Items.Length)
            {
                eventCustomerEx = eventCustomerExResponse.Items[0];
            }
            else
            {
                throw new ApplicationException("There an error while retrieving the details of the event ID provided.");
            }

            GetCustomersResponse creatorCustomerResponse = null;
            int creatorCustomerId = 0;
            if (int.TryParse(eventCustomerEx.Field5, out creatorCustomerId))
            {
                creatorCustomerResponse = Api.GetCustomers(new GetCustomersRequest
                {
                    CustomerID = creatorCustomerId,
                    CustomerTypes = new int[] { (int)CustomerTypes.IndependentStyleAmbassador }
                });
            }

            Customer creator = null;
            if (ResultStatus.Success.Equals(creatorCustomerResponse.Result.Status) &&
                null != creatorCustomerResponse.Customers &&
                0 != creatorCustomerResponse.Customers.Length)
            {
                creator = Mapper.Map<Customer>(creatorCustomerResponse.Customers[0]);

                try
                {
                    var creatorCustomerSiteResponse = Api.GetCustomerSite(new GetCustomerSiteRequest
                    {
                        CustomerID = creator.CustomerID
                    });
                    creator.WebAlias = creatorCustomerSiteResponse.WebAlias;
                }
                catch { }
            }

            GetCustomersResponse hostCustomerResponse = null;
            int hostCustomerId = 0;
            if (int.TryParse(eventCustomerEx.Field6, out hostCustomerId))
            {
                hostCustomerResponse = Api.GetCustomers(new GetCustomersRequest
                {
                    CustomerID = hostCustomerId
                });
            }

            Customer host = null;
            if (ResultStatus.Success.Equals(hostCustomerResponse.Result.Status) &&
                null != hostCustomerResponse.Customers &&
                0 != hostCustomerResponse.Customers.Length)
            {
                host = Mapper.Map<Customer>(hostCustomerResponse.Customers[0]);
            }

            // Retrieve the party sales total
            var salesTotal = GetPointAccountBalance(eventId, PointAccounts.PartySalesTotal);

            // Retieve the Host' rewards point accounts
            var pointAccounts = GetHostRewardPointAccounts(eventId);

            //var bookingRewards = GetBookingRewards();

            //int bookingRewardsOwnerId = 0;
            //int.TryParse(eventCustomerEx.Field7, out bookingRewardsOwnerId);

            // Create Missing Host Specials
            CreateMissngHostSpecials(eventCustomerEx);


            var @event = new Event()
            {
                Id = eventCustomer.CustomerID,
                Name = eventCustomer.FirstName,
                Location = eventCustomer.MainAddress,
                MainAddress1 = eventCustomer.MainAddress.Address1,
                MainAddress2 = eventCustomer.MainAddress.Address2,
                MainCity = eventCustomer.MainAddress.City,
                MainState = eventCustomer.MainAddress.State,
                MainZip = eventCustomer.MainAddress.Zip,
                MainCountry = eventCustomer.MainAddress.Country,
                Creator = creator,
                Host = host,
                IsNewGT = true,
                HostFullName = host.FirstName + " " + host.LastName,
                CreatorFullName = creator.FirstName + " " + creator.LastName,
                HostId = host.CustomerID.ToString(),
                HostEmail = host.Email,
                HostPhone = host.Phone,
                CreatorId = creator.CustomerID.ToString(),
                TotalSales = salesTotal,
                EventPointAccounts = null != pointAccounts ? pointAccounts.ToArray() : null,
                //EventBookingRewards = bookingRewards,
                //BookingRewardsOwnerId = bookingRewardsOwnerId,
                WebAlias = eventCustomerSiteResponse.WebAlias,
                StartDate = Convert.ToDateTime(eventCustomerEx.Field1),
                ActualDate = Convert.ToDateTime(eventCustomerEx.Field2),
                EventStatus = eventCustomer.CustomerStatusID == 0 ? "Close" : Convert.ToDateTime(eventCustomerEx.Field4) < DateTime.Now ? "Close" : "Open",
                CloseDate = Convert.ToDateTime(eventCustomerEx.Field3),
                LockoutDate = Convert.ToDateTime(eventCustomerEx.Field4),
                PartyDate = string.IsNullOrEmpty(eventCustomerEx.Field12) ? Convert.ToDateTime(eventCustomerEx.Field2) : Convert.ToDateTime(eventCustomerEx.Field12),
                PartyStartTime = string.IsNullOrEmpty(eventCustomerEx.Field13) ? DateTime.Parse(eventCustomerEx.Field2) : DateTime.Parse(eventCustomerEx.Field13),
                PartyEndTime = string.IsNullOrEmpty(eventCustomerEx.Field14) ? DateTime.Parse(eventCustomerEx.Field2).AddHours(1) : DateTime.Parse(eventCustomerEx.Field14),
                TimeZone = string.IsNullOrEmpty(eventCustomerEx.Field15) ? "(UTC-06:00) Central Time (US & Canada)" : eventCustomerEx.Field15,
                DeletedEvents = eventCustomer.CustomerStatusID == 0 ? true : false,
                HostSpecialReward = (HostSpecialDiscount)eventCustomerEx,
            };

            if ((@event.LockoutDate - @event.CloseDate).TotalDays <= 2.00)
            {
                @event.LockoutDate = @event.CloseDate.AddDays(7);
            }
            return @event;
        }
        private void CreateMissngHostSpecials(CustomerExtendedResponse eventCustomerEx)
        {
            //Create Host Special if one does not exist in CustEx table
            var hostSpecial = RewardService.GetHostSpecialReward(Convert.ToDateTime(eventCustomerEx.Field2));
            if (!string.IsNullOrWhiteSpace(hostSpecial.ItemCode))
            {
                var details = RewardService.GetHostSpecialReward(eventCustomerEx.CustomerID);
                if (string.IsNullOrWhiteSpace(details.ItemCode))
                //Create the host special entry in CustomerEx
                {
                    var updateCustomerExtendedRequest = new UpdateCustomerExtendedRequest()
                    {
                        CustomerID = eventCustomerEx.CustomerID,
                        CustomerExtendedID = eventCustomerEx.CustomerExtendedID,
                        ExtendedGroupID = eventCustomerEx.ExtendedGroupID,
                        Field8 = hostSpecial.ItemCode,
                        Field9 = hostSpecial.DiscountAmount.ToString(CultureInfo.InvariantCulture),
                        Field10 = string.Empty, // Reward Not Redeemed
                        Field11 = hostSpecial.SalesThreshold.ToString(CultureInfo.InvariantCulture)
                    };
                    Api.UpdateCustomerExtended(updateCustomerExtendedRequest);
                }
            }
        }

        #endregion

        #region Get Point Account Methods

        public decimal GetPointAccountBalance(int customerId, int pointAccountId)
        {
            var response = Exigo.GetCustomerPointAccount(customerId, pointAccountId);
            
            return (null != response ?
                    response.Balance :
                    0M);
        }

        public RewardsAccount[] GetHostRewardPointAccounts(int customerId)
        {
            var pointAccounts = GetPointAccountsByTypes(customerId,
                new int[]
                {
                    PointAccounts.HostRewardsCash,
                    PointAccounts.Host12offcredits,
                    PointAccounts.BookingsRewardsCash,
                    PointAccounts.PartySalesTotal
//                    PointAccounts.RecruitingRewards
                });

            return (null != pointAccounts ?
                pointAccounts.ToArray() :
                null);
        }

        private List<RewardsAccount> GetPointAccountsByTypes(int customerId, int[] pointAccountIds)
        {
            return (from pointAccountId in pointAccountIds
                let balance = GetPointAccountBalance(customerId, pointAccountId)
                select new RewardsAccount
                {
                    CustomerID = customerId, Balance = balance, PointAccountID = pointAccountId
                }).ToList();
        }

        private static BookingReward CreateBookingReward(int customerId, int partyId, int bookingsRewardOwnerId)
        {
            var bookingReward = new BookingReward();
            if (partyId == 0) return bookingReward;
            //Get Party Details of Party that we earned booking rewards from.
            var party = Exigo.GetCustomer(partyId);
            var customerSite = Exigo.GetCustomerSite(partyId);
            //Get Point Account Value Earned for Booking Rewards - CustomerPointAccounts - PointAccountID 3
            var bookingsRewardsCash = Exigo.GetCustomerPointAccount(partyId, PointAccounts.BookingsRewardsCash);
            bookingReward.PartyName = party.FirstName;
            bookingReward.WebAlias = customerSite.WebAlias;
            bookingReward.BookingRewardOwner = bookingsRewardOwnerId;
            bookingReward.CustomerID = party.CustomerID; //This will be the Field 3, Field 4, Field 5, Field 6 from the ExtendedDetails
            bookingReward.Balance = (bookingsRewardsCash != null) ? bookingsRewardsCash.Balance : 0;
            bookingReward.PointAccountID = PointAccounts.BookingsRewardsCash;
            return bookingReward;
        }

        #endregion
        #region Host Reminders
       List<Common.CRMContext.HostReminder> IEventService.GetHostReminders()
        {
            try
            {
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("GetHostReminders");
                    List<Common.CRMContext.HostReminder> HostReminderList = context.Query<Common.CRMContext.HostReminder>(sqlProcedure).ToList();
                    return HostReminderList;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
       public bool InsertCustomerHostReminders(int CustomerID, int GetTogetherID, DateTime GTStartDate)
       {

           try
           {
               using (var context = Exigo.Sql())
               {
                   string sqlProcedure = string.Format("InsertCustomerHostReminders {0},{1},'{2}'",CustomerID,GetTogetherID,GTStartDate);
                   return context.Query<Common.CRMContext.HostReminder>(sqlProcedure).ToList().Count > 0;
               }
           }
           catch (Exception ex)
           {
               return false;
           }

       }
       public bool UpdateCustomerHostReminders(int GetTogetherID)
       {

           try
           {
               using (var context = Exigo.Sql())
               {
                   string sqlProcedure = string.Format("DeleteCustomerHostReminderByGTId {0}", GetTogetherID);
                   return context.Query<Common.CRMContext.HostReminder>(sqlProcedure).ToList().Count > 0;
               }
           }
           catch (Exception ex)
           {
               return false;
           }

       }
        #endregion

        #region Create Event

        EventConfirmation IEventService.CreateEvent(Common.ModelsEx.Event.EventBooking eventBooking)
        {
            var confirmation = new EventConfirmation();
            TransactionalResponse result = null;

            try
            {
                var requests = new List<ApiRequest>();

                // Need to create customer if new party customer
                if (eventBooking.Customer.CustomerID == 0)
                {
                    // TODO: gwb:20150111 - What is the default warehouse for a party customer?
                    var createCustomerRequest = new CreateCustomerRequest(eventBooking.Customer);

                    requests.Add(createCustomerRequest);
                }
                else
                {
                    throw new ApplicationException("Cannot call CreateEvent for an existing event.  Call EditEvent instead.");
                }

                var setCustomerSiteRequest = new SetCustomerSiteRequest(eventBooking.CustomerSite);
                requests.Add(setCustomerSiteRequest);

                var hostSpecial = RewardService.GetHostSpecialReward(eventBooking.ActualStartDate);
                var createCustomerExtendedRequest = new CreateCustomerExtendedRequest()
                {
                    CustomerID = eventBooking.Customer.CustomerID,
                    ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails, // Party Extended Fields
                    Field1 = eventBooking.StartDate.ToString(CultureInfo.InvariantCulture),
                    Field2 = eventBooking.ActualStartDate.ToString(CultureInfo.InvariantCulture),
                    Field3 = eventBooking.CloseDate.ToString(CultureInfo.InvariantCulture),
                    Field4 = eventBooking.LockoutDate.ToString(CultureInfo.InvariantCulture),
                    Field5 = eventBooking.CreatorCustomerID.ToString(),
                    Field6 = eventBooking.HostCustomerID.ToString(),
                    Field8 = hostSpecial.ItemCode,
                    Field9 = hostSpecial.DiscountAmount.ToString(CultureInfo.InvariantCulture),
                    Field10 = string.Empty,  // Reward Not Redeemed
                    Field11 = hostSpecial.SalesThreshold.ToString(CultureInfo.InvariantCulture),
                    Field12=eventBooking.PartyDate.ToString(CultureInfo.InvariantCulture),
                    Field13 =eventBooking.PartyStartTime,
                    Field14=eventBooking.PartyEndTime,
                    Field15=eventBooking.TimeZone
                };
                requests.Add(createCustomerExtendedRequest);
                result = Api.ProcessTransaction(new TransactionalRequest
                {
                    TransactionRequests = requests.ToArray()
                });
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your event was created but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while creating your event: " + ex.Message);

                return confirmation;
            }

            ProcessCreatePartyCustomerResponse(result, eventBooking, confirmation);
            // save new event entry into database to overcome delay 
            try
            {
            using (var context = Exigo.Sql())
            {                
                string sqlProcedure = string.Format("InsertNewCreatedEvent {0},{1},'{2}','{3}','{4}','{5}','{6}'", confirmation.CustomerID, eventBooking.CreatorCustomerID, eventBooking.Customer.FirstName, eventBooking.ActualStartDate.ToString(CultureInfo.InvariantCulture), eventBooking.StartDate.ToString(CultureInfo.InvariantCulture), eventBooking.CloseDate.ToString(CultureInfo.InvariantCulture), eventBooking.LockoutDate.ToString(CultureInfo.InvariantCulture));
                bool eventinserted   = context.Query<bool>(sqlProcedure).FirstOrDefault();
            }
            }
            catch (Exception EX )
            {
                
            }
            
            return confirmation;
        }

        protected void ProcessCreatePartyCustomerResponse(TransactionalResponse result, EventBooking eventBooking, EventConfirmation confirmation)
        {
            try
            {
                if (IsSuccess(result) &&
                    result.TransactionResponses != null &&
                    result.TransactionResponses.Length != 0)
                {
                    confirmation.Result.Status = Status.Success;

                    var responses = result.TransactionResponses;

                    ProcessCreateCustomerResponse(responses, confirmation);

                    ProcessExigoApiResponse(typeof(SetCustomerSiteResponse), responses, confirmation);

                    ProcessExigoApiResponse(typeof(CreateCustomerExtendedResponse), responses, confirmation);
                }
                else
                {
                    confirmation.Result.Status = Status.Failure;
                    ProcessErrors(result, confirmation);
                }
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your event was created but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while creating your event: " + ex.Message);
            }

        }

        #endregion

        #region Edit Event

        EventConfirmation IEventService.EditEvent(Common.ModelsEx.Event.EventBooking eventBooking)
        {
            var confirmation = new EventConfirmation();
            TransactionalResponse result = null;

            try
            {
                var requests = new List<ApiRequest>();

                // Need to create customer if new party customer
                if (eventBooking.Customer.CustomerID != 0)
                {
                    // TODO: gwb:20150111 - Need clarification which fields can be updated for a party
                    // Right now, updating party location
                    var updateCustomerRequest = new UpdateCustomerRequest
                    {
                        CustomerID = eventBooking.Customer.CustomerID,
                        FirstName = eventBooking.Customer.FirstName,
                        LastName = eventBooking.Customer.LastName,
                        MainAddress1 = eventBooking.Customer.MainAddress.Address1,
                        MainAddress2 = eventBooking.Customer.MainAddress.Address2,
                        MainCity = eventBooking.Customer.MainAddress.City,
                        MainState = eventBooking.Customer.MainAddress.State,
                        MainZip = eventBooking.Customer.MainAddress.Zip,
                        MainCountry = eventBooking.Customer.MainAddress.Country
                    };
                    requests.Add(updateCustomerRequest);
                }
                else
                {
                    throw new ApplicationException("Cannot call EditEvent for a new event.  Call CreateEvent instead.");
                }

                var setCustomerSiteRequest = new SetCustomerSiteRequest(eventBooking.CustomerSite);
                requests.Add(setCustomerSiteRequest);

                var getCustomerExtendedRequest = new GetCustomerExtendedRequest()
                {
                    CustomerID = eventBooking.Customer.CustomerID,
                    ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails // Party Extended Fields
                };
                var getCustomerExtendedResponse = Exigo.GetCustomerExtendedDetails(eventBooking.Customer.CustomerID, (int)CustomerExtendedGroup.PartyDetails).FirstOrDefault();

                var updateCustomerExtendedRequest = new UpdateCustomerExtendedRequest()
                {
                    CustomerExtendedID = getCustomerExtendedResponse.CustomerExtendedDetailID,
                    CustomerID = eventBooking.Customer.CustomerID,
                    ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails, // Party Extended Fields
                    Field2 = eventBooking.ActualStartDate.ToString(),
                    Field3 = eventBooking.CloseDate.ToString(),
                    Field4 = eventBooking.LockoutDate.ToString(),
                    Field12=eventBooking.PartyDate.ToString(),
                    Field13=eventBooking.PartyStartTime,
                    Field14=eventBooking.PartyEndTime,
                    Field15=eventBooking.TimeZone
                };
                requests.Add(updateCustomerExtendedRequest);

                result = Api.ProcessTransaction(new TransactionalRequest
                {
                    TransactionRequests = requests.ToArray()
                });
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your event was updated but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while updating your event: " + ex.Message);

                return confirmation;
            }
            try
            {
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("UpdateEvent {0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'", eventBooking.Customer.CustomerID, eventBooking.Customer.FirstName, eventBooking.ActualStartDate.ToString(CultureInfo.InvariantCulture), eventBooking.CloseDate.ToString(CultureInfo.InvariantCulture), eventBooking.LockoutDate.ToString(CultureInfo.InvariantCulture), eventBooking.PartyStartTime, eventBooking.PartyEndTime,eventBooking.TimeZone,eventBooking.CustomerSite.WebAlias);
                    bool eventUpdated = context.Query<bool>(sqlProcedure).FirstOrDefault();
                }
            }
            catch (Exception EX)
            {

            }

            ProcessUpdatePartyCustomerResponse(result, eventBooking, confirmation);
            return confirmation;
        }

        protected void ProcessUpdatePartyCustomerResponse(TransactionalResponse result, EventBooking eventBooking, EventConfirmation confirmation)
        {
            try
            {
                if (IsSuccess(result) &&
                    result.TransactionResponses != null &&
                    result.TransactionResponses.Length != 0)
                {
                    confirmation.Result.Status = Status.Success;

                    var responses = result.TransactionResponses;

                    ProcessExigoApiResponse(typeof(UpdateCustomerResponse), responses, confirmation);

                    ProcessExigoApiResponse(typeof(SetCustomerSiteResponse), responses, confirmation);

                    ProcessExigoApiResponse(typeof(UpdateCustomerExtendedResponse), responses, confirmation);
                }
                else
                {
                    confirmation.Result.Status = Status.Failure;

                    ProcessWarnings(result, confirmation);
                    ProcessErrors(result, confirmation);
                }
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your event was updated but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while updateing your event: " + ex.Message);
            }

        }

        #endregion

        #region UserNameExists

        public bool EventNameCheck(string webalias)
        {
            //========Event Name Already Exist============
            var CustomerResponse = Exigo.GetCustomerByWebalias(webalias);

            if (CustomerResponse.Count > 0)
            {
                return true;
            }

            return false;

        }

        #endregion

        #region API Response Processing Methods

        protected void ProcessCreateCustomerResponse(ApiResponse[] responses, EventConfirmation confirmation)
        {
            var createCustomerResponse = GetExigoApiResponse(typeof(CreateCustomerResponse), responses) as CreateCustomerResponse;

            if (ProcessExigoApiResponse(createCustomerResponse, confirmation))
            {
                confirmation.CustomerID = createCustomerResponse.CustomerID;
                return;
            }
        }

        #endregion
    }
}
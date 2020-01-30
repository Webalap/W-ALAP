using AutoMapper;
using Common.Api.ExigoWebService;
using ExigoService;
using Ninject.Modules;

namespace Common.ServicesEx
{
    public class ServicesModule : NinjectModule
    {
        /// <summary>
        /// Gets the singleton instance of this module.
        /// </summary>
        /// <example>To load this module, call <c>kernel.Load(ServicesModule.Instance);</c></example>
        public static ServicesModule Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Loads all the bindings for this module.
        /// </summary>
        public override void Load()
        {
            BindExigoApi();
            BindMappings();
            BindConfigurations();
            BindServices();
        }

        private void BindExigoApi()
        {
            // ExigoApi is the object to use for all your Exigo API calls.
            // To include this in your class, define the following:
            //   [Inject]
            //   public ExigoApi Api { get; set; }
            Bind<ExigoApi>().ToMethod(context =>
                ExigoService.Exigo.WebService()).InSingletonScope();
        }

        private void BindMappings()
        {
            // TODO: BRIAN F. - This is not working properly in OrderService.
            Bind<IMappingEngine>().ToConstant(Mapper.Engine).InSingletonScope();
        }

        private void BindConfigurations()
        {
            // TODO: BRIAN F. - Bind<IOrderConfiguration>().ToMethod(context => Identity.Current.Market.Configuration.Orders).InTransientScope();
            // TODO: NEED TO CHECK IF GOING TO REUSE
            Bind<IOrderConfiguration>().To<UnitedStatesConfiguration.CustomerOrderConfiguration>();
        }

        private void BindServices()
        {
            // IProductService was created to return items that can be added to carts/orders.
            Bind<IProductService>().To<ProductService>().InTransientScope();

            // IShippingService was created as a centralized place to
            // retrieve shipping methods.
            Bind<IShippingService>().To<ShippingService>().InTransientScope();

            // IOrderService was created to provide a common interface
            // for interacting with orders and calculating totals.
            Bind<IOrderService>().To<OrderService>().InTransientScope();

            // IEventService was created to provide party planning methods.
            Bind<IEventService>().To<EventService>().InTransientScope();

            // IRewardsService was created to provide party planning methods.
            Bind<IRewardService>().To<RewardService>().InTransientScope();

            // IDiscountValidator was created to provide a common
            // mechanism for validating whether a discount can
            // be applied to a particular product.
            Bind<IDiscountValidator>().To<ProductEligibilityDiscountValidator>().InTransientScope();
        }

        /// <summary>
        /// Private constructor.  All references to this class should use <c>ServicesModule.Instance</c>.
        /// </summary>
        private ServicesModule() { }

        private static readonly ServicesModule _instance = new ServicesModule();
    }
}
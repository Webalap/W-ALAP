using Common.Api.ExigoWebService;
using Common.ModelsEx.Base;
using Ninject;
using System;
using System.Linq;
using System.Reflection;

namespace Common.ServicesEx.Base
{
    public class ServiceBase
    {
        #region Dependencies

        /// <summary>
        /// The Exigo API.
        /// </summary>
        [Inject]
        public ExigoApi Api { get; set; }

        #endregion

        #region Process Exigo API Response Methods

        protected ApiResponse GetExigoApiResponse(Type responseType, ApiResponse[] responses)
        {
            var apiResponse = responses
                .FirstOrDefault(s => responseType.Equals(s.GetType()));

            if (null != apiResponse)
            {
                return apiResponse;
            }

            throw new ApplicationException("Exigo API response was not found.");
        }

        protected bool ProcessExigoApiResponse(ApiResponse apiResponse, ServiceResponse confirmation)
        {
            if (IsSuccess(apiResponse))
            {
                return true;
            }
            else
            {
                ProcessWarnings(apiResponse, confirmation);
                ProcessErrors(apiResponse, confirmation);
                return false;
            }
        }

        protected bool ProcessExigoApiResponse(Type responseType, ApiResponse[] responses, ServiceResponse confirmation)
        {
            var apiResponse = GetExigoApiResponse(responseType, responses);

            return ProcessExigoApiResponse(apiResponse, confirmation);
        }

        #endregion

        #region Process Exigo API Warnings

        protected virtual void ProcessWarnings(ApiResponse response, ServiceResponse confirmation)
        {
            if (null == response)
                return;

            Type type = response.GetType();
            PropertyInfo pi = type.GetProperty("Warnings");
            if (null != pi)
            {
                var warnings = pi.GetValue(response) as string[];
                if (null != warnings)
                {
                    foreach (string warning in warnings)
                        confirmation.Result.Warnings.Add(warning);
                }
            }
        }

        #endregion

        #region Process Exigo API Errors

        protected virtual void ProcessErrors(ApiResponse response, ServiceResponse confirmation)
        {
            if (null != response &&
                null != response.Result &&
                null != response.Result.Errors)
            {
                foreach (string error in response.Result.Errors)
                    confirmation.Result.Errors.Add(error);
            }
        }

        protected virtual void ProcessErrors(ApiResult result, ServiceResponse confirmation)
        {
            if (null != result
                && result.Errors != null)
            {
                foreach (string error in result.Errors)
                    confirmation.Result.Errors.Add(error);
            }
        }

        #endregion

        #region Success Checker Methods

        protected bool IsSuccess(ApiResponse response)
        {
            return (null != response &&
                null != response.Result &&
                ResultStatus.Success.Equals(response.Result.Status));
        }

        protected bool IsSuccess(ApiResult result)
        {
            return (null != result &&
                ResultStatus.Success.Equals(result.Status));
        }

        #endregion
    }
}
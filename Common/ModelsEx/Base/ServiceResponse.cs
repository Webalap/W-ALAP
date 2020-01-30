namespace Common.ModelsEx.Base
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            Result = new ServiceResult();
        }

        public ServiceResult Result { get; set; }
    }
}
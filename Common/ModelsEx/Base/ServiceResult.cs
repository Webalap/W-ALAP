using System.Collections.Generic;

namespace Common.ModelsEx.Base
{
    public enum Status
    { 
        Success = 0,
        Failure = 1
    }

    public class ServiceResult
    {
        public ServiceResult()
        {
            Warnings = new List<string>();
            Errors = new List<string>();
        }

        public Status Status { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }
    }
}
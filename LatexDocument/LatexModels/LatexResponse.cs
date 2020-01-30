using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatexDocument.LatexModels
{
    public class LatexResponse
    {
        public bool Status { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }

        public LatexResponse()
        {

        }

        public LatexResponse(bool status, int errorCode, string errorString)
        {
            Status = status;
            ErrorCode = errorCode;
            ErrorString = errorString;
        }
    }
}

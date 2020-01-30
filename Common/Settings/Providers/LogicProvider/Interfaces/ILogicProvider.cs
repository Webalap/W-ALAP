using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.Providers
{
    public interface ILogicProvider
    {
        Controller Controller { get; set; }

        CheckLogicResult CheckLogic();
        ActionResult GetNextAction();

        bool IsAuthenticated();
    }
}

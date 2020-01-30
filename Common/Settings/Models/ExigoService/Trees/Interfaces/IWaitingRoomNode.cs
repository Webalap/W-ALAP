using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    interface IWaitingRoomNode
    {
        int CustomerID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int? EnrollerID { get; set; }
        DateTime EnrollmentDate { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
    }
}
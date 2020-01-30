using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Common.Api.ExigoWebService
{
    public partial class ItemMemberResponse
    {
        public static explicit operator ExigoService.ItemGroupMember(ItemMemberResponse item)
        {
            var model = new ExigoService.ItemGroupMember();
            if (item == null) return model;

            model.ItemCode             = item.ItemCode;
            model.MemberDescription    = item.MemberDescription;

            return model;
        }
    }
}
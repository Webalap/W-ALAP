using Common.Api.ExigoWebService;
using System;
using System.Linq;
using System.Text;

namespace Common.ModelsEx.Email
{
    public class InvoiceDetail : OrderResponse
    {
        public InvoiceDetail()
        {

        }

        public InvoiceDetail(OrderResponse order)
        {
            this.OrderID = order.OrderID;
            this.CustomerID = order.CustomerID;
            this.AutoOrderID = 0;

            this.FirstName = order.FirstName;
            this.LastName = order.LastName;
            
            this.CurrencyCode = order.CurrencyCode;
            this.WarehouseID = order.WarehouseID;
            this.ShipMethodID = order.ShipMethodID;
            this.OrderStatus = order.OrderStatus;
            this.OrderType = order.OrderType;
            this.PriceType = order.PriceType;
            this.Notes = order.Notes;
            
            this.CreatedDate = order.CreatedDate;
            this.ModifiedDate = order.ModifiedDate;
            this.OrderDate = order.OrderDate;
            this.ShippedDate = order.ShippedDate;
            this.Address1 = order.Address1;
            this.Address2 = order.Address2;
            this.City = order.City;
            this.State = order.State;
            this.Zip = order.Zip;
            this.Country = order.Country;
            this.Email = order.Email;
            this.Phone = order.Phone;
            this.Total = order.Total;
            this.SubTotal = order.SubTotal;
            this.TaxTotal = order.TaxTotal;
            this.ShippingTotal = order.ShippingTotal;
            this.DiscountTotal = order.DiscountTotal;
            this.DiscountPercent = order.DiscountPercent;
            this.WeightTotal = order.WeightTotal;
            this.BusinessVolumeTotal = order.BusinessVolumeTotal;
            this.CommissionableVolumeTotal = order.CommissionableVolumeTotal;
            this.TrackingNumber1 = order.TrackingNumber1;
            this.TrackingNumber2 = order.TrackingNumber2;
            this.TrackingNumber3 = order.TrackingNumber3;
            this.TrackingNumber4 = order.TrackingNumber4;
            this.TrackingNumber5 = order.TrackingNumber5;
            
            this.Other1Total = order.Other1Total;
            this.Other2Total = order.Other2Total;
            this.Other3Total = order.Other3Total;
            this.Other4Total = order.Other4Total;
            this.Other5Total = order.Other5Total;
            this.Other6Total = order.Other6Total;
            this.Other7Total = order.Other7Total;
            this.Other8Total = order.Other8Total;
            this.Other9Total = order.Other9Total;
            this.Other10Total = order.Other10Total;

            this.Details = order.Details;
            this.Payments = order.Payments;
            this.Other11 = order.Other11;
            this.Other12 = order.Other12;
            this.Other13 = order.Other13;
            this.Other14 = order.Other14;
            this.Other15 = order.Other15;
            this.Other16 = order.Other16;
            this.Other17 = order.Other17;
            this.Other18 = order.Other18;
            this.Other19 = order.Other19;
            this.Other20 = order.Other20;
        }
        public string TrackingUrl
        {
            get
            {
                return string.Format("<a href=\"https://www.google.com.pk/search?q={0}\">{0}</a>",this.TrackingNumber1);
            }
        }
        public string AddressDisplay
        {
            get
            {
                return string.Format("{0}, {1}, {2} {3}, {4}", this.Address1, this.City, this.State, this.Zip, this.Country);
            }
        }
        public string OrderDateDisplay
        {
            get
            {
                return OrderDate.ToShortDateString();
            }
        }
        public string CreditCardDisplay
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Payments)
                {
                    sb.Append(string.Format("<div>Card ending in  {0} - {1}</div>", item.CreditCardNumberDisplay, item.Amount.ToString("C")));
                }
                return sb.ToString();
            }
        }
        public string ItemsDisplay
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Details)
                {
                    sb.Append(string.Format(@"<tr>
                                                <td style='padding-top: 3px;text-align:left;'>{0}</td>
                                                <td style='padding-top: 3px;text-align:left;'>{1}</td>
                                                <td style='padding-top: 3px;text-align:left;'>{2}</td>
                                                <td style='padding-top: 3px;text-align:left;'>{3}</td>
                                                <td style='padding-top: 3px;text-align:left;'>{4}</td>
                                    </tr>", item.ItemCode,
                                    item.Description,
                                    Convert.ToInt32(item.Quantity),
                                    item.PriceEach.ToString("C"),
                                    item.PriceTotal.ToString("C")));
                }
                return sb.ToString();
            }
        }

        public string TotalPaidAmmont
        {
            get
            {
                return Payments.Sum(i => i.Amount).ToString("C");
            }
        }

        public string SubTotalDisplay
        {
            get
            {
                return this.SubTotal.ToString("C");
            }
        }

        public string ShippingTotalDisplay
        {
            get
            {
                return this.ShippingTotal.ToString("C");
            }
        }

        public string TaxTotalDisplay
        {
            get
            {
                return this.TaxTotal.ToString("C");
            }
        }

        public string TotalDisplay
        {
            get
            {
                return this.Total.ToString("C");
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerEmail { get; set; }

    public string? CustomerAddress { get; set; }

    public int? DriverId { get; set; }

    public string? DriverName { get; set; }

    public string? DriverLastName { get; set; }

    public string? VendorName { get; set; }

    public int VendorId { get; set; }

    public int OrderStatusId { get; set; }

    public string? Status { get; set; }

    public int OrderDeliveryId { get; set; }

    public string? DeliveryType { get; set; }

    public int OrderPayMethodId { get; set; }

    public string? PayMethod { get; set; }

    public string? PayData { get; set; }

    public string? AppId { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string? DeliveryTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? AcceptedDate { get; set; }

    public DateTime? PickupReadyDate { get; set; }

    public DateTime? DriverAcceptedDate { get; set; }

    public DateTime? DriverPickupDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal? TaxRatePerc { get; set; }

    public decimal? Tax { get; set; }

    public int? TaxType { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? DriverTip { get; set; }

    public decimal? ServiceFeeRate { get; set; }

    public decimal? ServiceFee { get; set; }

    public string? DiscountType { get; set; }

    public decimal? DiscountRate { get; set; }

    public decimal? Discount { get; set; }

    public decimal Total { get; set; }

    public decimal? LogisticsRate { get; set; }

    public decimal? LogisticsFee { get; set; }

    public string? Refund { get; set; }

    public string? RefundData { get; set; }

    public int? DeliveryZoneId { get; set; }

    public int? OrderGroupId { get; set; }

    public int LogisticStatus { get; set; }

    public int? PrepareMinutes { get; set; }

    public DateTime? RejectDate { get; set; }

    public string? RejectReason { get; set; }

    public DateTime? DriverRejectDate { get; set; }

    public DateTime? PickupFailedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<PekkishOrderDetail> PekkishOrderDetails { get; } = new List<PekkishOrderDetail>();

    public virtual ICollection<PekkishOrderHistory> PekkishOrderHistories { get; } = new List<PekkishOrderHistory>();
}

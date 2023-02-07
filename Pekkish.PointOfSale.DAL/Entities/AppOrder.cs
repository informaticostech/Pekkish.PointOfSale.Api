using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrder
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Brand { get; set; }

    public int? LocationId { get; set; }

    public int? OrderStatusId { get; set; }

    public int? SalesChannelId { get; set; }

    public int? OrderFulfillmentId { get; set; }

    public int? PaymentMethodId { get; set; }

    public int? SeatingAreaId { get; set; }

    public DateTime EffectiveDate { get; set; }

    public Guid? AcceptedUser { get; set; }

    public DateTime? AcceptedDate { get; set; }

    public DateTime? PickupReadyDate { get; set; }

    public Guid? PickupReadyUser { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? CompletedUser { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxRatePerc { get; set; }

    public decimal? Tax { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? DriverTip { get; set; }

    public decimal? ServiceFeeRate { get; set; }

    public decimal? ServiceFee { get; set; }

    public int? DiscountType { get; set; }

    public decimal? DiscountRate { get; set; }

    public decimal? Discount { get; set; }

    public decimal Total { get; set; }

    public decimal? PaidCash { get; set; }

    public decimal? PaidCard { get; set; }

    public decimal? PaidOnline { get; set; }

    public decimal? PaidEft { get; set; }

    public decimal? PaidOther { get; set; }

    public decimal PaidTotal { get; set; }

    public decimal? PaidTip { get; set; }

    public decimal PaidChange { get; set; }

    public DateTime? PaidDate { get; set; }

    public Guid? PaidUser { get; set; }

    public decimal? Refund { get; set; }

    public string? RefundReason { get; set; }

    public DateTime? RefundDate { get; set; }

    public int? PrepareMinutes { get; set; }

    public DateTime? RejectDate { get; set; }

    public Guid? RejectUser { get; set; }

    public string? RejectReason { get; set; }

    public DateTime? PickupFailedDate { get; set; }

    public string? PickupFailedReason { get; set; }

    public string? ExternalId { get; set; }

    public string? ExternalCustomerName { get; set; }

    public string? ExternalPayMethod { get; set; }

    public bool IsMultiBrand { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public virtual ICollection<AppOrderBrand> AppOrderBrands { get; } = new List<AppOrderBrand>();

    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; } = new List<AppOrderDetail>();

    public virtual ICollection<AppOrderRequest> AppOrderRequests { get; } = new List<AppOrderRequest>();

    public virtual ICollection<AppWatiOrder> AppWatiOrders { get; } = new List<AppWatiOrder>();

    public virtual AppLocation? Location { get; set; }

    public virtual AppOrderFulfillment? OrderFulfillment { get; set; }

    public virtual AppOrderStatus? OrderStatus { get; set; }

    public virtual AppPaymentMethod? PaymentMethod { get; set; }

    public virtual AppSalesChannel? SalesChannel { get; set; }
}

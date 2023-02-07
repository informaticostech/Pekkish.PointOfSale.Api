using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiOrder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int WaId { get; set; }

    public int WatiConversationId { get; set; }

    public int WatiOrderStatusId { get; set; }

    public Guid? TenantId { get; set; }

    public int? TenantInfoId { get; set; }

    public int? LocationId { get; set; }

    public int? OrderFulfillmentId { get; set; }

    public int? PaymentMethodId { get; set; }

    public int? PosOrderId { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? AcceptedDate { get; set; }

    public Guid? AcceptedUser { get; set; }

    public DateTime? RejectDate { get; set; }

    public Guid? RejectUser { get; set; }

    public string? RejectReason { get; set; }

    public int? PrepareMinutes { get; set; }

    public int? CurrentOrderDetail { get; set; }

    public int? CurrentBrand { get; set; }

    public int? CurrentCategory { get; set; }

    public int? CurrentProduct { get; set; }

    public int? CurrentProductExtra { get; set; }

    public virtual AppLocation? Location { get; set; }

    public virtual AppPaymentMethod? PaymentMethod { get; set; }

    public virtual AppOrder? PosOrder { get; set; }

    public virtual AppTenantInfo? TenantInfo { get; set; }

    public virtual AppWatiConversation WatiConversation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PointOfSaleContext : DbContext
{
    public PointOfSaleContext()
    {
    }

    public PointOfSaleContext(DbContextOptions<PointOfSaleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppWatiOrderDetailOption> AppWatiOrderDetailOptions { get; set; }

    public virtual DbSet<AbpAuditLog> AbpAuditLogs { get; set; }

    public virtual DbSet<AbpAuditLogAction> AbpAuditLogActions { get; set; }

    public virtual DbSet<AbpBackgroundJob> AbpBackgroundJobs { get; set; }

    public virtual DbSet<AbpBlob> AbpBlobs { get; set; }

    public virtual DbSet<AbpBlobContainer> AbpBlobContainers { get; set; }

    public virtual DbSet<AbpClaimType> AbpClaimTypes { get; set; }

    public virtual DbSet<AbpEntityChange> AbpEntityChanges { get; set; }

    public virtual DbSet<AbpEntityPropertyChange> AbpEntityPropertyChanges { get; set; }

    public virtual DbSet<AbpFeatureValue> AbpFeatureValues { get; set; }

    public virtual DbSet<AbpLanguage> AbpLanguages { get; set; }

    public virtual DbSet<AbpLanguageText> AbpLanguageTexts { get; set; }

    public virtual DbSet<AbpLinkUser> AbpLinkUsers { get; set; }

    public virtual DbSet<AbpOrganizationUnit> AbpOrganizationUnits { get; set; }

    public virtual DbSet<AbpOrganizationUnitRole> AbpOrganizationUnitRoles { get; set; }

    public virtual DbSet<AbpPermissionGrant> AbpPermissionGrants { get; set; }

    public virtual DbSet<AbpRole> AbpRoles { get; set; }

    public virtual DbSet<AbpRoleClaim> AbpRoleClaims { get; set; }

    public virtual DbSet<AbpSecurityLog> AbpSecurityLogs { get; set; }

    public virtual DbSet<AbpSetting> AbpSettings { get; set; }

    public virtual DbSet<AbpTextTemplateContent> AbpTextTemplateContents { get; set; }

    public virtual DbSet<AbpUser> AbpUsers { get; set; }

    public virtual DbSet<AbpUserClaim> AbpUserClaims { get; set; }

    public virtual DbSet<AbpUserLogin> AbpUserLogins { get; set; }

    public virtual DbSet<AbpUserOrganizationUnit> AbpUserOrganizationUnits { get; set; }

    public virtual DbSet<AbpUserRole> AbpUserRoles { get; set; }

    public virtual DbSet<AbpUserToken> AbpUserTokens { get; set; }

    public virtual DbSet<AppBrand> AppBrands { get; set; }

    public virtual DbSet<AppExpense> AppExpenses { get; set; }

    public virtual DbSet<AppExpenseDetail> AppExpenseDetails { get; set; }

    public virtual DbSet<AppFinanceAccount> AppFinanceAccounts { get; set; }

    public virtual DbSet<AppFinanceAccountCategory> AppFinanceAccountCategories { get; set; }

    public virtual DbSet<AppFinanceAccountCategoryParent> AppFinanceAccountCategoryParents { get; set; }

    public virtual DbSet<AppFinanceVatType> AppFinanceVatTypes { get; set; }

    public virtual DbSet<AppImportProductCategory> AppImportProductCategories { get; set; }

    public virtual DbSet<AppLocation> AppLocations { get; set; }

    public virtual DbSet<AppLocationBrand> AppLocationBrands { get; set; }

    public virtual DbSet<AppOrder> AppOrders { get; set; }

    public virtual DbSet<AppOrderBrand> AppOrderBrands { get; set; }

    public virtual DbSet<AppOrderDetail> AppOrderDetails { get; set; }

    public virtual DbSet<AppOrderDetailOption> AppOrderDetailOptions { get; set; }

    public virtual DbSet<AppOrderFulfillment> AppOrderFulfillments { get; set; }

    public virtual DbSet<AppOrderKitchenScreen> AppOrderKitchenScreens { get; set; }

    public virtual DbSet<AppOrderPrintQueue> AppOrderPrintQueues { get; set; }

    public virtual DbSet<AppOrderRequest> AppOrderRequests { get; set; }

    public virtual DbSet<AppOrderRequestBrand> AppOrderRequestBrands { get; set; }

    public virtual DbSet<AppOrderStatus> AppOrderStatuses { get; set; }

    public virtual DbSet<AppPaymentMethod> AppPaymentMethods { get; set; }

    public virtual DbSet<AppProduct> AppProducts { get; set; }

    public virtual DbSet<AppProductCategory> AppProductCategories { get; set; }

    public virtual DbSet<AppProductExtra> AppProductExtras { get; set; }

    public virtual DbSet<AppProductExtraLink> AppProductExtraLinks { get; set; }

    public virtual DbSet<AppProductExtraOption> AppProductExtraOptions { get; set; }

    public virtual DbSet<AppProductRecipe> AppProductRecipes { get; set; }

    public virtual DbSet<AppRateSheet> AppRateSheets { get; set; }

    public virtual DbSet<AppSalesChannel> AppSalesChannels { get; set; }

    public virtual DbSet<AppSeatingArea> AppSeatingAreas { get; set; }

    public virtual DbSet<AppStock> AppStocks { get; set; }

    public virtual DbSet<AppStockCategory> AppStockCategories { get; set; }

    public virtual DbSet<AppStockLocation> AppStockLocations { get; set; }

    public virtual DbSet<AppStockLocationHistory> AppStockLocationHistories { get; set; }

    public virtual DbSet<AppStockLocationHistoryReason> AppStockLocationHistoryReasons { get; set; }

    public virtual DbSet<AppStockPurchaseDetail> AppStockPurchaseDetails { get; set; }

    public virtual DbSet<AppStockPurchaseHeader> AppStockPurchaseHeaders { get; set; }

    public virtual DbSet<AppStockUnit> AppStockUnits { get; set; }

    public virtual DbSet<AppSupplier> AppSuppliers { get; set; }

    public virtual DbSet<AppTenantInfo> AppTenantInfos { get; set; }

    public virtual DbSet<AppUserBrand> AppUserBrands { get; set; }

    public virtual DbSet<AppUserConfig> AppUserConfigs { get; set; }

    public virtual DbSet<AppUserLocation> AppUserLocations { get; set; }

    public virtual DbSet<AppWatiConversation> AppWatiConversations { get; set; }

    public virtual DbSet<AppWatiConversationStatus> AppWatiConversationStatuses { get; set; }

    public virtual DbSet<AppWatiConversationType> AppWatiConversationTypes { get; set; }

    public virtual DbSet<AppWatiMessage> AppWatiMessages { get; set; }

    public virtual DbSet<AppWatiOrder> AppWatiOrders { get; set; }

    public virtual DbSet<AppWatiOrderDetail> AppWatiOrderDetails { get; set; }

    public virtual DbSet<AppWatiOrderStatus> AppWatiOrderStatuses { get; set; }

    public virtual DbSet<HubspotDriverEnquiry> HubspotDriverEnquiries { get; set; }

    public virtual DbSet<HubspotVendorTerm> HubspotVendorTerms { get; set; }

    public virtual DbSet<IdentityServerApiResource> IdentityServerApiResources { get; set; }

    public virtual DbSet<IdentityServerApiResourceClaim> IdentityServerApiResourceClaims { get; set; }

    public virtual DbSet<IdentityServerApiResourceProperty> IdentityServerApiResourceProperties { get; set; }

    public virtual DbSet<IdentityServerApiResourceScope> IdentityServerApiResourceScopes { get; set; }

    public virtual DbSet<IdentityServerApiResourceSecret> IdentityServerApiResourceSecrets { get; set; }

    public virtual DbSet<IdentityServerApiScope> IdentityServerApiScopes { get; set; }

    public virtual DbSet<IdentityServerApiScopeClaim> IdentityServerApiScopeClaims { get; set; }

    public virtual DbSet<IdentityServerApiScopeProperty> IdentityServerApiScopeProperties { get; set; }

    public virtual DbSet<IdentityServerClient> IdentityServerClients { get; set; }

    public virtual DbSet<IdentityServerClientClaim> IdentityServerClientClaims { get; set; }

    public virtual DbSet<IdentityServerClientCorsOrigin> IdentityServerClientCorsOrigins { get; set; }

    public virtual DbSet<IdentityServerClientGrantType> IdentityServerClientGrantTypes { get; set; }

    public virtual DbSet<IdentityServerClientIdPrestriction> IdentityServerClientIdPrestrictions { get; set; }

    public virtual DbSet<IdentityServerClientPostLogoutRedirectUri> IdentityServerClientPostLogoutRedirectUris { get; set; }

    public virtual DbSet<IdentityServerClientProperty> IdentityServerClientProperties { get; set; }

    public virtual DbSet<IdentityServerClientRedirectUri> IdentityServerClientRedirectUris { get; set; }

    public virtual DbSet<IdentityServerClientScope> IdentityServerClientScopes { get; set; }

    public virtual DbSet<IdentityServerClientSecret> IdentityServerClientSecrets { get; set; }

    public virtual DbSet<IdentityServerDeviceFlowCode> IdentityServerDeviceFlowCodes { get; set; }

    public virtual DbSet<IdentityServerIdentityResource> IdentityServerIdentityResources { get; set; }

    public virtual DbSet<IdentityServerIdentityResourceClaim> IdentityServerIdentityResourceClaims { get; set; }

    public virtual DbSet<IdentityServerIdentityResourceProperty> IdentityServerIdentityResourceProperties { get; set; }

    public virtual DbSet<IdentityServerPersistedGrant> IdentityServerPersistedGrants { get; set; }

    public virtual DbSet<PayGatewayPlan> PayGatewayPlans { get; set; }

    public virtual DbSet<PayPaymentRequest> PayPaymentRequests { get; set; }

    public virtual DbSet<PayPaymentRequestProduct> PayPaymentRequestProducts { get; set; }

    public virtual DbSet<PayPlan> PayPlans { get; set; }

    public virtual DbSet<PekkishCustomer> PekkishCustomers { get; set; }

    public virtual DbSet<PekkishDeliveryGroup> PekkishDeliveryGroups { get; set; }

    public virtual DbSet<PekkishDeliveryGroupDriver> PekkishDeliveryGroupDrivers { get; set; }

    public virtual DbSet<PekkishDeliveryManager> PekkishDeliveryManagers { get; set; }

    public virtual DbSet<PekkishDriver> PekkishDrivers { get; set; }

    public virtual DbSet<PekkishDriverGroup> PekkishDriverGroups { get; set; }

    public virtual DbSet<PekkishDriverGroupDriver> PekkishDriverGroupDrivers { get; set; }

    public virtual DbSet<PekkishDriverGroupVendor> PekkishDriverGroupVendors { get; set; }

    public virtual DbSet<PekkishDriverRoster> PekkishDriverRosters { get; set; }

    public virtual DbSet<PekkishDriverSignup> PekkishDriverSignups { get; set; }

    public virtual DbSet<PekkishExpense> PekkishExpenses { get; set; }

    public virtual DbSet<PekkishExpenseDetail> PekkishExpenseDetails { get; set; }

    public virtual DbSet<PekkishFinanceAccount> PekkishFinanceAccounts { get; set; }

    public virtual DbSet<PekkishFinanceAccountCategory> PekkishFinanceAccountCategories { get; set; }

    public virtual DbSet<PekkishFinanceVatType> PekkishFinanceVatTypes { get; set; }

    public virtual DbSet<PekkishOrder> PekkishOrders { get; set; }

    public virtual DbSet<PekkishOrderDelivery> PekkishOrderDeliveries { get; set; }

    public virtual DbSet<PekkishOrderDetail> PekkishOrderDetails { get; set; }

    public virtual DbSet<PekkishOrderDetailOption> PekkishOrderDetailOptions { get; set; }

    public virtual DbSet<PekkishOrderHistory> PekkishOrderHistories { get; set; }

    public virtual DbSet<PekkishOrderPayMethod> PekkishOrderPayMethods { get; set; }

    public virtual DbSet<PekkishOrderStatus> PekkishOrderStatuses { get; set; }

    public virtual DbSet<PekkishPekkishUser> PekkishPekkishUsers { get; set; }

    public virtual DbSet<PekkishPekkishUserType> PekkishPekkishUserTypes { get; set; }

    public virtual DbSet<PekkishProduct> PekkishProducts { get; set; }

    public virtual DbSet<PekkishProductCategory> PekkishProductCategories { get; set; }

    public virtual DbSet<PekkishProductExtra> PekkishProductExtras { get; set; }

    public virtual DbSet<PekkishProductExtraLink> PekkishProductExtraLinks { get; set; }

    public virtual DbSet<PekkishProductExtraOption> PekkishProductExtraOptions { get; set; }

    public virtual DbSet<PekkishProductExtraOptionSub> PekkishProductExtraOptionSubs { get; set; }

    public virtual DbSet<PekkishReview> PekkishReviews { get; set; }

    public virtual DbSet<PekkishSupplier> PekkishSuppliers { get; set; }

    public virtual DbSet<PekkishVendor> PekkishVendors { get; set; }

    public virtual DbSet<PekkishVendorConfig> PekkishVendorConfigs { get; set; }

    public virtual DbSet<PekkishVendorDeliveryZone> PekkishVendorDeliveryZones { get; set; }

    public virtual DbSet<PekkishVendorOwner> PekkishVendorOwners { get; set; }

    public virtual DbSet<PekkishVendorSignup> PekkishVendorSignups { get; set; }

    public virtual DbSet<PekkishVendorType> PekkishVendorTypes { get; set; }

    public virtual DbSet<SaasEdition> SaasEditions { get; set; }

    public virtual DbSet<SaasTenant> SaasTenants { get; set; }

    public virtual DbSet<SaasTenantConnectionString> SaasTenantConnectionStrings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbpAuditLog>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.ExecutionTime }, "IX_AbpAuditLogs_TenantId_ExecutionTime");

            entity.HasIndex(e => new { e.TenantId, e.UserId, e.ExecutionTime }, "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApplicationName).HasMaxLength(96);
            entity.Property(e => e.BrowserInfo).HasMaxLength(512);
            entity.Property(e => e.ClientId).HasMaxLength(64);
            entity.Property(e => e.ClientIpAddress).HasMaxLength(64);
            entity.Property(e => e.ClientName).HasMaxLength(128);
            entity.Property(e => e.Comments).HasMaxLength(256);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.CorrelationId).HasMaxLength(64);
            entity.Property(e => e.HttpMethod).HasMaxLength(16);
            entity.Property(e => e.Url).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AbpAuditLogAction>(entity =>
        {
            entity.HasIndex(e => e.AuditLogId, "IX_AbpAuditLogActions_AuditLogId");

            entity.HasIndex(e => new { e.TenantId, e.ServiceName, e.MethodName, e.ExecutionTime }, "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_ExecutionTime");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MethodName).HasMaxLength(128);
            entity.Property(e => e.Parameters).HasMaxLength(2000);
            entity.Property(e => e.ServiceName).HasMaxLength(256);

            entity.HasOne(d => d.AuditLog).WithMany(p => p.AbpAuditLogActions).HasForeignKey(d => d.AuditLogId);
        });

        modelBuilder.Entity<AbpBackgroundJob>(entity =>
        {
            entity.HasIndex(e => new { e.IsAbandoned, e.NextTryTime }, "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.IsAbandoned)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.JobName).HasMaxLength(128);
            entity.Property(e => e.Priority).HasDefaultValueSql("(CONVERT([tinyint],(15)))");
            entity.Property(e => e.TryCount).HasDefaultValueSql("(CONVERT([smallint],(0)))");
        });

        modelBuilder.Entity<AbpBlob>(entity =>
        {
            entity.HasIndex(e => e.ContainerId, "IX_AbpBlobs_ContainerId");

            entity.HasIndex(e => new { e.TenantId, e.ContainerId, e.Name }, "IX_AbpBlobs_TenantId_ContainerId_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Container).WithMany(p => p.AbpBlobs).HasForeignKey(d => d.ContainerId);
        });

        modelBuilder.Entity<AbpBlobContainer>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.Name }, "IX_AbpBlobContainers_TenantId_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<AbpClaimType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Regex).HasMaxLength(512);
            entity.Property(e => e.RegexDescription).HasMaxLength(128);
        });

        modelBuilder.Entity<AbpEntityChange>(entity =>
        {
            entity.HasIndex(e => e.AuditLogId, "IX_AbpEntityChanges_AuditLogId");

            entity.HasIndex(e => new { e.TenantId, e.EntityTypeFullName, e.EntityId }, "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EntityId).HasMaxLength(128);
            entity.Property(e => e.EntityTypeFullName).HasMaxLength(128);

            entity.HasOne(d => d.AuditLog).WithMany(p => p.AbpEntityChanges).HasForeignKey(d => d.AuditLogId);
        });

        modelBuilder.Entity<AbpEntityPropertyChange>(entity =>
        {
            entity.HasIndex(e => e.EntityChangeId, "IX_AbpEntityPropertyChanges_EntityChangeId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NewValue).HasMaxLength(512);
            entity.Property(e => e.OriginalValue).HasMaxLength(512);
            entity.Property(e => e.PropertyName).HasMaxLength(128);
            entity.Property(e => e.PropertyTypeFullName).HasMaxLength(64);

            entity.HasOne(d => d.EntityChange).WithMany(p => p.AbpEntityPropertyChanges).HasForeignKey(d => d.EntityChangeId);
        });

        modelBuilder.Entity<AbpFeatureValue>(entity =>
        {
            entity.HasIndex(e => new { e.Name, e.ProviderName, e.ProviderKey }, "IX_AbpFeatureValues_Name_ProviderName_ProviderKey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(64);
            entity.Property(e => e.ProviderName).HasMaxLength(64);
            entity.Property(e => e.Value).HasMaxLength(128);
        });

        modelBuilder.Entity<AbpLanguage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.CultureName).HasMaxLength(10);
            entity.Property(e => e.DisplayName).HasMaxLength(32);
            entity.Property(e => e.FlagIcon).HasMaxLength(48);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.UiCultureName).HasMaxLength(10);
        });

        modelBuilder.Entity<AbpLanguageText>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.ResourceName, e.CultureName }, "IX_AbpLanguageTexts_TenantId_ResourceName_CultureName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CultureName).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(512);
            entity.Property(e => e.ResourceName).HasMaxLength(128);
        });

        modelBuilder.Entity<AbpLinkUser>(entity =>
        {
            entity.HasIndex(e => new { e.SourceUserId, e.SourceTenantId, e.TargetUserId, e.TargetTenantId }, "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId")
                .IsUnique()
                .HasFilter("([SourceTenantId] IS NOT NULL AND [TargetTenantId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<AbpOrganizationUnit>(entity =>
        {
            entity.HasIndex(e => e.Code, "IX_AbpOrganizationUnits_Code");

            entity.HasIndex(e => e.ParentId, "IX_AbpOrganizationUnits_ParentId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(95);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.DisplayName).HasMaxLength(128);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);
        });

        modelBuilder.Entity<AbpOrganizationUnitRole>(entity =>
        {
            entity.HasKey(e => new { e.OrganizationUnitId, e.RoleId });

            entity.HasIndex(e => new { e.RoleId, e.OrganizationUnitId }, "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId");

            entity.HasOne(d => d.OrganizationUnit).WithMany(p => p.AbpOrganizationUnitRoles).HasForeignKey(d => d.OrganizationUnitId);

            entity.HasOne(d => d.Role).WithMany(p => p.AbpOrganizationUnitRoles).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AbpPermissionGrant>(entity =>
        {
            entity.HasIndex(e => new { e.Name, e.ProviderName, e.ProviderKey }, "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(64);
            entity.Property(e => e.ProviderName).HasMaxLength(64);
        });

        modelBuilder.Entity<AbpRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_AbpRoles_NormalizedName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AbpRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AbpRoleClaims_RoleId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClaimType).HasMaxLength(256);
            entity.Property(e => e.ClaimValue).HasMaxLength(1024);

            entity.HasOne(d => d.Role).WithMany(p => p.AbpRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AbpSecurityLog>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.Action }, "IX_AbpSecurityLogs_TenantId_Action");

            entity.HasIndex(e => new { e.TenantId, e.ApplicationName }, "IX_AbpSecurityLogs_TenantId_ApplicationName");

            entity.HasIndex(e => new { e.TenantId, e.Identity }, "IX_AbpSecurityLogs_TenantId_Identity");

            entity.HasIndex(e => new { e.TenantId, e.UserId }, "IX_AbpSecurityLogs_TenantId_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Action).HasMaxLength(96);
            entity.Property(e => e.ApplicationName).HasMaxLength(96);
            entity.Property(e => e.BrowserInfo).HasMaxLength(512);
            entity.Property(e => e.ClientId).HasMaxLength(64);
            entity.Property(e => e.ClientIpAddress).HasMaxLength(64);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.CorrelationId).HasMaxLength(64);
            entity.Property(e => e.Identity).HasMaxLength(96);
            entity.Property(e => e.TenantName).HasMaxLength(64);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AbpSetting>(entity =>
        {
            entity.HasIndex(e => new { e.Name, e.ProviderName, e.ProviderKey }, "IX_AbpSettings_Name_ProviderName_ProviderKey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(64);
            entity.Property(e => e.ProviderName).HasMaxLength(64);
            entity.Property(e => e.Value).HasMaxLength(2048);
        });

        modelBuilder.Entity<AbpTextTemplateContent>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.CultureName).HasMaxLength(10);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<AbpUser>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_AbpUsers_Email");

            entity.HasIndex(e => e.NormalizedEmail, "IX_AbpUsers_NormalizedEmail");

            entity.HasIndex(e => e.NormalizedUserName, "IX_AbpUsers_NormalizedUserName");

            entity.HasIndex(e => e.UserName, "IX_AbpUsers_UserName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsExternal)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.LockoutEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(16);
            entity.Property(e => e.PhoneNumberConfirmed)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.SecurityStamp).HasMaxLength(256);
            entity.Property(e => e.Surname).HasMaxLength(64);
            entity.Property(e => e.TwoFactorEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AbpUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AbpUserClaims_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClaimType).HasMaxLength(256);
            entity.Property(e => e.ClaimValue).HasMaxLength(1024);

            entity.HasOne(d => d.User).WithMany(p => p.AbpUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AbpUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider });

            entity.HasIndex(e => new { e.LoginProvider, e.ProviderKey }, "IX_AbpUserLogins_LoginProvider_ProviderKey");

            entity.Property(e => e.LoginProvider).HasMaxLength(64);
            entity.Property(e => e.ProviderDisplayName).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(196);

            entity.HasOne(d => d.User).WithMany(p => p.AbpUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AbpUserOrganizationUnit>(entity =>
        {
            entity.HasKey(e => new { e.OrganizationUnitId, e.UserId });

            entity.HasIndex(e => new { e.UserId, e.OrganizationUnitId }, "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId");

            entity.HasOne(d => d.OrganizationUnit).WithMany(p => p.AbpUserOrganizationUnits).HasForeignKey(d => d.OrganizationUnitId);

            entity.HasOne(d => d.User).WithMany(p => p.AbpUserOrganizationUnits).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AbpUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => new { e.RoleId, e.UserId }, "IX_AbpUserRoles_RoleId_UserId");

            entity.HasOne(d => d.Role).WithMany(p => p.AbpUserRoles).HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany(p => p.AbpUserRoles).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AbpUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AbpUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AppBrand>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.EmailAddress).HasMaxLength(200);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameShort).HasMaxLength(14);
            entity.Property(e => e.PreperationTimeMins).HasDefaultValueSql("((20))");
        });

        modelBuilder.Entity<AppExpense>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(500);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatRate).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Supplier).WithMany(p => p.AppExpenses)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppExpenses_AppSuppliers");
        });

        modelBuilder.Entity<AppExpenseDetail>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPerc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Exclusive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatPerc).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.FinanceAccount).WithMany(p => p.AppExpenseDetails)
                .HasForeignKey(d => d.FinanceAccountId)
                .HasConstraintName("FK_AppExpenseDetails_AppFinanceAccounts");
        });

        modelBuilder.Entity<AppFinanceAccount>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.FinanceAccountCategory).WithMany(p => p.AppFinanceAccounts)
                .HasForeignKey(d => d.FinanceAccountCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppFinanceAccounts_AppFinanceAccountCategories");
        });

        modelBuilder.Entity<AppFinanceAccountCategory>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.FinanceAccountCategoryParent).WithMany(p => p.AppFinanceAccountCategories)
                .HasForeignKey(d => d.FinanceAccountCategoryParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppFinanceAccountCategories_AppFinanceAccountCategoryParents");
        });

        modelBuilder.Entity<AppFinanceAccountCategoryParent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppFinanceAccountCategoryParent");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppFinanceVatType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.VatRate).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<AppImportProductCategory>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<AppLocation>(entity =>
        {
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(200);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.PrinterName).HasMaxLength(50);
        });

        modelBuilder.Entity<AppLocationBrand>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppLocationBrands_BrandId");

            entity.HasIndex(e => e.LocationId, "IX_AppLocationBrands_LocationId");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PrinterName).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.AppLocationBrands).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.Location).WithMany(p => p.AppLocationBrands).HasForeignKey(d => d.LocationId);
        });

        modelBuilder.Entity<AppOrder>(entity =>
        {
            entity.HasIndex(e => e.LocationId, "IX_AppOrders_LocationId");

            entity.HasIndex(e => e.OrderFulfillmentId, "IX_AppOrders_OrderFulfillmentId");

            entity.HasIndex(e => e.OrderStatusId, "IX_AppOrders_OrderStatusId");

            entity.HasIndex(e => e.PaymentMethodId, "IX_AppOrders_PaymentMethodId");

            entity.HasIndex(e => e.SalesChannelId, "IX_AppOrders_SalesChannelId");

            entity.Property(e => e.Brand).HasMaxLength(500);
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DriverTip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExternalCustomerName).HasMaxLength(100);
            entity.Property(e => e.ExternalId).HasMaxLength(50);
            entity.Property(e => e.ExternalPayMethod).HasMaxLength(50);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.PaidCard).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidCash).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidChange).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidDate).HasColumnType("datetime");
            entity.Property(e => e.PaidEft)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PaidEFT");
            entity.Property(e => e.PaidOnline).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidOther).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidTip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PickupFailedReason).HasDefaultValueSql("(N'')");
            entity.Property(e => e.Refund).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RejectDate).HasColumnType("datetime");
            entity.Property(e => e.RejectReason).HasDefaultValueSql("(N'')");
            entity.Property(e => e.ServiceFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFeeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxRatePerc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Location).WithMany(p => p.AppOrders).HasForeignKey(d => d.LocationId);

            entity.HasOne(d => d.OrderFulfillment).WithMany(p => p.AppOrders).HasForeignKey(d => d.OrderFulfillmentId);

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.AppOrders).HasForeignKey(d => d.OrderStatusId);

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.AppOrders).HasForeignKey(d => d.PaymentMethodId);

            entity.HasOne(d => d.SalesChannel).WithMany(p => p.AppOrders).HasForeignKey(d => d.SalesChannelId);
        });

        modelBuilder.Entity<AppOrderBrand>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.AppOrderBrands)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_AppOrderBrands_AppOrders");
        });

        modelBuilder.Entity<AppOrderDetail>(entity =>
        {
            entity.HasIndex(e => e.ProductId, "IX_AppOrderDetails_ProductId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBaseNormal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountNoDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountNormal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.OrderRequestCrono).HasDefaultValueSql("((1))");
            entity.Property(e => e.RateIncrease).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.AppOrderDetails)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("FK_AppOrderDetails_AppOrders");

            entity.HasOne(d => d.Product).WithMany(p => p.AppOrderDetails).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<AppOrderDetailOption>(entity =>
        {
            entity.HasIndex(e => e.ProductExtraId, "IX_AppOrderDetailOptions_ProductExtraId");

            entity.HasIndex(e => e.ProductExtraOptionId, "IX_AppOrderDetailOptions_ProductExtraOptionId");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.AppOrderDetailOptions)
                .HasForeignKey(d => d.OrderDetailId)
                .HasConstraintName("FK_AppOrderDetailOptions_AppOrderDetails");

            entity.HasOne(d => d.ProductExtra).WithMany(p => p.AppOrderDetailOptions).HasForeignKey(d => d.ProductExtraId);

            entity.HasOne(d => d.ProductExtraOption).WithMany(p => p.AppOrderDetailOptions).HasForeignKey(d => d.ProductExtraOptionId);
        });

        modelBuilder.Entity<AppOrderFulfillment>(entity =>
        {
            entity.Property(e => e.HexCodeBack)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HexCodeFront)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppOrderKitchenScreen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderKitchenScreen");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppOrderPrintQueue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppOrderPrintQueue");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PrintedDate).HasColumnType("datetime");
            entity.Property(e => e.PrinterName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(1);
        });

        modelBuilder.Entity<AppOrderRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderRequest");

            entity.Property(e => e.Brand)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Iteration).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.PickupReadyDate).HasColumnType("datetime");
            entity.Property(e => e.RejectDate).HasColumnType("datetime");
            entity.Property(e => e.RejectReason).IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.AppOrderRequests)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_AppOrderRequest_AppOrders");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.AppOrderRequests)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppOrderRequest_AppOrderStatuses");
        });

        modelBuilder.Entity<AppOrderRequestBrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppOrderRequestBrand");

            entity.Property(e => e.PickupReadyDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderRequest).WithMany(p => p.AppOrderRequestBrands)
                .HasForeignKey(d => d.OrderRequestId)
                .HasConstraintName("FK_AppOrderRequestBrands_AppOrderRequests");
        });

        modelBuilder.Entity<AppOrderStatus>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppPaymentMethod>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.HexCodeBack)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HexCodeFront)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsExternal)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppProduct>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppProducts_BrandId");

            entity.HasIndex(e => e.ProductCategoryId, "IX_AppProducts_ProductCategoryId");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Brand).WithMany(p => p.AppProducts).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.AppProducts)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppProductCategory>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppProductCategories_BrandId");

            entity.Property(e => e.CostPercentageDefault).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExternalAppName).HasMaxLength(50);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppProductExtra>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppProductExtras_BrandId");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Brand).WithMany(p => p.AppProductExtras).HasForeignKey(d => d.BrandId);
        });

        modelBuilder.Entity<AppProductExtraLink>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppProductExtraLinks_BrandId");

            entity.HasIndex(e => e.ProductExtraId, "IX_AppProductExtraLinks_ProductExtraId");

            entity.HasIndex(e => e.ProductId, "IX_AppProductExtraLinks_ProductId");

            entity.HasOne(d => d.Brand).WithMany(p => p.AppProductExtraLinks).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.ProductExtra).WithMany(p => p.AppProductExtraLinks).HasForeignKey(d => d.ProductExtraId);

            entity.HasOne(d => d.Product).WithMany(p => p.AppProductExtraLinks).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<AppProductExtraOption>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppProductExtraOptions_BrandId");

            entity.HasIndex(e => e.ProductExtraId, "IX_AppProductExtraOptions_ProductExtraId");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Brand).WithMany(p => p.AppProductExtraOptions).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.ProductExtra).WithMany(p => p.AppProductExtraOptions)
                .HasForeignKey(d => d.ProductExtraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppProductRecipe>(entity =>
        {
            entity.Property(e => e.AdditionalInfo).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(100);

            entity.HasOne(d => d.Product).WithMany(p => p.AppProductRecipes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_AppProductRecipes_AppProducts");

            entity.HasOne(d => d.Stock).WithMany(p => p.AppProductRecipes)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppProductRecipes_AppStocks");
        });

        modelBuilder.Entity<AppRateSheet>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppRateSheets_BrandId");

            entity.HasIndex(e => e.SalesChannelId, "IX_AppRateSheets_SalesChannelId");

            entity.Property(e => e.IncreasePercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Brand).WithMany(p => p.AppRateSheets).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.SalesChannel).WithMany(p => p.AppRateSheets).HasForeignKey(d => d.SalesChannelId);
        });

        modelBuilder.Entity<AppSalesChannel>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.HexColourCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HexColourCodeText)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsExternalPayment)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsMultiBrand)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsShowNewOrder)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppSeatingArea>(entity =>
        {
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppStock>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.StockCategory).WithMany(p => p.AppStocks)
                .HasForeignKey(d => d.StockCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStocks_AppStockCategories");

            entity.HasOne(d => d.StockUnit).WithMany(p => p.AppStocks)
                .HasForeignKey(d => d.StockUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStocks_AppStockUnits");
        });

        modelBuilder.Entity<AppStockCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppStockLocation>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Location).WithMany(p => p.AppStockLocations)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_AppStockLocations_AppLocations");

            entity.HasOne(d => d.Stock).WithMany(p => p.AppStockLocations)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStockLocations_AppStocks");
        });

        modelBuilder.Entity<AppStockLocationHistory>(entity =>
        {
            entity.Property(e => e.Reason).HasMaxLength(100);
        });

        modelBuilder.Entity<AppStockLocationHistoryReason>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppStockPurchaseDetail>(entity =>
        {
            entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPerc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Exclusive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockUnit).HasMaxLength(100);
            entity.Property(e => e.StockUnitCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatPerc).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Stock).WithMany(p => p.AppStockPurchaseDetails)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStockPurchaseDetails_AppStocks");

            entity.HasOne(d => d.StockPurchaseHeader).WithMany(p => p.AppStockPurchaseDetails)
                .HasForeignKey(d => d.StockPurchaseHeaderId)
                .HasConstraintName("FK_AppStockPurchaseDetails_AppStockPurchaseHeaders");
        });

        modelBuilder.Entity<AppStockPurchaseHeader>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatRate).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Location).WithMany(p => p.AppStockPurchaseHeaders)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStockPurchaseHeaders_AppLocations");

            entity.HasOne(d => d.Supplier).WithMany(p => p.AppStockPurchaseHeaders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStockPurchaseHeaders_AppStockPurchaseHeaders");
        });

        modelBuilder.Entity<AppStockUnit>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppSupplier>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.Suburb).HasMaxLength(100);
        });

        modelBuilder.Entity<AppTenantInfo>(entity =>
        {
            entity.Property(e => e.IsActiveWhatsApp)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NameShort).HasMaxLength(14);
            entity.Property(e => e.StoreNotice).HasMaxLength(1024);
            entity.Property(e => e.WelcomeMessage).HasMaxLength(1024);
        });

        modelBuilder.Entity<AppUserBrand>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_AppUserBrands_BrandId");

            entity.Property(e => e.CreationTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.HasOne(d => d.Brand).WithMany(p => p.AppUserBrands).HasForeignKey(d => d.BrandId);
        });

        modelBuilder.Entity<AppUserConfig>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.HomeView).HasMaxLength(1);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AppUserLocation>(entity =>
        {
            entity.HasIndex(e => e.LocationId, "IX_AppUserLocations_LocationId");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.PrinterName).HasMaxLength(50);

            entity.HasOne(d => d.Location).WithMany(p => p.AppUserLocations).HasForeignKey(d => d.LocationId);
        });

        modelBuilder.Entity<AppWatiConversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppWatiConversations_1");

            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.ConversationId).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.WaId).HasMaxLength(50);

            entity.HasOne(d => d.WatiConversationStatus).WithMany(p => p.AppWatiConversations)
                .HasForeignKey(d => d.WatiConversationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppWatiConversations_AppWatiConversationStatuses");

            entity.HasOne(d => d.WatiConversationType).WithMany(p => p.AppWatiConversations)
                .HasForeignKey(d => d.WatiConversationTypeId)
                .HasConstraintName("FK_AppWatiConversations_AppWatiConversationTypes");
        });

        modelBuilder.Entity<AppWatiConversationStatus>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppWatiConversationType>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppWatiMessage>(entity =>
        {
            entity.Property(e => e.AssignedId).HasMaxLength(100);
            entity.Property(e => e.ConversationId).HasMaxLength(100);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.ListReplyId).HasMaxLength(50);
            entity.Property(e => e.ListReplyTitle).HasMaxLength(100);
            entity.Property(e => e.MessageContact).HasMaxLength(100);
            entity.Property(e => e.OperatorEmail).HasMaxLength(100);
            entity.Property(e => e.OperatorName).HasMaxLength(100);
            entity.Property(e => e.ReplyContextId).HasMaxLength(100);
            entity.Property(e => e.SenderName).HasMaxLength(100);
            entity.Property(e => e.StatusString).HasMaxLength(100);
            entity.Property(e => e.TicketId).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.WaId).HasMaxLength(50);
            entity.Property(e => e.WatiId).HasMaxLength(100);
        });

        modelBuilder.Entity<AppWatiOrder>(entity =>
        {
            entity.Property(e => e.AcceptedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RejectDate).HasColumnType("datetime");
            entity.Property(e => e.RejectReason).HasMaxLength(100);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Location).WithMany(p => p.AppWatiOrders)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_AppWatiOrders_AppLocations");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.AppWatiOrders)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_AppWatiOrders_AppPaymentMethods");

            entity.HasOne(d => d.PosOrder).WithMany(p => p.AppWatiOrders)
                .HasForeignKey(d => d.PosOrderId)
                .HasConstraintName("FK_AppWatiOrders_AppOrders");

            entity.HasOne(d => d.TenantInfo).WithMany(p => p.AppWatiOrders)
                .HasForeignKey(d => d.TenantInfoId)
                .HasConstraintName("FK_AppWatiOrders_AppTenantInfos");

            entity.HasOne(d => d.WatiConversation).WithMany(p => p.AppWatiOrders)
                .HasForeignKey(d => d.WatiConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppWatiOrders_AppWatiConversations");
        });

        modelBuilder.Entity<AppWatiOrderDetail>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppWatiOrderStatus>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<HubspotDriverEnquiry>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HubspotDriverEnquiry");

            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EnquiryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Experience)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Suburb)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HubspotVendorTerm>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressPostCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressStreet)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AddressSuburb)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactSurname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cuisine)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Dietary)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fullfillment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Instagram)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<IdentityServerApiResource>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AllowedAccessTokenSigningAlgorithms).HasMaxLength(100);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<IdentityServerApiResourceClaim>(entity =>
        {
            entity.HasKey(e => new { e.ApiResourceId, e.Type });

            entity.Property(e => e.Type).HasMaxLength(200);

            entity.HasOne(d => d.ApiResource).WithMany(p => p.IdentityServerApiResourceClaims).HasForeignKey(d => d.ApiResourceId);
        });

        modelBuilder.Entity<IdentityServerApiResourceProperty>(entity =>
        {
            entity.HasKey(e => new { e.ApiResourceId, e.Key, e.Value });

            entity.Property(e => e.Key).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(2000);

            entity.HasOne(d => d.ApiResource).WithMany(p => p.IdentityServerApiResourceProperties).HasForeignKey(d => d.ApiResourceId);
        });

        modelBuilder.Entity<IdentityServerApiResourceScope>(entity =>
        {
            entity.HasKey(e => new { e.ApiResourceId, e.Scope });

            entity.Property(e => e.Scope).HasMaxLength(200);

            entity.HasOne(d => d.ApiResource).WithMany(p => p.IdentityServerApiResourceScopes).HasForeignKey(d => d.ApiResourceId);
        });

        modelBuilder.Entity<IdentityServerApiResourceSecret>(entity =>
        {
            entity.HasKey(e => new { e.ApiResourceId, e.Type, e.Value });

            entity.Property(e => e.Type).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(4000);
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(d => d.ApiResource).WithMany(p => p.IdentityServerApiResourceSecrets).HasForeignKey(d => d.ApiResourceId);
        });

        modelBuilder.Entity<IdentityServerApiScope>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<IdentityServerApiScopeClaim>(entity =>
        {
            entity.HasKey(e => new { e.ApiScopeId, e.Type });

            entity.Property(e => e.Type).HasMaxLength(200);

            entity.HasOne(d => d.ApiScope).WithMany(p => p.IdentityServerApiScopeClaims).HasForeignKey(d => d.ApiScopeId);
        });

        modelBuilder.Entity<IdentityServerApiScopeProperty>(entity =>
        {
            entity.HasKey(e => new { e.ApiScopeId, e.Key, e.Value });

            entity.Property(e => e.Key).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(2000);

            entity.HasOne(d => d.ApiScope).WithMany(p => p.IdentityServerApiScopeProperties).HasForeignKey(d => d.ApiScopeId);
        });

        modelBuilder.Entity<IdentityServerClient>(entity =>
        {
            entity.HasIndex(e => e.ClientId, "IX_IdentityServerClients_ClientId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(100);
            entity.Property(e => e.BackChannelLogoutUri).HasMaxLength(2000);
            entity.Property(e => e.ClientClaimsPrefix).HasMaxLength(200);
            entity.Property(e => e.ClientId).HasMaxLength(200);
            entity.Property(e => e.ClientName).HasMaxLength(200);
            entity.Property(e => e.ClientUri).HasMaxLength(2000);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.FrontChannelLogoutUri).HasMaxLength(2000);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.LogoUri).HasMaxLength(2000);
            entity.Property(e => e.PairWiseSubjectSalt).HasMaxLength(200);
            entity.Property(e => e.ProtocolType).HasMaxLength(200);
            entity.Property(e => e.UserCodeType).HasMaxLength(100);
        });

        modelBuilder.Entity<IdentityServerClientClaim>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Type, e.Value });

            entity.Property(e => e.Type).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(250);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientClaims).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientCorsOrigin>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Origin });

            entity.Property(e => e.Origin).HasMaxLength(150);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientCorsOrigins).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientGrantType>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.GrantType });

            entity.Property(e => e.GrantType).HasMaxLength(250);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientGrantTypes).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientIdPrestriction>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Provider });

            entity.ToTable("IdentityServerClientIdPRestrictions");

            entity.Property(e => e.Provider).HasMaxLength(200);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientIdPrestrictions).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientPostLogoutRedirectUri>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.PostLogoutRedirectUri });

            entity.Property(e => e.PostLogoutRedirectUri).HasMaxLength(2000);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientPostLogoutRedirectUris).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientProperty>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Key, e.Value });

            entity.Property(e => e.Key).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(2000);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientProperties).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientRedirectUri>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.RedirectUri });

            entity.Property(e => e.RedirectUri).HasMaxLength(2000);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientRedirectUris).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientScope>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Scope });

            entity.Property(e => e.Scope).HasMaxLength(200);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientScopes).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerClientSecret>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.Type, e.Value });

            entity.Property(e => e.Type).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(4000);
            entity.Property(e => e.Description).HasMaxLength(2000);

            entity.HasOne(d => d.Client).WithMany(p => p.IdentityServerClientSecrets).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<IdentityServerDeviceFlowCode>(entity =>
        {
            entity.HasIndex(e => e.DeviceCode, "IX_IdentityServerDeviceFlowCodes_DeviceCode").IsUnique();

            entity.HasIndex(e => e.Expiration, "IX_IdentityServerDeviceFlowCodes_Expiration");

            entity.HasIndex(e => e.UserCode, "IX_IdentityServerDeviceFlowCodes_UserCode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClientId).HasMaxLength(200);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DeviceCode).HasMaxLength(200);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.SubjectId).HasMaxLength(200);
            entity.Property(e => e.UserCode).HasMaxLength(200);
        });

        modelBuilder.Entity<IdentityServerIdentityResource>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<IdentityServerIdentityResourceClaim>(entity =>
        {
            entity.HasKey(e => new { e.IdentityResourceId, e.Type });

            entity.Property(e => e.Type).HasMaxLength(200);

            entity.HasOne(d => d.IdentityResource).WithMany(p => p.IdentityServerIdentityResourceClaims).HasForeignKey(d => d.IdentityResourceId);
        });

        modelBuilder.Entity<IdentityServerIdentityResourceProperty>(entity =>
        {
            entity.HasKey(e => new { e.IdentityResourceId, e.Key, e.Value });

            entity.Property(e => e.Key).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(2000);

            entity.HasOne(d => d.IdentityResource).WithMany(p => p.IdentityServerIdentityResourceProperties).HasForeignKey(d => d.IdentityResourceId);
        });

        modelBuilder.Entity<IdentityServerPersistedGrant>(entity =>
        {
            entity.HasKey(e => e.Key);

            entity.HasIndex(e => e.Expiration, "IX_IdentityServerPersistedGrants_Expiration");

            entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type }, "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type");

            entity.HasIndex(e => new { e.SubjectId, e.SessionId, e.Type }, "IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type");

            entity.Property(e => e.Key).HasMaxLength(200);
            entity.Property(e => e.ClientId).HasMaxLength(200);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.SubjectId).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<PayGatewayPlan>(entity =>
        {
            entity.HasKey(e => new { e.PlanId, e.Gateway });

            entity.HasOne(d => d.Plan).WithMany(p => p.PayGatewayPlans).HasForeignKey(d => d.PlanId);
        });

        modelBuilder.Entity<PayPaymentRequest>(entity =>
        {
            entity.HasIndex(e => e.CreationTime, "IX_PayPaymentRequests_CreationTime");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
        });

        modelBuilder.Entity<PayPaymentRequestProduct>(entity =>
        {
            entity.HasKey(e => new { e.PaymentRequestId, e.Code });

            entity.HasOne(d => d.PaymentRequest).WithMany(p => p.PayPaymentRequestProducts).HasForeignKey(d => d.PaymentRequestId);
        });

        modelBuilder.Entity<PayPlan>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<PekkishCustomer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CellphoneNumber).HasMaxLength(50);
            entity.Property(e => e.CountryCode).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.SocialId).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishDeliveryGroup>(entity =>
        {
            entity.Property(e => e.ApiKey)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishDeliveryManager>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishDriver>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CellphoneNumber).HasMaxLength(50);
            entity.Property(e => e.CountryCode).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.SocialId).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishDriverGroup>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishDriverSignup>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(100);
            entity.Property(e => e.Suburb).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishExpense>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(1);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatRate).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PekkishExpenseDetail>(entity =>
        {
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPerc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Exclusive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VatPerc).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PekkishFinanceAccount>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishFinanceAccountCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishFinanceVatType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.VatRate).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PekkishOrder>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AppId).HasMaxLength(50);
            entity.Property(e => e.CustomerEmail).HasMaxLength(200);
            entity.Property(e => e.CustomerLastName).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeliveryTime).HasMaxLength(20);
            entity.Property(e => e.DeliveryType).HasMaxLength(50);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountType).HasMaxLength(50);
            entity.Property(e => e.DriverLastName).HasMaxLength(50);
            entity.Property(e => e.DriverName).HasMaxLength(50);
            entity.Property(e => e.DriverTip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LogisticsFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LogisticsRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayData).HasMaxLength(50);
            entity.Property(e => e.PayMethod).HasMaxLength(50);
            entity.Property(e => e.Refund).HasMaxLength(50);
            entity.Property(e => e.RefundData).HasMaxLength(50);
            entity.Property(e => e.ServiceFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFeeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxRatePerc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VendorName).HasMaxLength(500);
        });

        modelBuilder.Entity<PekkishOrderDelivery>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishOrderDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Order).WithMany(p => p.PekkishOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_PekkishOrderDetails_PekkishOrders");
        });

        modelBuilder.Entity<PekkishOrderDetailOption>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ProductExtraOptionId, e.ProductExtraOptionSubId });

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.PekkishOrderDetailOptions)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_PekkishOrderDetailOptions_PekkishOrderDetails");
        });

        modelBuilder.Entity<PekkishOrderHistory>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrderId, e.RowId }).HasName("PK_PekkishOrderHistories_1");

            entity.Property(e => e.Attribute).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.PekkishOrderHistories)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_PekkishOrderHistories_PekkishOrders");
        });

        modelBuilder.Entity<PekkishOrderPayMethod>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishOrderStatus>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishPekkishUser>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CellphoneNumber).HasMaxLength(50);
            entity.Property(e => e.CountryCode).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.SocialId).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishPekkishUserType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishProduct>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.PekkishProducts)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PekkishProducts_PekkishProductCategories");
        });

        modelBuilder.Entity<PekkishProductCategory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<PekkishProductExtra>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<PekkishProductExtraOption>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.ProductExtra).WithMany(p => p.PekkishProductExtraOptions)
                .HasForeignKey(d => d.ProductExtraId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PekkishProductExtraOptions_PekkishProductExtras");
        });

        modelBuilder.Entity<PekkishProductExtraOptionSub>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.HalfPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProductExtraOption).WithMany(p => p.PekkishProductExtraOptionSubs)
                .HasForeignKey(d => d.ProductExtraOptionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PekkishProductExtraOptionSubs_PekkishProductExtraOptions");
        });

        modelBuilder.Entity<PekkishReview>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PekkishSupplier>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PostCode).HasMaxLength(20);
            entity.Property(e => e.Suburb).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishVendor>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cellphone).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(50);
            entity.Property(e => e.Slug).HasMaxLength(50);
            entity.Property(e => e.Timezone).HasMaxLength(50);
        });

        modelBuilder.Entity<PekkishVendorConfig>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmailAddress).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NameShort)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PekkishVendorDeliveryZone>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Minimum).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PekkishVendorOwner>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PekkishVendorSignup>(entity =>
        {
            entity.Property(e => e.AddressCity).HasMaxLength(50);
            entity.Property(e => e.AddressPostCode).HasMaxLength(100);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactName).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.ContactSurname).HasMaxLength(50);
            entity.Property(e => e.Cuisine).HasMaxLength(50);
            entity.Property(e => e.Dietary).HasMaxLength(50);
            entity.Property(e => e.Fullfillment).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<PekkishVendorType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SaasEdition>(entity =>
        {
            entity.HasIndex(e => e.DisplayName, "IX_SaasEditions_DisplayName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.DisplayName).HasMaxLength(128);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
        });

        modelBuilder.Entity<SaasTenant>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_SaasTenants_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(40);
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(64);
        });

        modelBuilder.Entity<SaasTenantConnectionString>(entity =>
        {
            entity.HasKey(e => new { e.TenantId, e.Name });

            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.Value).HasMaxLength(1024);

            entity.HasOne(d => d.Tenant).WithMany(p => p.SaasTenantConnectionStrings).HasForeignKey(d => d.TenantId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClient
{
    public Guid Id { get; set; }

    public string ClientId { get; set; } = null!;

    public string? ClientName { get; set; }

    public string? Description { get; set; }

    public string? ClientUri { get; set; }

    public string? LogoUri { get; set; }

    public bool Enabled { get; set; }

    public string ProtocolType { get; set; } = null!;

    public bool RequireClientSecret { get; set; }

    public bool RequireConsent { get; set; }

    public bool AllowRememberConsent { get; set; }

    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    public bool RequirePkce { get; set; }

    public bool AllowPlainTextPkce { get; set; }

    public bool RequireRequestObject { get; set; }

    public bool AllowAccessTokensViaBrowser { get; set; }

    public string? FrontChannelLogoutUri { get; set; }

    public bool FrontChannelLogoutSessionRequired { get; set; }

    public string? BackChannelLogoutUri { get; set; }

    public bool BackChannelLogoutSessionRequired { get; set; }

    public bool AllowOfflineAccess { get; set; }

    public int IdentityTokenLifetime { get; set; }

    public string? AllowedIdentityTokenSigningAlgorithms { get; set; }

    public int AccessTokenLifetime { get; set; }

    public int AuthorizationCodeLifetime { get; set; }

    public int? ConsentLifetime { get; set; }

    public int AbsoluteRefreshTokenLifetime { get; set; }

    public int SlidingRefreshTokenLifetime { get; set; }

    public int RefreshTokenUsage { get; set; }

    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    public int RefreshTokenExpiration { get; set; }

    public int AccessTokenType { get; set; }

    public bool EnableLocalLogin { get; set; }

    public bool IncludeJwtId { get; set; }

    public bool AlwaysSendClientClaims { get; set; }

    public string? ClientClaimsPrefix { get; set; }

    public string? PairWiseSubjectSalt { get; set; }

    public int? UserSsoLifetime { get; set; }

    public string? UserCodeType { get; set; }

    public int DeviceCodeLifetime { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public virtual ICollection<IdentityServerClientClaim> IdentityServerClientClaims { get; } = new List<IdentityServerClientClaim>();

    public virtual ICollection<IdentityServerClientCorsOrigin> IdentityServerClientCorsOrigins { get; } = new List<IdentityServerClientCorsOrigin>();

    public virtual ICollection<IdentityServerClientGrantType> IdentityServerClientGrantTypes { get; } = new List<IdentityServerClientGrantType>();

    public virtual ICollection<IdentityServerClientIdPrestriction> IdentityServerClientIdPrestrictions { get; } = new List<IdentityServerClientIdPrestriction>();

    public virtual ICollection<IdentityServerClientPostLogoutRedirectUri> IdentityServerClientPostLogoutRedirectUris { get; } = new List<IdentityServerClientPostLogoutRedirectUri>();

    public virtual ICollection<IdentityServerClientProperty> IdentityServerClientProperties { get; } = new List<IdentityServerClientProperty>();

    public virtual ICollection<IdentityServerClientRedirectUri> IdentityServerClientRedirectUris { get; } = new List<IdentityServerClientRedirectUri>();

    public virtual ICollection<IdentityServerClientScope> IdentityServerClientScopes { get; } = new List<IdentityServerClientScope>();

    public virtual ICollection<IdentityServerClientSecret> IdentityServerClientSecrets { get; } = new List<IdentityServerClientSecret>();
}

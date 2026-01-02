# Identity Service Implementation with OpenIddict

## 1. Package Installation

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.1
dotnet add package OpenIddict.AspNetCore --version 7.2.0
dotnet add package OpenIddict.EntityFrameworkCore --version 7.2.0
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.0
```

## 2. Application User Entity

```csharp
// IdentityService.Domain/Entities/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserType UserType { get; set; }
    public AuthProvider AuthProvider { get; set; } = AuthProvider.Local;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
}

public enum UserType
{
    Customer,
    Partner,
    Driver,
    Admin
}

public enum AuthProvider
{
    Local,
    Google,
    Apple,
    Facebook
}
```

## 3. DbContext Configuration

```csharp
// IdentityService.Infrastructure/Data/IdentityDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure OpenIddict entities
        builder.UseOpenIddict<Guid>();

        // Custom configurations
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.UserType).HasConversion<string>();
            entity.Property(u => u.AuthProvider).HasConversion<string>();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
```

## 4. Program.cs Configuration

```csharp
// IdentityService.API/Program.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseOpenIddict<Guid>();
});

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

// OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<IdentityDbContext>()
               .ReplaceDefaultEntities<Guid>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token")
               .SetAuthorizationEndpointUris("/connect/authorize")
               .SetUserinfoEndpointUris("/connect/userinfo")
               .SetIntrospectionEndpointUris("/connect/introspect")
               .SetRevocationEndpointUris("/connect/revoke");

        options.AllowPasswordFlow()
               .AllowRefreshTokenFlow()
               .AllowClientCredentialsFlow()
               .AllowAuthorizationCodeFlow();

        options.RegisterScopes(
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles,
            "api"
        );

        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30))
               .SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictConstants.Schemes.Bearer;
    options.DefaultChallengeScheme = OpenIddictConstants.Schemes.Bearer;
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## 5. Authorization Controller

```csharp
// IdentityService.API/Controllers/AuthorizationController.cs
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthorizationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() 
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByEmailAsync(request.Username!);
            if (user == null || !user.IsActive)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid credentials."
                    }));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid credentials."
                    }));
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var identity = await CreateClaimsIdentityAsync(user);
            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsRefreshTokenGrantType())
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var userId = result.Principal?.GetClaim(Claims.Subject);
            
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null || !user.IsActive)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Token is no longer valid."
                    }));
            }

            var identity = await CreateClaimsIdentityAsync(user);
            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("The specified grant type is not supported.");
    }

    private async Task<ClaimsIdentity> CreateClaimsIdentityAsync(ApplicationUser user)
    {
        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.SetClaim(Claims.Subject, user.Id.ToString())
                .SetClaim(Claims.Email, user.Email)
                .SetClaim(Claims.Name, $"{user.FirstName} {user.LastName}".Trim())
                .SetClaim("user_type", user.UserType.ToString());

        var roles = await _userManager.GetRolesAsync(user);
        identity.SetClaims(Claims.Role, roles.ToImmutableArray());

        identity.SetScopes(new[]
        {
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Profile,
            Scopes.Roles,
            Scopes.OfflineAccess,
            "api"
        });

        identity.SetDestinations(claim => claim.Type switch
        {
            Claims.Name or Claims.Email => new[] { Destinations.AccessToken, Destinations.IdentityToken },
            Claims.Role => new[] { Destinations.AccessToken, Destinations.IdentityToken },
            "user_type" => new[] { Destinations.AccessToken },
            _ => new[] { Destinations.AccessToken }
        });

        return identity;
    }
}
```

## 6. Auth Controller (Registration & Account Management)

```csharp
// IdentityService.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public AuthController(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            UserType = request.UserType
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        await _userManager.AddToRoleAsync(user, request.UserType.ToString());

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendEmailConfirmationAsync(user.Email!, token);

        return Ok(new { Message = "Registration successful. Please verify your email." });
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Ok(); // Don't reveal user existence

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendPasswordResetAsync(user.Email!, token);

        return Ok(new { Message = "Password reset email sent." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return BadRequest();

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserType = user.UserType,
            PhoneNumber = user.PhoneNumber
        });
    }
}
```

## 7. DTOs
// IdentityService.Application/DTOs/RegisterRequest.cs
public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public UserType UserType { get; set; } = UserType.Customer;
}

// IdentityService.Application/DTOs/VerifyEmailRequest.cs
public class VerifyEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

// IdentityService.Application/DTOs/ForgotPasswordRequest.cs
public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

// IdentityService.Application/DTOs/ResetPasswordRequest.cs
public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

// IdentityService.Application/DTOs/ChangePasswordRequest.cs
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

// IdentityService.Application/DTOs/UserDto.cs
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserType UserType { get; set; }
    public string? PhoneNumber { get; set; }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Template.Domain.Identity;
using Template.Infrastructure.Data.Seed.Configuration;

namespace Template.Infrastructure.Data.Seed
{
    public class DbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DefaultUsersAndRolesOptions _seedOptions;
        private readonly ILogger<DbSeeder> _logger;

        public DbSeeder(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<DefaultUsersAndRolesOptions> seedOptions,
            ILogger<DbSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _seedOptions = seedOptions.Value;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                // Seed roles
                await SeedRolesAsync(_seedOptions.Roles);

                // Seed users
                await SeedUsersAsync(_seedOptions.Users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private async Task SeedRolesAsync(List<string> roles)
        {
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Role '{RoleName}' created successfully", roleName);
                    }
                    else
                    {
                        _logger.LogError("Failed to create role '{RoleName}': {Errors}",
                            roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogInformation("Role '{RoleName}' already exists", roleName);
                }
            }
        }

        private async Task SeedUsersAsync(List<UserSeedOptions> users)
        {
            foreach (var userSeedOptions in users)
            {
                var existingUser = await _userManager.FindByEmailAsync(userSeedOptions.Email);
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userSeedOptions.Email,
                        Email = userSeedOptions.Email,
                        FullName = userSeedOptions.FullName,
                        EmailConfirmed = true,
                        IsActive = true
                    };

                    var result = await _userManager.CreateAsync(user, userSeedOptions.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User '{Email}' created successfully", userSeedOptions.Email);

                        // Assign roles to the user
                        foreach (var roleName in userSeedOptions.Roles)
                        {
                            if (await _roleManager.RoleExistsAsync(roleName))
                            {
                                await _userManager.AddToRoleAsync(user, roleName);
                                _logger.LogInformation("Role '{RoleName}' assigned to user '{Email}'", roleName, userSeedOptions.Email);
                            }
                            else
                            {
                                _logger.LogWarning("Role '{RoleName}' does not exist, cannot assign to user '{Email}'", roleName, userSeedOptions.Email);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to create user '{Email}': {Errors}",
                            userSeedOptions.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogInformation("User '{Email}' already exists", userSeedOptions.Email);
                }
            }
        }
    }
}

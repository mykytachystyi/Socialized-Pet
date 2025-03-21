namespace Domain.Enums;

public static class IdentityRoleConverter
{
    public const string General = "General";
    public const string DefaultAdmin = "DefaultAdmin";
    public const string DefaultUser = "DefaultUser";
    public const string NonActivate = "NonActivateUser";

    public static string CovertToRoleName(int role)
    {
        return role switch
        {
            0 => General,
            1 => DefaultAdmin,
            2 => DefaultUser,
            3 => NonActivate,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
    public static string ConvertToRoleName(IdentityRole role)
    {
        return role switch
        {
            IdentityRole.General => General,
            IdentityRole.DefaultAdmin => DefaultAdmin,
            IdentityRole.DefaultUser => DefaultUser,
            IdentityRole.NonActivateUser => NonActivate,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
    public static IdentityRole ConvertToRole(string roleName)
    {
        return roleName switch
        {
            General => IdentityRole.General,
            DefaultAdmin => IdentityRole.DefaultAdmin,
            DefaultUser => IdentityRole.DefaultUser,
            NonActivate => IdentityRole.NonActivateUser,
            _ => throw new ArgumentOutOfRangeException(nameof(roleName), roleName, null)
        };
    }
}

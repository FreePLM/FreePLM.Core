// Cloned From TrueNorthPLM, Free License 08-02-2025 //

using System.Runtime.InteropServices;

namespace FreePLM.Core.Helpers.SecureString
{
    public static class SecureStringExtensions
    {
        public static string? ConvertToUnsecureString(System.Security.SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            nint unmanagedString = nint.Zero;
            try
            {
                // Marshal SecureString to unmanaged memory
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                // Convert unmanaged memory to a .NET string
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Ensure that the unmanaged memory is freed
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}

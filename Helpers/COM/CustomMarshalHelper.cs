// Cloned From TrueNorthPLM, Free License 08-02-2025 //

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace FreePLM.Core.Helpers.COM
{
    // A helper class for working with COM objects and performing certain operations 
    // related to COM object retrieval using the OLE32 and OLEAUT32 libraries.
    public static class CustomMarshalHelper
    {
        // Constants for the OLE32 and OLEAUT32 library names
        internal const string OLEAUT32 = "oleaut32.dll";
        internal const string OLE32 = "ole32.dll";

        // A method that retrieves the active COM object associated with a given ProgID
        [SecurityCritical]  // Denotes that this method performs security-critical operations
        public static object GetActiveObject(string progID)
        {
            object? obj = null;
            Guid clsid;

            // Try to retrieve the CLSID (Class Identifier) of the COM object
            // using CLSIDFromProgIDEx, and if it fails, fall back on CLSIDFromProgID
            try
            {
                // Attempt to get CLSID from the ProgID using CLSIDFromProgIDEx
                CLSIDFromProgIDEx(progID, out clsid);
            }
            catch (Exception)
            {
                // If CLSIDFromProgIDEx fails, fall back to using CLSIDFromProgID
                CLSIDFromProgID(progID, out clsid);
            }

            // Now attempt to retrieve the active COM object with the obtained CLSID
            GetActiveObject(ref clsid, nint.Zero, out obj);
            return obj;
        }

        // DllImport for CLSIDFromProgIDEx, which retrieves the CLSID for a given ProgID.
        // This method is specific to the OLE32 library and does not return a value
        // because it is marked with PreserveSig = false (it throws exceptions on error).
        [DllImport(OLE32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]  // Prevents the method from exposing resources
        [SuppressUnmanagedCodeSecurity]  // Disables the security checks for unmanaged code
        [SecurityCritical]  // This method is security-critical and is required for access to unmanaged code
        private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        // DllImport for CLSIDFromProgID, a method similar to CLSIDFromProgIDEx but older.
        [DllImport(OLE32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]  // Prevents the method from exposing resources
        [SuppressUnmanagedCodeSecurity]  // Disables security checks for unmanaged code
        [SecurityCritical]  // Denotes that this method is security-critical
        private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        // DllImport for GetActiveObject, which retrieves a pointer to an active COM object 
        // based on a provided CLSID (class identifier).
        [DllImport(OLEAUT32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]  // Prevents the method from exposing resources
        [SuppressUnmanagedCodeSecurity]  // Disables unmanaged code security checks
        [SecurityCritical]  // Denotes this method as security-critical
        private static extern void GetActiveObject(ref Guid rclsid, nint reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }
}

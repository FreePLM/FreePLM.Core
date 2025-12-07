// Cloned From TrueNorthPLM, Free License 08-02-2025 //

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace FreePLM.Core.Helpers.COM
{
    // Model that holds information about a COM object's type, IID (interface identifier), 
    // methods, and properties. This is used to structure data for reflection/inspections.
    public class ObjectIdentifierModel
    {
        public string Type { get; set; } = string.Empty;  // The COM object type (name)
        public Guid IID { get; set; } = Guid.Empty;  // The Interface Identifier (IID) for the COM object
        public List<string>? Methods { get; set; } = null;  // A list of methods the COM object exposes
        public List<string>? Properties { get; set; } = null;  // A list of properties the COM object exposes
    }

    // Helper class that inspects COM objects, retrieving their type, IID, methods, and properties.
    public class ComInspectorHelper
    {
        // Enumeration that defines the types of queries you can perform on a COM object
        private enum _QUERY_TYPES
        {
            TYPE_ONLY,  // Query to get only the type of the object
            TYPE_AND_IID_ONLY,  // Query to get the type and IID (identifier) of the object
            METHODS_AND_PROPERTIES  // Query to get methods, properties, and type & IID of the object
        }

        // Method to retrieve only the type of the COM object
        public ObjectIdentifierModel GetTypeOnly(object comObject)
        {
            return _queryComObject(comObject, _QUERY_TYPES.TYPE_ONLY);
        }

        // Method to retrieve the type and IID (interface identifier) of the COM object
        public ObjectIdentifierModel GetTypeAndIID(object comObject)
        {
            return _queryComObject(comObject, _QUERY_TYPES.TYPE_AND_IID_ONLY);
        }

        // Method to retrieve the methods, properties, and type & IID of the COM object
        public ObjectIdentifierModel GetMethodsAndProperties(object comObject)
        {
            return _queryComObject(comObject, _QUERY_TYPES.METHODS_AND_PROPERTIES);
        }

        // Importing the IDispatch interface which is used for late-bound COM objects
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020400-0000-0000-C000-000000000046")]  // IDispatch IID
        public interface IDispatch
        {
            int GetTypeInfoCount();  // Retrieves the number of type descriptions available for this object
            [return: MarshalAs(UnmanagedType.Interface)]
            ITypeInfo GetTypeInfo(int iTInfo, int lcid);  // Retrieves the type description for the object
        }

        // Private method to query a COM object for its type, IID, methods, and properties
        private ObjectIdentifierModel _queryComObject(object comObject, _QUERY_TYPES what)
        {
            // Initialize an ObjectIdentifierModel to return
            ObjectIdentifierModel returnModel = new ObjectIdentifierModel();

            // If the comObject is null, return the default model (empty values)
            if (comObject == null) return returnModel;

            // If the COM object implements the IDispatch interface (common for COM objects)
            if (comObject is IDispatch dispatch)  // every object in CATIA inherits from IDispatch
            {
                // Get the type information for the COM object using IDispatch
                ITypeInfo typeInfo = dispatch.GetTypeInfo(0, 0); // Assuming default locale

                // Get the type documentation and assign the type name to the model
                typeInfo.GetDocumentation(-1, out string typeName, out _, out _, out _);
                returnModel.Type = typeName;

                // If the query type is TYPE_ONLY, return the model with just the type
                if (what == _QUERY_TYPES.TYPE_ONLY) return returnModel;

                // Get the type attributes (including IID)
                typeInfo.GetTypeAttr(out nint typeAttrPtr);
                TYPEATTR typeAttr = Marshal.PtrToStructure<TYPEATTR>(typeAttrPtr);

                // Set the IID for the model
                returnModel.IID = typeAttr.guid;

                // If the query type is TYPE_AND_IID_ONLY, return the model with just the type and IID
                if (what == _QUERY_TYPES.TYPE_AND_IID_ONLY)
                {
                    // Release the type attribute pointer after use
                    typeInfo.ReleaseTypeAttr(typeAttrPtr);
                    return returnModel;
                }

                // If the query type is METHODS_AND_PROPERTIES, proceed to retrieve methods and properties
                if (what == _QUERY_TYPES.METHODS_AND_PROPERTIES)
                {
                    // List to hold the names of the methods
                    List<string> methods = new List<string>();
                    for (int i = 0; i < typeAttr.cFuncs; i++)  // Iterate over the functions (methods)
                    {
                        // Retrieve function description (method details)
                        typeInfo.GetFuncDesc(i, out nint funcDescPtr);
                        FUNCDESC funcDesc = Marshal.PtrToStructure<FUNCDESC>(funcDescPtr);

                        // Retrieve the documentation for the function (method)
                        typeInfo.GetDocumentation(funcDesc.memid, out string memberName, out _, out _, out _);
                        Console.WriteLine($"Method: {memberName}");  // Optional: Log method names to the console
                        methods.Add(memberName);  // Add method name to the list

                        // Release the function description after use
                        typeInfo.ReleaseFuncDesc(funcDescPtr);
                    }

                    // Set the methods list in the return model
                    returnModel.Methods = methods;

                    // List to hold the names of the properties
                    List<string> properties = new List<string>();
                    for (int i = 0; i < typeAttr.cVars; i++)  // Iterate over the variables (properties)
                    {
                        // Retrieve variable description (property details)
                        typeInfo.GetVarDesc(i, out nint varDescPtr);
                        VARDESC varDesc = Marshal.PtrToStructure<VARDESC>(varDescPtr);

                        // Retrieve the documentation for the variable (property)
                        typeInfo.GetDocumentation(varDesc.memid, out string memberName, out _, out _, out _);
                        Console.WriteLine($"Property: {memberName}");  // Optional: Log property names to the console
                        properties.Add(memberName);  // Add property name to the list

                        // Release the variable description after use
                        typeInfo.ReleaseVarDesc(varDescPtr);
                    }

                    // Set the properties list in the return model
                    returnModel.Properties = properties;
                }

                // Release the type attribute pointer after all processing
                typeInfo.ReleaseTypeAttr(typeAttrPtr);
                return returnModel;
            }
            else
            {
                // If the object does not support IDispatch, log a message and return an empty model
                Console.WriteLine("Object does not support IDispatch.");
                return returnModel;
            }
        }
    }
}

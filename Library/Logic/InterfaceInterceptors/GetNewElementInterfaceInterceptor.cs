using System.Globalization;
using Castle.DynamicProxy;
using Serilog;

namespace CollectionConfig.net.Logic.InterfaceInterceptors
{
   /// <summary>
   /// Sets up the interception logic for "GenerateNewElement" in ListExtensions.
   /// </summary>
   /// <typeparam name="T">The interface to set up a proxy object from</typeparam>
   public class GetNewElementInterfaceInterceptor<TIListOfCustomInterface> : IInterceptor where T : class
   {
      private readonly ILogger? _logger;
      private readonly List<KeyValuePair<string, string>> _storedValues = new();

      /// <summary>
      /// Constructor that sets up injected dependencies
      /// </summary>
      /// <param name="logger">Injected</param>
      public GetNewElementInterfaceInterceptor(ILogger? logger = null)
      {
         _logger = logger;
      }
      
      /// <summary>
      /// Actual logic that handles methods on the proxied object that was created from the settings interface passed
      /// as a type parameter
      /// </summary>
      /// <param name="invocation"></param>
      /// <exception cref="Exception"></exception>
      public void Intercept(IInvocation invocation)
      {
         if (invocation.Method.Name.StartsWith("get_"))
         {
            _logger?.Information(
               "In {ClassName}, Intercepting 'get_'. Specifically: {InterceptedFullString}", 
               nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>), 
               invocation.Method.Name);
            
            // Initialize if necessary, or get stored value and set it as invocation.ReturnValue
            SetInvocationReturnValueAsSpecificType(invocation);
            
            return;
         }
         
         if (invocation.Method.Name.StartsWith("set_"))
         {
            _logger?.Information(
               "In {ClassName}, Intercepting 'set_'. Specifically: {InterceptedFullString}", 
               nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>), 
               invocation.Method.Name);

            SetValueOfProperty(invocation);
            
            return;
         }

         _logger?.Warning(
            "In {ClassName}, Intercepting 'Unsupported'. Specifically: {InterceptedFullString}", 
            nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>), 
            invocation.Method.Name);
         
         throw new Exception($"Intercepted method not supported: {invocation.Method.Name}");
      }

      private void SetValueOfProperty(IInvocation invocation)
      {
         var propertyName = invocation.Method.Name.Replace("set_", "");
         var value = invocation.Arguments[0].ToString();
         var originalPropertyType = invocation.Method.ReturnType;
         
         var timeoutCounter = 10;
         
         while (timeoutCounter > 0)
         {
            for (var i = 0; i < _storedValues.Count; i++)
            {
               var keyValuePair = _storedValues[i];
               
               _logger?.Debug("Checking {NameOfProperty} against stored: {KeyName} which is a " +
                              "{PropertyType} in {ThisClass}",
                  propertyName,
                  keyValuePair.Key,
                  originalPropertyType,
                  nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>));

               if (keyValuePair.Key != propertyName) continue;

               // Otherwise:
               _logger?.Information("Found {NameOfProperty} which is a {PropertyType} in {ThisClass}",
                  propertyName,
                  originalPropertyType,
                  nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>));

               _storedValues.Remove(keyValuePair);
               _storedValues.Add(new KeyValuePair<string, string>(propertyName, value ??
                  throw new ArgumentException($".ToString() on value in {nameof(SetValueOfProperty)} " +
                                              $"was null")));

               // Found it, updated it, exit
               return;
            }

            // Otherwise if not found:
            InitializeKeyValuePairForProperty(propertyName, originalPropertyType);
            timeoutCounter--;
         }

         // This should really never throw, but I think the compiler will get mad if I don't have it
         throw new KeyNotFoundException("Could not find or initialize key " +
                                        $"{propertyName} in {nameof(_storedValues)}");
      }

      private void SetInvocationReturnValueAsSpecificType(IInvocation invocation)
      {
         var method = invocation.Method;

         var propertyName = method.Name.Replace("get_", "");

         var originalPropertyType = method.ReturnType;

         var value =
            GetValueOfStoredProperty(propertyName, originalPropertyType);
         
         // Default value handled as originalPropertyType = string
         invocation.ReturnValue = value;
         
         // Convert if original property was double
         if (originalPropertyType == typeof(double))
         {
            value.TryParse<double>(out var convertedObject);

            invocation.ReturnValue = convertedObject;
         }
         
         // Convert if original property was int
         if (originalPropertyType == typeof(int))
         {
            value.TryParse<int>(out var convertedObject);

            invocation.ReturnValue = convertedObject;
         }
      }
      
      private string GetValueOfStoredProperty(string name, Type originalPropertyType)
      {
         var timeoutCounter = 10;
         
         while (timeoutCounter > 0)
         {
            foreach (var keyValuePair in _storedValues)
            {
               if (keyValuePair.Key != name) continue;

               // Otherwise:
               _logger?.Information("Found {NameOfProperty} which is a {PropertyType} in {ThisClass}", 
                  name, originalPropertyType, nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>));
               
               return keyValuePair.Value;
            }

            // Otherwise if not found:
            InitializeKeyValuePairForProperty(name, originalPropertyType);
            timeoutCounter--;
         }

         // This should really never throw, but I think the compiler will get mad if I don't have it
         throw new KeyNotFoundException($"Could not find or initialize key {name} in {nameof(_storedValues)}");
      }

      private void InitializeKeyValuePairForProperty(string name, Type originalPropertyType)
      {
         _logger?.Information("Initializing {NameOfProperty} which is a {PropertyType} in {ThisClass}", 
            name, originalPropertyType, nameof(GetNewElementInterfaceInterceptor<TIListOfCustomInterface>));
         
         // Initialize if original property was double
         if (originalPropertyType == typeof(double))
         {
            _storedValues.Add(new KeyValuePair<string, string>(
               name, 
               default(double).ToString(CultureInfo.CurrentCulture)));
         }
         
         // Initialize if original property was int
         if (originalPropertyType == typeof(int))
         {
            _storedValues.Add(new KeyValuePair<string, string>(
               name, 
               default(int).ToString()));
         }
         
         // Initialize if original property was string
         if (originalPropertyType == typeof(string))
         {
            _storedValues.Add(new KeyValuePair<string, string>(
               name, 
               ""));
         }
         
         // Initialize if original property was System.Void I have no idea why this happens but it just means
         // original property was a string
         if (originalPropertyType == typeof(void))
         {
            _storedValues.Add(new KeyValuePair<string, string>(
               name, 
               ""));
         }
      }
   }
}

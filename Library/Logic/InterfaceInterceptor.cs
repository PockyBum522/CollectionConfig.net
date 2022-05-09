using Castle.DynamicProxy;
using CollectionConfig.net.Core.Models;

namespace CollectionConfig.net.Logic
{
   /// <summary>
   /// Sets up the interception logic for the proxy object. Anything that's going to run on method calls
   /// using "methods" in the proxy object will get handled here.
   /// </summary>
   /// <typeparam name="T">The interface to set up a proxy object from</typeparam>
   public class InterfaceInterceptor<T> : IInterceptor where T : class
   {
      private readonly InstanceData _instanceData;
      private readonly ProxyGenerator _generator = new ();
      private int _indexBeingAccessedCurrently;

      /// <summary>
      /// Constructor to set up injected dependencies
      /// </summary>
      /// <param name="instanceData">Injected</param>
      public InterfaceInterceptor(InstanceData instanceData)
      {
         _instanceData = instanceData;
      }

      /// <summary>
      /// Actual logic that handles methods on the proxied object that was created from the settings interface passed
      /// as a type parameter
      /// </summary>
      /// <param name="invocation"></param>
      /// <exception cref="Exception"></exception>
      public void Intercept(IInvocation invocation)
      {
         if (invocation.Method.Name == "get_Item")
         {
            SetUpElementProxy(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("get_"))
         {
            SetInvocationReturnValueAsSpecificType(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("set_"))
         {
            // For debugging, temp TODO: Remove this temp variable
            var unused = invocation;

            return;
         }

         throw new Exception($"Intercepted method not supported: {invocation.Method.Name}");
      }

      private void SetInvocationReturnValueAsSpecificType(IInvocation invocation)
      {
         var method = invocation.Method;

         var propertyName = method.Name.Replace("get_", "");

         var originalPropertyType = method.ReturnType;

         var value =
            GetValueOfPropertyAtIndexInCachedList(_indexBeingAccessedCurrently, propertyName);
         
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

      private void SetUpElementProxy(IInvocation invocation)
      {
         var containedInterfaceInList = typeof(T).GetProperties()[0].PropertyType;

         var itemProxy = _generator.CreateInterfaceProxyWithoutTarget(containedInterfaceInList,this);

         invocation.ReturnValue = itemProxy;

         _indexBeingAccessedCurrently = int.Parse(invocation.Arguments[0].ToString() ?? "");
      }

      private string GetValueOfPropertyAtIndexInCachedList(int index, string name)
      {
         UpdateCachedData();
         
         foreach (var keyValuePair in _instanceData.CachedConfigurationItems[index].StoredValues)
         {
            if (keyValuePair.Key == name)
               return keyValuePair.Value;
         }
         
         // Otherwise if not found:
         throw new ArgumentException($"Key {name} not found in cached data");
      }

      private void UpdateCachedData()
      {
         _instanceData.CachedConfigurationItems.Clear();
         
         var cachedData = 
            _instanceData.CacheLoader.UpdateCachedDataFromFile();

         foreach (var fileElement in cachedData)
         {
            _instanceData.CachedConfigurationItems.Add(fileElement);
         }
      }
   }
}

using System.Reflection;
using Castle.DynamicProxy;
using CollectionConfig.net.Core.Interfaces;
using CollectionConfig.net.Core.Models;
using Serilog;

namespace CollectionConfig.net.Logic.InterfaceInterceptors
{
   /// <summary>
   /// Sets up the interception logic for the proxy object. Anything that's going to run on method calls
   /// using "methods" in the proxy object will get handled here.
   /// </summary>
   /// <typeparam name="TIListOfCustomInterface">The interface to set up a proxy object from</typeparam>
   public class InterfaceInterceptor<TIListOfCustomInterface, TNestedCustomInterface> : IInterceptor where TIListOfCustomInterface : class
   {
      private readonly ILogger? _logger = null;
      private readonly IInstanceData _instanceData;
      private readonly ProxyGenerator _generator = new ();
      private int _indexBeingAccessedCurrently;

      /// <summary>
      /// Constructor to set up injected dependencies
      /// </summary>
      /// <param name="instanceData">Injected</param>
      public InterfaceInterceptor(IInstanceData instanceData)
      {
         _instanceData = instanceData;
         _logger = instanceData.Logger;
      }

      /// <summary>
      /// Actual logic that handles methods on the proxied object that was created from the settings interface passed
      /// as a type parameter
      /// </summary>
      /// <param name="invocation">invocation with intercepted method info</param>
      /// <exception cref="Exception">Thrown if method is called which is isn't handled</exception>
      public void Intercept(IInvocation invocation)
      {
         _logger?.Information("New method intercepted in {ThisClass}, method name is {MethodName}",
            nameof(InterfaceInterceptor<TIListOfCustomInterface>), invocation.Method.Name);
         
         if (invocation.Method.Name == "get_Item")
         {
            UpdateCachedData();
            SetUpElementProxy(invocation);
            return;
         }
         
         if (invocation.Method.Name == "GetEnumerator")
         {
            UpdateCachedData();
            SetInvocationReturnValueAsEnumeratorProxy(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("get_Count"))
         {
            UpdateCachedData();
            SetProxyReturnValueAsListCurrentCount(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("get_"))
         {
            UpdateCachedData();
            SetInvocationReturnValueAsSpecificType(invocation);
            return;
         }
         
         // Handles when a proxy element is being added to the proxy object (List of ICustomInterface)
         if (invocation.Method.Name.StartsWith("Add"))
         {
            UpdateCachedData();
            AddElementToFile(invocation);
            UpdateCachedData();

            return;
         }

         throw new Exception($"Intercepted method not supported: {invocation.Method.Name}");
      }

      private void SetProxyReturnValueAsListCurrentCount(IInvocation invocation)
      {
         invocation.ReturnValue = _instanceData.CachedConfigurationItems.Count;
      }

      private void SetInvocationReturnValueAsEnumeratorProxy(IInvocation invocation)
      {
         // Enumerable from cached items
         invocation.ReturnValue = 
            new ProxyListEnumerator<TNestedCustomInterface>(_instanceData);
      }

      private void AddElementToFile(IInvocation invocation)
      {
         var elementToAddToFile = invocation.Arguments[0];

         // For each property, convert to string, add all to last line of file in appropriate "columns"
         AddPropertiesInElementToConfigurationFile(elementToAddToFile);
      }

      private void AddPropertiesInElementToConfigurationFile(object elementToAddToFile)
      {
         // if (_instanceData.FileWriter is null)
         //    throw new NullReferenceException($"{nameof(_instanceData.FileWriter)} was null");
         //
         // var elementFormattedForFile = _instanceData.FileWriter.FormatNewElement(elementToAddToFile);
         //
         // _instanceData.FileWriter.WriteNewElement(elementFormattedForFile);
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
            value.TryParse<double>(out TIListOfCustomInterface convertedObject);

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
         var containedInterfaceInList = typeof(TIListOfCustomInterface).GetProperties()[0].PropertyType;

         var itemProxy = _generator.CreateInterfaceProxyWithoutTarget(containedInterfaceInList,this);

         invocation.ReturnValue = itemProxy;

         _indexBeingAccessedCurrently = int.Parse(invocation.Arguments[0].ToString() ?? "");
      }

      private string GetValueOfPropertyAtIndexInCachedList(int index, string name)
      {
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

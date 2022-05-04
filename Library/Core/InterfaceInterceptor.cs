using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Castle.DynamicProxy;
using CollectionConfig.net.Common.Logic.Csv;
using CollectionConfig.net.Common.Models;

namespace CollectionConfig.net.Common.Core
{
   class InterfaceInterceptor<T> : IInterceptor where T : class
   {
      private readonly CollectionConfigurationInternalData _collectionConfigurationInternalData;
      private readonly CsvCacheLoader _csvCacheLoader;

      private readonly ProxyGenerator _generator = new ();
      private int _indexBeingAccessedCurrently;

      public InterfaceInterceptor(CollectionConfigurationInternalData collectionConfigurationInternalData, CsvCacheLoader csvCacheLoader)
      {
         _collectionConfigurationInternalData = collectionConfigurationInternalData;
         _csvCacheLoader = csvCacheLoader;
      }
      
      public void Intercept(IInvocation invocation)
      {
         if (invocation.Method.Name == "get_Item")
         {
            SetUpElementProxy(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("get_"))
         {
            SetInvocationReturnToValueAsSpecificType(invocation);
            return;
         }
         
         if (invocation.Method.Name.StartsWith("set_"))
         {
            // For debugging, temp TODO: Remove this
            var invocationTemp = invocation;
            
            // Update CSV with new name
            
            return;
         }

         throw new Exception($"Intercepted method not supported: {invocation.Method.Name}");
      }

      private void SetInvocationReturnToValueAsSpecificType(IInvocation invocation)
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
         
         foreach (var keyValuePair in _collectionConfigurationInternalData.CachedConfigurationItems[index].StoredValues)
         {
            if (keyValuePair.Key == name)
               return keyValuePair.Value;
         }
         
         // Otherwise if not found:
         throw new ArgumentException($"Key {name} not found in cached data");
      }

      private void UpdateCachedData()
      {
         _collectionConfigurationInternalData.CachedConfigurationItems.Clear();
         
         var cachedData = _csvCacheLoader.UpdateCachedDataFromCsv(_collectionConfigurationInternalData);

         foreach (var fileElement in cachedData)
         {
            _collectionConfigurationInternalData.CachedConfigurationItems.Add(fileElement);
         }
      }
   }
}

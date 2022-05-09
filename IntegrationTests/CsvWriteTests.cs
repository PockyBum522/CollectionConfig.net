// using System.Collections.Generic;
// using System.IO;
// using CollectionConfig.net.IntegrationTests.TestResources.ExamplePerson;
// using CollectionConfig.net.Main;
// using FluentAssertions;
// using NUnit.Framework;
//
// namespace CollectionConfig.net.IntegrationTests;
//
// public class CsvWriteTests
// {
//     private const string ExamplePersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList_WRITE_TEST.csv";
//     private const string ExampleOriginalPersonListCsvPath = @"TestResources\ExamplePerson\ExamplePersonList.csv";
//
//     [SetUp]
//     public void SetUp()
//     {
//         File.Delete(ExamplePersonListCsvPath);
//         
//         // Ensure file isn't locked open or something (ensure was deleted successfully)
//         File.Exists(ExamplePersonListCsvPath).Should().Be(false);
//     
//         File.Copy(ExampleOriginalPersonListCsvPath, ExamplePersonListCsvPath);
//         
//         // Ensure file is there
//         File.Exists(ExamplePersonListCsvPath).Should().Be(true);
//     }
//     
//     [Test]
//     public void CollectionConfigurationBuilder_WhenBuiltWithCsvFile_ShouldNotBeNull()
//     {
//         var myList = 
//             new CollectionConfigurationBuilder<IList<IExamplePerson>>()
//                 .UseCsvFile(ExamplePersonListCsvPath)
//                 .Build();
//
//         myList.Should().NotBeNull();
//     }
//
//     // [Test]
//     // public void IListGenerateNewElementExtensionMethod_WhenCalled_ShouldGenerateBlankElementProxy()
//     // {
//     //     var myList =
//     //         new CollectionConfigurationBuilder<IList<IExamplePerson>>()
//     //             .UseCsvFile(ExamplePersonListCsvPath)
//     //             .Build();
//     //
//     //     var personToAdd = myList.GenerateNewElement();
//     //
//     //     personToAdd.Name.Should().BeEmpty();
//     //     personToAdd.Age.Should().Be(0);
//     //     personToAdd.MilesRun.Should().Be(0.0);
//     //     personToAdd.PetsName.Should().BeEmpty();
//     // }
//     
// //     [Test]
// //     public void CollectionConfiguration_StringsWhenAdded_ShouldBeAddedToFile()
// //     {
// //         var myList = 
// //             new CollectionConfigurationBuilder<IList<IExamplePerson>>()
// //                 .UseCsvFile(ExamplePersonListCsvPath)
// //                 .Build();
// //
// //         var personToAdd = myList.GenerateNewElement();
// //
// //         personToAdd.Name = "Bocephus";
// //         personToAdd.Age = 97;
// //         personToAdd.MilesRun = 907.32;
// //         personToAdd.Name = "Cerberus";
// //         
// //         myList.Add(personToAdd);
// //         
// //         var result = File.ReadAllText(ExamplePersonListCsvPath);
// //
// //         Console.WriteLine();
// //         
// //         result.Should().Be(@"Name,Age,MilesRun,PetsName
// // David,32,3.15,Whiskey
// // Alyssa,26,2.12,Maxx
// // Robin,28,1.23,Snuggles
// // Dyamond,31,2.23,Trixie
// // Bocephus,97,907.32,Cerberus");
// //         
// //     } 
// }

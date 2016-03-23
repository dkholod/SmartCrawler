module PublisherTests

open System
open System.Net

open Microsoft.ServiceFabric.Actors

open SmartCrawler.Actor.Interfaces

open FsUnit

open Xunit

[<Fact>]
let ``should match # chars``() = 
    let result = SmartCrawler.Actor.SlackPublisher.StringExtensions.ContainsWord("Fluent С# (or F#) development", "F#")
    FsUnit.Assert.AreEqual(true, result)

[<Fact>]
let ``should match only words``() = 
    let negResult = SmartCrawler.Actor.SlackPublisher.StringExtensions.ContainsWord("Build scalable solutions", "scala")
    let posResult = SmartCrawler.Actor.SlackPublisher.StringExtensions.ContainsWord("Build scala solutions", "scala")
    
    FsUnit.Assert.AreEqual(false, negResult)
    FsUnit.Assert.AreEqual(true, posResult)
module DouUaActorTests

open System
open System.Net

open Microsoft.ServiceFabric.Actors

open SmartCrawler.Actor.Interfaces

open FsUnit

open Xunit


(*
    Run this tests to initialize actors first time.
*)

[<Fact>]
let ``start net actor``() = 
    let actor = ActorProxy.Create<IDouUaActor>(new ActorId("DouDotNet"), "fabric:/SmartCrawler")
    actor.GetLastRun().Result |> should not' (be Null)

[<Fact>]
let ``start dou actors``() = 
    ["DouDotNet";"DouJava";"DouScala"] 
    |> Seq.iter (fun name -> 
                            let actor = ActorProxy.Create<IDouUaActor>(new ActorId(name), "fabric:/SmartCrawler")
                            actor.GetLastRun().Result |> should not' (be Null)
                 )
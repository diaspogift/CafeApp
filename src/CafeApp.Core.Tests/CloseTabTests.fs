module CloseTabTests
open Domain
open States
open Commands
open Events
open CafeAppTestsDSL
open NUnit.Framework
open TestData
open Errors

[<Test>]
let ``Can close the tab by paying full amount`` () =
  let order = {order with
                  FoodItems = [salad;pizza]
                  DrinksItems = [coke]}
  let payment = {
    Tab = tab
    Amount = 10.5m
  }

  Given (OrderServed order)
  |> When (CloseTab payment)
  |> ThenStateShouldBe (ClosedTab (Some tab.Id))
  |> WithEvent (TabClosed payment)

[<Test>]
let ``Can not close a tab with invalid payment amount`` () =
  let order = {order with
                  FoodItems = [salad;pizza]
                  DrinksItems = [coke]}

  Given (OrderServed order)
  |> When (CloseTab {Tab = tab; Amount = 9.5m})
  |> ShouldFailWith (InvalidPayment (10.5m, 9.5m))
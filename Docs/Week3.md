# UAT GPE338 Week 3

> I have decided to refactor my [UAT Tanks (UAT-GAM205)](https://github.com/Mordil/UAT-GAM205) course project as this
> course's "final" rather than write a new game project.

> There won't be any more videos here on out that I've recorded of examples.

## Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Events (and Queues)](#events-and-queues)
- [UnityEvents](#unityevents)
- [Object Pooling](#object-pooling)
- [Dependency Injection](#dependency-injection)
- [LINQ](#linq)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Events (and Queues)
[[Back to Top](#table-of-contents)]

Given that software, particularly video games, are an _interactive_ experience - it needs to be written with points of
 entry for the user to _interact_ with it.

However, that brings up the question of how do we respond to an interaction "event"? With event architecture!

When an NPC dies, a user clicks on a button, or a long process has completed could each trigger their own events.

In Week 1, this was implemented using [delegates](./Week1.md#delegates), but you could implement a basic "event" system
 using just event key names with action callbacks in a queue.
 
Or even simpler, if you have a queue dedicated to a particular purpose - such as running actions on the main thread, you
 just need to keep a queue of actions to invoke.
 
I implemented this exact system with the [`TaskManager`](../Harris.Nathan_Week1/Assets/Scripts/TaskManager.cs), and its
 entire purpose was to process all the queued actions that haven't been triggered yet each frame.
 
If I wanted to, I could write another queue for processing user login sessions, like what [Battle.net](http://i.imgur.com/03vEitY.png)
 does when there's a high volume of people logging in at once. 

## UnityEvents
[[Back to Top](#table-of-contents)]

[UnityEvents](https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html) are very similar to [delegates](./Week1.md#delegates)
 in both their use and writing - but they come with the added benefit of having a special [drawer](./Week4.md#property-drawers)
 in the editor.
 
They provide designers the ability to hook in to your custom event architecture, and make quick changes, without needing
 to ask you for every little change.
 
My preference, much like [properties](./Docs/Week2.md#properties) on classes is to use UnityEvents whenever I want to
 have the event exposed to the UI, versus _only_ code.
 
An example of this preference is in the [`StoreManager`](../Harris.Nathan_Week1/Assets/Scripts/UI/Store/StoreManager.cs)
 from week 1. The `StoreManager` does the job of... managing the store. It has code events that other code wants, but
 I'm predicting the UI might want to hook into the event as well.
 
Rather than having fields asking for a `Panel` reference, and then manually toggling the `Active` state, I can let a
 designer hook all of that up within the editor - letting me and the class focus on the single responsibility of 
 managing the store!

## Object Pooling
[[Back to Top](#table-of-contents)]

The idea of object pooling is to front-load the cost of creating objects at runtime to the start of a process, rather than
 spread throughout the application's lifespan. If it takes 10 seconds to load a scene this way, but it allows the game
 to run at 30+ FPS for the entire game round - it's worth it.
 
This idea can span further than game development. Desktop and mobile applications take use of "object pooling" for table
 views, generally called "virtual lists".
  
The software only renders the number of rows (sometimes called `cells`) as it
 can fit on screen at once, but as you "scroll", it redraws the upcoming "cell" in memory with the proper data.
 
In this way, you can have 10,000 table entries, without having to have 10,000 table cells loaded into memory as well.

iOS Developers use the method [`tableView(_:willDisplay:forRowAt)`](https://developer.apple.com/documentation/uikit/uitableviewdelegate/1614883-tableview)
 to provide the data for the upcoming table row, given the index of the next item with the `forRowAt` parameter.

## Dependency Injection
[[Back to Top](#table-of-contents)]

Plainly, dependency injection is explicitly declaring your (data) _dependencies_ at instantiation.

Misko Hevery, from Google, describes it using the "house" analogy.

A house might need a door, a window, and 4 walls in order to be created.

So we'll write the constructor as

```csharp
public House(Door door, Window window, Wall[] walls)
{
    // store these references in member variables
}
```

If a `Door` needs a `Doorknob` in order to be created - that doesn't matter to the House. As part of dependency injection,
 the house just says "I need a door". It doesn't do `this.door = new Door(new Doorknob());`. The idea is to _inject_ what
 you need, rather than create it.

Misko championed dependency injection for testability and clean code purposes in Google Clean Code talks.

I recommend watching his two videos I found on YouTube: [#1](https://www.youtube.com/watch?v=4F72VULWFvc) & [#2](https://www.youtube.com/watch?v=-FRm3VPhseI)

Now, we could improve the code further by using [interfaces](./Week2.md#interfaces) but DI doesn't require it. While
 some will say "never use concrete classes", dependency injection doesn't dictate that.
 The pattern just says "declare what you need".
 
The idea is to avoid global state.

For an idea of factory patterns and dependency injection, check out the [`StockFactory`](../Harris.Nathan_Final/Assets/_core/Scripts/StockExchange/Factories/StockFactory.cs)
 class from the project suffixed `_Final`.
  
A factory needs to know a mapping stock types to a particular texture (an icon) for when later creating Stock Models.

I could have written

```csharp
public StockFactory()
{
    _iconMapping = new Dictionary<String, Texture2D>()
    {
        // my mappings
    };
}
```

but it makes it magical on how the factory knows what all those textures are, their relationship to the types, _where_
 they are, etc. in addition to making it impossible to override for testing.
 
By writing it that it _depends_ on the mapping to function, anyone who needs a factory can provide the information to
 carry out the task. The factory has a single responsibility to create Stock Models - it doesn't need to also know how
 to create a `Dictionary<string, Texture2D>` or what that data needs to look like.

## LINQ
[[Back to Top](#table-of-contents)]

LINQ, or "Language-Integrated Query", is basically SQL in native C#. It provides keyword support for `select`, `where`, 
 and `from` like you would expect in SQL.
 
Under the hood, these are syntax abstractions for LINQ [generic method](./Week1.md#generics-and-templates) queries.

Methods like `ForEach`, `Take`, `Except`, `Select` are provided in the `System.Linq` [namespace](./Week2.md#namespaces) 
 which can be chained off the previous.

As generics, you can use them for any list or array (any [`IEnumerable`](https://msdn.microsoft.com/en-us/library/9eekhta0(v=vs.110).aspx))
 of objects to build queries.

```csharp
var myList = new List<Item>() { ... };
var filteredList = myList
    .Except(item => ...)    // all items that don't match the criteria
    .Select(item => ...)    // select the items a certain way
    .Take(10)               // select the first 10 items
    .ToList();              // finish the query and get a new list back
```

As I stated previously, this can mostly be abstracted with the `linq` syntax, as long as the type is also [`IQueryable`](https://msdn.microsoft.com/en-us/library/system.linq.iqueryable(v=vs.110).aspx)
 (practically all native list data structures are).

```csharp
var myList = new List<item>() { ... };
var filteredList = (from item in myList
                    where item ...      // same logic as the `Except` method above
                    select item ...)    // same as the `Select` method above
                   .Take(10)            // select the first 10 items
                   .ToList();           // finish the query and get a new list back
```

One of the main benefits of `LINQ` is the ability to query and _manipulate_ data sets in a structured way... like SQL.

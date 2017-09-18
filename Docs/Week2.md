# UAT GPE338 Week 2

As not much is different or ready for showing with the final project - refer to [Week 1's](./Week1.md) video below.

[![Week 1 Demo](https://i.ytimg.com/vi/4-327CQCg0I/hqdefault.jpg)](https://youtu.be/4-327CQCg0I)

# Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Data Driven Design](#data-driven-design)
- [Dynamic Resources](#dynamic-resources)
- [Namespaces](#namespaces)
- [Properties](#properties)
- [Interfaces](#interfaces)
- [OOP Principles](#oop-principles)
  - [Inheritance](#inheritance)
  - [Polymorphism](#polymorphism)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Data Driven Design
[[Back to Top](#table-of-contents)]

Data Driven Design is a broad topic and one that was covered in a few talks at GDC 2017 (which I attended):
 - [Complex AI via Data Driven Design](https://www.gdcvault.com/play/1024223/Creating-Complex-AI-Behavior-in)
 - [Dialog System and Tools in 'Firewatch'](https://docs.google.com/document/d/1eLELnYwR911B8-KXKwg87rPbdVTC29VPBR2V5Dh2_vQ/edit?usp=sharing)
   - [Presentation Slides](https://www.gdcvault.com/play/1024415/Do-You-Copy-Dialog-System)
   
However, at the core it's taking data stored outside of the executable during runtime, and loading it. That data is used
 for variables, content, etc. that change how the game is viewed or played.
 
In Week 1, I did this with loading from a [free API service with dummy data](https://jsonplaceholder.typicode.com/). It
 included text and a unique thumbnail image, so I decided to create a list of items with that data.
 
Using the [`WWW`](https://docs.unity3d.com/ScriptReference/WWW.html) class, I could easily fetch JSON data within a
 [coroutine](./Week1.md#coroutines) and then parse it into a C# class instance using the
 [`JsonUtility`](https://docs.unity3d.com/ScriptReference/JsonUtility.html) class provided by Unity.
 
As long as the data coming in is in the shape I expect it - my game will have different ouput, but run the same - hopefully
 bug free. ;)

## Dynamic Resources
[[Back to Top](#table-of-contents)]

Sometimes resources are not available at compile time, such as content from DLC or user-generated content.

Unity provides a method of packaging content into logical units that can be downloaded and accessed at runtime with
 [AssetBundles](https://docs.unity3d.com/ScriptReference/AssetBundle.html).

Individual assets, such as a specific texture, can be loaded using the [`WWW`](https://docs.unity3d.com/ScriptReference/WWW.html)
 class with the [`texture`](https://docs.unity3d.com/ScriptReference/WWW-texture.html) property.
 
> As a commentary on Unity's API - this should probably be a method, as per [Microsoft Guidelines](https://msdn.microsoft.com/en-us/library/ms229054(v=vs.100).aspx)
 referenced in the [properties](#properties) section below.
 
You can see this example in [`StoreItemModel`](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemModel.cs#L42) from
 Week 1:

```csharp
// provide asynchronous access
public IEnumerator FetchImageDat()
{
    // the WWW class only accepts .png and .jpeg images for textures, so we're explicit in the web request
    WWW request = new WWW(thumbnailUrl + ".png");
    
    yield return request;
    
    // use the request.texture getter to receive the request data as a Texture2D
    thumbnailImage = request.texture;
    
    // notify the main scrip that the texture is ready to be assigned to an image / mesh / etc.
    ImageDownloaded();
}
```

## Namespaces
[[Back to Top](#table-of-contents)]

Namespaces are similar to the same keyword as in C++, and the same idea applies: Place code in a namespace to avoid
 collisions and pollution of a namespace.
 
In some languages, namespaces are _physical_ organizations, while in C# they are purely logical.

> **A logical organization** is where it's just an abstract concept. All code within System.Web are logically related to system
> web access / functionality.
>
> **A physical organization**, however, is where code is _physically_ grouped together, such as in a DLL.

Microsoft defines physical organization in _assemblies_ (DLLs) and logical organization in _namespaces_. A namespace could
 be in multiple assemblies, and depending on which you have linked to your application, determines what code exists in those namespaces.
 
Sometimes they'll mean both, in that all code within the `System` namespace is probably in a DLL named `System.DLL`, but nested
 namespaces might be provided by additional DLLs, like `System.Web` from `Networking.DLL`.

A great example is if you need to version your code. The same namespaces exist, but you _physically_ separate the different
 versions of the code into different binaries. Consumers of your code don't need to change anything other than what's necessary
 from changes between versions.
 
By default Visual Studio is configured to create new code within a namespace that follows the project's folder structure.

The intent is to enforce _logical_ grouping of your code, since you wouldn't put your "web" code with your "database" code.

For more, read Microsoft's and NDepend's articles:
 - [Understanding and Using Assemblies and Namespaces in .NET](https://msdn.microsoft.com/en-us/library/ms973231.aspx)
 - [Partitioning code through .NET assemblies](https://www.ndepend.com/Res/NDependWhiteBook_Assembly.pdf)

## Properties
[[Back to Top](#table-of-contents)]

Properties provide a stronger than normal form of data encapsulation within Unity since they are not serialized and
 can't be changed in the editor.
 
My personal coding style is to use properties vs. fields as a way to document _intent_ with my code. A property is something
 that only code cares about - while a field means something that the editor might care about, such as for [JSON serialization](#data-driven-design)
 or editor access.
 
> An example is [StoreItem](../Harris.Nathan_Week1/Assets/Scripts/UI/Store/StoreItem.cs) from Week 1.
 The `Model` property isn't meant to be used within Unity, so I wrote it as a property to hide it from the editor while
 still allowing it to be publicly available to other pieces of code.
 
While they provide a convenient way to trigger other behavior within your class or modify other values that might rely
 on it - a developer should be disciplined to **_avoid side-effects at all cost_**.

Microsoft has written several articles on the discussion of properties vs. methods, and I recommend you read them.
 - [Choosing Between Properties and Methods](https://msdn.microsoft.com/en-us/library/ms229054(v=vs.100).aspx)
 - [CA1024: Use properties where appropriate](https://msdn.microsoft.com/en-us/library/ms182181.aspx)
 
In Week 1 - I attempted to use the [`Model`](../Harris.Nathan_Week1/Assets/Scripts/UI/Store/StoreItem.cs#L14) setter
 to trigger [`UpdateUI()`](../Harris.Nathan_Week1/Assets/Scripts/UI/Store/StoreItem.cs#L33), but I ran into issues with
 the field assignment correctly starting the [coroutine](./Week1.md#coroutines).

## Interfaces
[[Back to Top](#table-of-contents)]

Interfaces in C# are in general syntax sugar for [pure abstract classes from C++](http://www.cplusplus.com/forum/general/202797/).

They allow the definition of API contracts and play nicely with [Dependency Injection](./Week3.md#dependency-injection)
 for decoupling of code.
 
The reason something is decoupled is the same reason as [generic / template programming](.Week1.md##generics-and-templates)-
 functions are reusable for objects that are similar. As long as they adhere to the API contract, the same function should work
 for two classes that might not even be related.
 
For example, if an Enemy and a Player both implement the interface `ITakeDamage`, our combat engine just needs to ask for the object,
and then make the appropriate call such as `ITakeDamage.TakeDamage(damageToTakeVar)`.

The combat engine doesn't have to handle _how_ those objects take the damage - it passes the responsibility to the object itself.

In the final project, the underlying systems don't have a concept of different [Stock Options](../Harris.Nathan_Final/Assets/_core/Scripts/StockExchange/IStock.cs) -
 to them, they're all the same. The individual concrete types of stock, like [TechStock](../Harris.Nathan_Final/Assets/_core/Scripts/StockExchange/Models/TechStock.cs)
 determine how they _implement_ themselves **as** Stock.
  
This can also be seen in the interaction between [IJsonModel](../Harris.Nathan_Week1/Assets/Scripts/Interfaces/IJsonModel.cs),
 [StoreItemModel](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemModel.cs), and
 [StoreItemFactory](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemFactory.cs) from Week 1.

> Admitedly, it's not the greatest example as the `StoreItemFactory` was too coupled to `StoreItemModel` and should have probably been
 broken into a separate class to handle just `IJsonModel` serialization.
>
> However, the premise is there that the method doesn't care
 what the actual object is - as long as it says it supports the `IJsonModel` contract, we expect it to work within our method.
 
## OOP Principles
[[Back to Top](#table-of-contents)]

While working on these new concepts, it should always be done with continual application of basic OOP principles:
* Encapsulation
* Abstraction
* Inheritance
* Polymorphism

> Interestingly enough, "Uncle" Bob Martin argues that these are not new with OOP, and they inherently cannot be strictly
 adhered to by OOP.
 
> He covers it a talk about a few SOLID principles in Agile development. The video is available on YouTube:
 [https://www.youtube.com/watch?v=t86v3N4OshQ](https://www.youtube.com/watch?v=t86v3N4OshQ)

> It starts around [14 minutes](https://youtu.be/t86v3N4OshQ?t=846) in where he covers this specifically.

I won't cover encapsulation here, as I've covered some advanced topics from [namespaces](#namespaces) and [properties](#properties).

In addition, abstraction is covered from the advanced topic in the [interfaces](#interfaces) section.

To read more on those concepts I've found a few links on all OOP Principles:
 - [Object-Orientated Programming Principles - Intro Programming](http://www.introprogramming.info/english-intro-csharp-book/read-online/chapter-20-object-oriented-programming-principles/)
 - [4 Major Principles of Object-Orientated Programming - Code Better](http://codebetter.com/raymondlewallen/2005/07/19/4-major-principles-of-object-oriented-programming/)
 - [Object-Orientated Programming Concepts and Principles - IBM](https://www.ibm.com/developerworks/library/j-perry-object-oriented-programming-concepts-and-principles/index.html)

### Inheritance
[[Back to Top](#table-of-contents)]

Inheritance allows code sharing and creating relationships between code. The common example is the [Shape](https://swinbrain.ict.swin.edu.au/wiki/Object_Oriented_Programming_-_Drawing_Example_-_Java_Code).

However, it does more when you start integrating [interfaces](#interfaces) - we can enforce implementation or gain implementations.

I show this in the [BaseStock](../Harris.Nathan_Final/Assets/_core/Scripts/StockExchange/Models/BaseStock.cs) class.

It is a pure abstract class that I intend **all** stock classes to derive from, and because it's a pure abstract class,
 it enforces implementation of the [IStock](../Harris.Nathan_Final/Assets/_core/Scripts/StockExchange/IStock.cs) interface.
 
However, to the implementer, it doesn't look like it's an interface implementation - it looks like requirements set by the parent.

While providing these implementations, derived classes also gain shared [properties](#properties) which saves code repetition.

### Polymorphism
[[Back to Top](#table-of-contents)]

Building off of [inheritance](#inheritance) above - we see an example of polymorphism in practice as well.

In all my code, I can reference either the `BaseStock` or `IStock` types and not worry about the underlying implementation
 of each concrete class - a `TechStock`, `FinanceStock`, or `ConstructionStock` could all be treated as the same.

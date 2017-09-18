# UAT GPE338 Week 1

[![Week 1 Demo](https://i.ytimg.com/vi/4-327CQCg0I/hqdefault.jpg)](https://youtu.be/4-327CQCg0I)

# Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Coroutines](#coroutines)
- [Delegates](#delegates)
- [Lambda Expressions](#lambda-expressions)
- [Generics & Templates](#generics-and-templates)
- [Basic Multi-threading](#basic-multi-threading)
- [Single Use Principle](#single-use-principle)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Coroutines
[[Back to Top](#table-of-contents)]

Coroutines are functions that make use of the [`IEnumerator`](https://msdn.microsoft.com/en-us/library/system.collections.ienumerator(v=vs.110).aspx)
 interface where an object will have a method for getting the "next" stage for execution to use.

In Unity, we use the `yield` with a return value to drive how the "next" item (execution flow) is determined.

An example of this is in the [`StoreItemModel`](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemModel.cs#L42) where the method for fetching a texture from a URL is handled "asynchronously".

```csharp
public IEnumerator FetchImageData()
{
    WWW request = new WWW(thumbnail + ".png");
    
    yield return request;
    
    thumbnailImage = request.texture;
    
    ImageDownloaded(); // event delegate for possible listeners
}
```

We want to fetch the image, but we don't want to block the main thread from being able to do other tasks.

Unity will take this coroutine (when we start it with `StartCoroutine(FetchImageData())`) and it will wait until
the `request` object has stated that we can continue. Once that happens, execution will return to assign the texture
and then invoke the event delegate for code that might be listening for the event (so they know when the task is done).

It's also important to do this with Unity Coroutines, rather than a background thread, because coroutines are executed on the main thread
and Unity APIs are only allowed to be accessed from that thread.

If we wanted to do a pure multi-threaded solution, another web request class or framework would be required.

## Delegates
[[Back to Top](#table-of-contents)]

Delegates are variables that store a method with a particular signature.

For example:
```csharp
public delegate void ImageDownloadedHandler();
```

defines a signature for a method that returns nothing, and accepts no parameters (a parameterless "action");

We can then use the delegate as a variable type to create an "event" field that other pieces of code can subscribe to.

```csharp
public ImageDownloadedHandler ImageDownloaded;

ImageDownloaded += () => // do something.
ImageDownloaded += () => // do another thing
```

This provides a way to schedule tasks to be invoked at specific moments perhaps between threads.

An example of this is again in [`StoreItemModel`](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemModel.cs).

To be a self-contained piece of code, the model is ignorant of any possible outside desires for its data. It just fetches
data when requested, and stores it for later access. Once we're done, we call `ImageDownloaded()` so that if external
code has subscribed to know when we finished, they know it's safe to now access `thumbnailImage` to retrieve non-null data.

## Lambda Expressions
[[Back to Top](#table-of-contents)]

Lambda expressions are just "shorthand notation" for methods, that might exist only temporarily, without needing to do
 the normal form of having it declared in a namespace, struct, etc.

They make use of the special `=>` syntax, with just parenthesis and curly braces. (I actually used it in the Delegate example above!)

It provides a way to be less verbose in our code when we just need to do one off actions.

An example of this is in [`StoreManager`](../Harris.Nathan_Week1/Assets/Scripts/UI/Store/StoreManager.cs#L49). I only need
to wrap the actual completion handler and unsubscribe the handler, and I only want to do it when the parent method is
 invoked - it's a self-contained purpose within the `StartJsonDataTransforming(string)` method.

If I was to create a separate method, and reference it that way, it gives ambiguous design intention to another developer
 who might think it's appropriate some times to call the method directly.

However, because it doesn't do anything unless used conjunction with `StartJsonDataTransforming(string)` it's more
 appropriate to have a temporary method.

## Generics and Templates
[[Back to Top](#table-of-contents)]

Generics were helpful to reuse a class without needing to hardcode a new variation every time I wanted to reuse it.

The best example is [`JsonCollection`](../Harris.Nathan_Week1/Assets/Scripts/Models/JsonCollection.cs) as with `JsonUtility` it expects
a root object, rather than an array and generally doesn't handle arrays that well.

Since it's just an object with an array of items, I was able to define a generic `T` type that conforms to the `IJsonModel`
 interface (see [Interfaces in week 2](./Week2.md#interfaces)) and have the ability to deserialize an array.
 
This gives me the ability to reuse this class anywhere by using angle brackets to provide the concrete type:
 (such as in [`StoreItemFactory`](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemFactory.cs#L77) where the type is passed through)

```csharp
var data = JsonCollection<T>.CreateFromJSON(parameters.JSON, 10, true);
```

## Basic Multi-threading
[[Back to Top](#table-of-contents)]

Historically, JSON parsing (deserialization / serialization) is slow, and because I might not know how many items I'm getting back
from the web service API, I elected to do the `JsonUtility` tasks on a background thread to have the highest speed that doesn't
block the main thread.

This is done in [`StoreItemFactory`'s](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemFactory.cs) 
`TransformJsonToModel` and `TransformData` methods.

To get around main thread requirements to access Unity APIs, I wrote the [`TaskManager`](../Harris.Nathan_Week1/Assets/Scripts/TaskManager.cs)
 class that utilizes locks to process a queue of actions on the main thread, since the execution of actions in the queue
 is done within a coroutine.

## Single Use Principle
[[Back to Top](#table-of-contents)]

Throughout the project, I tried to stick to process of a function doing one task, and then calling another function to carry out more work.

A great example is in [`StoreItemFactory`](../Harris.Nathan_Week1/Assets/Scripts/Models/StoreItemFactory.cs) where the main public method
just starts the background thread and passes control to the real thread doing the work.

That thread then just calls another method to deserialize JSON, before passing that intermediate data to another function that handles
object instantiation.

All of that is returned in a delegate call, rather than direct assignments.

The benefit of this architecture is that if I maintain the input and output, I theoretically can make changes to within the method
without breaking anything - which makes refactoring easier down the road.

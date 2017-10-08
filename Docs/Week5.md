# UAT-GPE338 Week 5

[![Week 5 Walkthrough](https://i.ytimg.com/vi/BBslKdqNPIs/hqdefault.jpg)](https://youtu.be/BBslKdqNPIs)

# Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Project Requirements](#project-requirements)
  - [Required Concepts](#required-concepts)
    - [Namespaces](#namespaces)
    - [Coroutines](#coroutines)
    - [Interfaces](#interfaces)
    - [Getters / Setters](#getters--setters)
    - [ScriptableObjects](#scriptableobjects)
  - [Optional Requirements](#optional-requirements)
    - [Generics](#generics)
    - [UnityEvents](#unityevents)
    - [LINQ](#linq)
    - [Custom Editor Scripting](#custom-editor-scripting)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Project Requirements
[[Back to Top](#table-of-contents)]

To complete this course, I decided to refactor my old [UAT-GAM205 "UAT Tanks"](https://github.com/Mordil/UAT-GAM205)
 game.
 
The changes can be seen on the [#refactor-tanks](https://github.com/mordil/uat-gam205/tree/refactor-tanks) branch.

The executable from a previous build (as I broke a lot of prefabs and did not have the time to fix them) is available
from [Dropbox](https://www.dropbox.com/sh/nvditz9r5wqhreb/AAAwpv6DsHFYX35evBJOnYqia?dl=0).

The final project had to be a game that utilized the following concepts covered in the class:

- [Namespaces](./Week2.md#namespaces)
- [Coroutines](./Week1.md#coroutines)
- [Interfaces](./Week2.md#interfaces)
- Getters / Setters
- [ScriptableObjects](./Week4.md#scriptableobjects)

In addition, I had to implement at least 4 of the following optional concepts:

- [Delegates](./Week1.md#delegates)
- [Generics](./Week1.md#generics-and-templates)
- [Multithreading](./Week1.md#basic-multi-threading)
- [JSON parsing](./Week2.md#data-driven-design)
- [Dynamic Asset Loading](./Week2.md#dynamic-resources)
- [UnityEvents](./Week3.md#unityevents)
- [Object Pooling](./Week3.md#object-pooling)
- [Dependency Injection](./Week3.md#dependency-injection)
- [LINQ](./Week3.md#linq)
- [Custom Editor Scripting](./Week4#unity-editor-scripts)
- [Bitmasking](./Week4.md#bitmasking)

### Required Concepts
#### Namespaces
[[Back to Top](#table-of-contents)]

This concept was used very weakly in the refactor work. In places that I used my "utility library", I properly
namespaced those components so as to keep my individual projects' global namespace clear.

In the codebase directly, I only used a namespace for `UATTanks.Tank.Components`. The plan was to properly namespace
all the individual tank components (game concept) into that namespace, so it's more closely tying game concepts to
literal code concepts.

See the [`PowerupAgent`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Components/PowerupAgent.cs#L7)
for more details.

#### Coroutines
[[Back to Top](#table-of-contents)]

This concept was also used very weakly in the refactor work. I had hoped to replace more timer-based implementations with
coroutines.

Luckily, I started this work with the Bullet's lifespan from tanks. Before, I was using the `Time` class in `Update()`
calls, but now I start a Coroutine that just yields the time - making the code much cleaner and easier to understand.

See [`TankBullet`](https://github.com/Mordil/UAT-GAM205/blob/master/NHarris_UATanks/Assets/Scripts/Tank/TankBullet.cs#L56) on master
 for the before, and [`TankBulletAgent`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Shooting/TankBulletAgent.cs#L44)
 on the `refactor-tanks` branch for the after.

#### Interfaces
[[Back to Top](#table-of-contents)]

In the UAT Tanks project, I had several components that made up a tank, but none of them had a common "base" that made
sense for a concrete base class to inherit from.

So I created an [`ITankComponent`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Components/ITankComponent.cs)
that each component should implement so as to quickly initialize their data at runtime, and providing the actual "base"
data that each shared - their need for the `TankSettings`.

#### Getters / Setters
[[Back to Top](#table-of-contents)]

Throughout the codebase, I used getters extensively to provide `read only` access to private variables that I only wanted
the editor to have access to.

Prime examples are all of the [`TankSettings`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Settings/TankSettings.cs)
 and sub "variations".
 
I didn't use property setters, as they generally required more than just a single value write - so they were done through
methods, or managed internally by classes.

#### ScriptableObjects
[[Back to Top](#table-of-contents)]

ScriptableObjects are my favorite Unity concept, and I used them for managing several data assets.

[Powerups](https://github.com/Mordil/UAT-GAM205/tree/refactor-tanks/NHarris_UATanks/Assets/Scripts/Powerups) are all
 implemented as ScriptableObjects, as well as
 [Tank Settings](https://github.com/Mordil/UAT-GAM205/tree/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Settings).

### Optional Requirements
#### Generics
[[Back to Top](#table-of-contents)]

While I don't use Generics at all, except for making calls or using generic base classes, I did implement generics
 in my [Unity Utility](https://github.com/Mordil/Unity-Utility) project.
 
In this project, I use the [`AppManagerBase<T, U>`](https://github.com/Mordil/Unity-Utility/blob/master/L4.Unity.Common/GameObjects/AppManagerBase.cs#L8)
 class for my [`GameManager`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Application/GameManager.cs#L37).

#### UnityEvents
[[Back to Top](#table-of-contents)]

UnityEvents are another fantastic feature of Unity, giving the ability to use the [delegates](./Week1.md#delegates) concept
 with support for Editor drawers to hook up listeners through the editor without needing code behind support.
 
I decided to use them instead of plain C# delegates for the [`HealthAgent`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Components/HealthAgent.cs#L11)
 in case down the road I wanted to do more on those events that didn't require code behind support.
 
It also removed the need for using the Unity Message system, as tanks could conditionally register handlers if the `HealthAgent`
 was attached to the `GameObject` or not.

#### LINQ
[[Back to Top](#table-of-contents)]

LINQ, while not the "SQL like" query implementation, was used in the `PowerupAgent` to clean up the `Update()` loop.

Before, I looped through all of the powerups, and stored them in intermediate lists that I later used in the update loop.

That caused memory overhead of creating the lists, and doing multiple iterations through the containers.

I was able to refactor and simplify the loop to iterate only on a subset of powerups and remove them in the same pass.

For the before, see [`TankController.UpdatePickups()`](https://github.com/Mordil/UAT-GAM205/blob/master/NHarris_UATanks/Assets/Scripts/Tank/TankController.cs#L251)
 on master, and for after see [`PowerupAgent.Update()`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Scripts/Tank/Components/PowerupAgent.cs#L22)
 on the `refactor-tanks` branch.

#### Custom Editor Scripting
[[Back to Top](#table-of-contents)]

This section was extremely basic in this project.

I used it to create a `ReadOnly` serialized field that would render values in the editor - but only to view.

It made it great for debugging, and making sure I don't accidentally edit the values.

See the [`ReadOnlyDrawer`](https://github.com/Mordil/UAT-GAM205/blob/refactor-tanks/NHarris_UATanks/Assets/Editor/Inspector/ReadOnlyDrawer.cs#L9)
 for more details.

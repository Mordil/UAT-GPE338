# UAT-GPE338 Week 4

> I have decided to refactor my [UAT Tanks (UAT-GAM205)](https://github.com/Mordil/UAT-GAM205) course project as this
> course's "final" rather than write a new game project.

> There won't be any more videos here on out that I've recorded of examples.

# Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<!-- END doctoc -->

- [Unity Editor Scripts](#unity-editor-scripts)
  - [GUI](#gui)
  - [Property Drawers](#property-drawers)
- [Bitmasking](#bitmasking)
- [Attributes](#attributes)
- [ScriptableObjects](#scriptableobjects)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Unity Editor Scripts
[[Back to Top](#table-of-contents)]

Unity is a one-size-fits-all editor to game development, and some times your particular game might be easier to
work on with shortcuts within the editor - this is where the `UnityEditor` [namespace](./Week2.md#namespaces) comes in.

It provides several ready to use [attributes](#attributes) and base classes for extending the editor to be more custom
to your project.

Examples of things you can add:
- [Context Menus](https://docs.unity3d.com/ScriptReference/MenuItem.html)
- [Design time execution](https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html)

### GUI
[[Back to Top](#table-of-contents)

Fully known as [_Immediate Mode GUI_](https://docs.unity3d.com/Manual/GUIScriptingGuide.html) (IMGUI), this systems allows
swift creation of a GUI that is intended to be used during development. Things like custom inspectors or quick actions.

To use it, implement the [`OnGUI`](https://docs.unity3d.com/Manual/gui-Basics.html) method in your `MonoBehaviour` subclasses.

Inside it, you can use several static methods on the [`GUI`](https://docs.unity3d.com/ScriptReference/GUI.html) and
[`GUILayout`](https://docs.unity3d.com/ScriptReference/GUILayout.html) classes.

To customize the look of the built-in controls, you can apply your own [`skins`](https://docs.unity3d.com/Manual/class-GUISkin.html)
which are a collection of [`styles`](https://docs.unity3d.com/Manual/class-GUIStyle.html).

### Property Drawers
[[Back to Top](#table-of-contents)]

Described as "draw-ers", these allow developers to customize how things are rendered in the Unity Inspector.

When you declare a public field (or use `[SerializeField]` on a private field), Unity tries to use its own
 [`Property Drawers`](https://docs.unity3d.com/Manual/editor-PropertyDrawers.html). Such as for an array, it'll create
 a nice dropdown to toggle displaying the items.
 
We can implement our own, and associate it with particular classes, so when we declare serialized fields, it'll render
 how we want.
 
A simple example is what I wrote in [the project final](https://github.com/Mordil/UAT-GAM205) - [`ReadOnly`](https://github.com/Mordil/UAT-GAM205/blob/master/NHarris_UATanks/Assets/Editor/Inspector/ReadOnlyDrawer.cs).

It allows you to mark a field as "Read Only" so that it can't be changed in the editor.

This is useful for when you want to quickly see debug information, but don't want to accidentally edit it.

I could easily write this in an `OnGUI` method, as mentioned in the previous section - but the UI might become cluttered
and it might bloat the class when I can use add an attribute to a field.

Unity provides a decently quick tutorial on [Building a Custom Inspector](https://unity3d.com/learn/tutorials/topics/interface-essentials/building-custom-inspector).
 
## Bitmasking
[[Back to Top](#table-of-contents)]

Bitmasking, otherwise known as "Enum Flags", is the process of using bit math to calculate a compound value
 with non-mathematical meaning - such as Enums!
 
The idea is to store each enum value of true (is selected) and false (not selected) within its own column.

For example: `enum DamageType { None, Lightening, Poison, Ice, Fire }`

| Fire | Ice | Poison | Lightening |
| :--: | :-: | :----: | :--------: |
|   0  |  1  |   0    |     1      |

In binary, this value will be read as `5` - but _semantically_ it means that both `Ice` and `Lightening` is Selected.

However, in order to actually support this we need to assign the backing value for each enumeration.
 By default it increments by ones (in the example, `Fire` would evaluate to `3`).
 
We can do this two ways:

1. Explicitly assign by powers of 2

```csharp
enum DamageType
{
    None = 0,
    Lightening = 1,
    Poison = 2,
    Ice = 4,
    Fire = 8
}
```

2. Use the `<<` [bit shift operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/left-shift-operator).

```csharp
enum DamageType
{
    None = 0,
    Lightening = 1 << 1,
    Poison = 1 << 2,
    Ice = 1 << 3,
    Fire = 1 << 4
}
```

We assign the first value of `0` to signify that `0000` is the `None` enum.

| Binary Value | Meaning |
| :----------: | :------ |
| 0000         | `None` |
| 0001         | `Lightening` |
| 0010         | `Poison` |
| 0011         | `Poison` & `Lightening` |
| 0100         | `Ice` |

etc.

We can do boolean logic and modify these values without having to convert to numeric representations with the _bitwise_
 operators

- AND (`&`)
  - `if (Weapon.DamageType & DamageType.Fire == DamageType.Fire) { ... }` (checks if it has the value)
- AND NOT (`~`)
  - `Weapon.DamageType &= ~DamageType.Fire` (removes the value)
- OR (`|`)
  - `Weapon.DamageType |= DamageType.Ice` (adds the value)
- XOR (`^`)
  - `Weapon.damageType ^= DamageType.Lightening` (toggles the value)
  
It can calculate this by looking at the representations of both values in binary.

| Binary | Name |
| :----: | :--- |
| 0101   | `Weapon.DamageType` |
| 0010   | `DamageType.Poison` |

If we ask if `Weapon.DamageType` _AND_ `DamageType.Poison` have the value in the same bit column - then it returns true.

For adding / removing, we're asking for whichever has a `1` (true) field and retrieving that value in order to add it.

To get some "out of the box" utilities for Enum Flags, use the [`[Flags]`](https://msdn.microsoft.com/en-us/library/system.flagsattribute(v=vs.110).aspx).

## Attributes
[[Back to Top](#table-of-contents)]

Attributes are "metadata" for methods, fields, classes, etc. that the assists the compiler or other code that uses
 [`Reflection`](https://en.wikipedia.org/wiki/Reflection_(computer_programming)).
 
Examples we're familiar with are like `[SerializeField]`, `[RequireComponent]`, and `[Range]`.

These tell the Unity Editor how to react when it sees it needs to display or interact with our class.

We can create our own, like I did for the `[ReadOnly]` attribute for creating a custom [Property Drawer](#property-drawers)
 for anything we want, such as a [custom drawer](http://www.sharkbombs.com/2015/02/17/unity-editor-enum-flags-as-toggle-buttons/)
 for [bitmasked fields](#bitmasking).
 
For more in-depth information on Attributes, see the [Microsoft Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/).

## ScriptableObjects
[[Back to Top](#table-of-contents)]

`ScriptableObjects` are like `MonoBehaviour`s... but without the "magic" of `Update`, `Awake`, etc.

They're "component assets", rather than "components". When you create an instance of the class, it's an asset in your project.

Their power comes from being able to use the [`Delegation` pattern](https://en.wikipedia.org/wiki/Delegation_pattern)
 in conjunction with [interfaces](./Week2.md#interfaces).
 
A perfect example is from the project final, with `powerups`(https://github.com/Mordil/UAT-GAM205/tree/refactor-tanks/NHarris_UATanks/Assets/Scripts/Powerups).

1. Each `Powerup` is a `ScriptableObject`, so a designer could easily create 5 different ones without having to manage
 entire prefabs.
2. Each `Powerup` implements methods to interact with an entity that picks it up, rather than the entity having to do the
 logic.
3. The `PowerupAgent` only needs to have a field for the type `Powerup`.

You could also use them for just data storage, such as a loot table.

```csharp
public class BossDropTable : ScriptableObject
{
    [SerializeField]
    private float _item1Chance;
    public float Item1Chance { get { return _item1Chance; } }
    
    ...
}
```

I stumbled across this fantastic video from Unite 2016 titled
 ["Overthrowing the MonoBehaviour Tyranny in a Glorious ScriptableObject Revolution"](https://www.youtube.com/watch?v=6vmRwLYWNRo)
 that covers the topic thoroughly.

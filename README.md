# TryGet

## Simple, fluent dictionary value retrieval.

![Nuget](https://img.shields.io/nuget/v/tryget)
![Nuget](https://img.shields.io/nuget/dt/tryget)
[![.NET](https://github.com/Confusingboat/tryget/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Confusingboat/tryget/actions/workflows/dotnet.yml)

**As anyone familiar wth C# knows, you cannot use the `as` or `is` keyword to change or check types with the `out var` syntax.**  

For example, something like this is not possible:
```csharp
var dictionary = new Dictionary<string, object>();

if (dictionary.TryGetValue("key", out as string val)) { }
```

**This project aims to address the above desired use case.**

# Install

Package manager console:  
`install-package tryget`

Dotnet CLI:  
`dotnet add package tryget`

# Quick Start

```csharp
using TryGet;
```

```csharp
// Check if the key exists
if (dictionary.TryGet("key")) { }

// Check if the key exists and can be cast to string
if (dictionary.TryGet("key").As<string>()) { }

// Capture the value at key "key" if it exists and can be cast to string, otherwise default
var val = dictionary.TryGet("key").As<string>().OrDefault();

// Same as above, but with custom default value
var val = dictionary.TryGet("key").As<string>().OrDefault("Not a string.");

// Check if the key exists and capture the value with 'out' keyword
if (dictionary.TryGet("key").As<string>(out var val)) { }

```

## More Examples

Let's set up our dictionary.

**TryGet** works on any `IDictionary<TKey, TValue>`, but it's most useful on `<string, object>` dictionaries commonly used to pass custom data.

We will assume this dictionary exists for our examples.

```csharp
var dictionary = new Dictionary<string, object>
{
    { "key", new { } },
    { "string", "This is a string." },
    { "number", 15 }
};
```

### Check if a value

**...exists**
```csharp
if (dictionary.TryGet("key"))
{
  // "key" exists, do stuff
}
```

**...exists and is of a certain type**
```csharp
if (dictionary.TryGet("string").As<string>())
{
  // "string" exists and is a string
}
```
You can also check casts to polymorphic types.
```csharp
class Animal { }
class Cat : Animal, IHasNineLives { }
class Dog : Animal { }
interface IHasNineLives { }
...
var animals = new Dictionary<string, Animal>
{
    { "cat", new Cat() },
    { "dog", new Dog() }
}

if (animals.TryGet("cat").As<Cat>())
{
    // Success
}
if (animals.TryGet("cat").As<Dog>()}
{
    // Not success
}
```
Interfaces work too
```csharp
if (animals.TryGet("cat").As<IHasNineLives>())
{
    // Success
}
if (animals.TryGet("dog").As<IHasNineLives>())
{
    // Not success
}
```
I guess this works too if you're just a mad lad
```csharp
if (animals.TryGet("cat").As<object>().As<Animal>().As<Cat>())
{
    // Success
}
```

### Get a value
**...if it exists**
```csharp
var val = dictionary.TryGet("number");

// val = 15 (boxed to an object)
```

**...if it exists and is of a certain type, otherwise return the default**
```csharp
var val = dictionary.TryGet("number").As<int>().OrDefault();

// val = 15
```

```csharp
var val = dictionary.TryGet("number").As<string>().OrDefault();

// val = null
```

`OrDefault()` also supports custom default values
```csharp
var val = dictionary.TryGet("number").As<string>().OrDefault("Not a string.");

// val = "Not a string."
```

You can also perform a check and capture the value with `out`
```csharp
if (dictionary.TryGet("number").As(out var val))
{
    // val = 15 (boxed to an object)
}
```
The real value-add here is being able to cast with `out`
```csharp
if (dictionary.TryGet("number").As<int>(out var val)){
    // val = 15
}
```
## Using the TryGet result
Calling `.TryGet()` returns a result of type `TryGetResult<TValue>`, which can be used directly instead of the fluent methods.
```csharp
var result = dictionary.TryGet("number");
if (result.IsSuccess)
{
    // Success!
    // Do something with result.Value
}
```
The result also supports being cast directly to a compatible result type.
```csharp
string stringValue = (string)result;
```
The only type you **cannot** cast to is `bool`; this is a special case used for indicating success.

`Value` will throw a `NoValueException` if the key did not exist:
```csharp
var result = dictionary.TryGet("does_not_exist");
var val = result.Value;  // !!!
```
It will also throw a `NoValueException` if the retrieved value was not the correct type:
```csharp
var result = dictionary.TryGet("string").As<int>();
var val = result.Value; // !!!
```

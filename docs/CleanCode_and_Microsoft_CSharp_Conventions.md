
# 🧼 Clean Code + Microsoft C# Code Conventions

This document combines the principles of clean coding with Microsoft's official C# coding conventions.

---


# 🧼 Principles of Clean Code

Clean code is code that is easy to read, understand, and maintain. It's about writing code that communicates clearly and can be changed with confidence.

---

## ✅ 1. Meaningful Names

### ❌ Bad
```csharp
int d; // what is d?
```

### ✅ Good
```csharp
int elapsedTimeInDays;
```

---

## ✅ 2. Small Functions

### ❌ Bad
```csharp
void ProcessData() {
    // Too much happening here
}
```

### ✅ Good
```csharp
void ValidateInput() { }
void TransformData() { }
void SaveToDatabase() { }
```

---

## ✅ 3. Single Responsibility Principle (SRP)

A class should have only one reason to change.

### ❌ Bad
```csharp
public class ReportManager {
    public void GenerateReport() { }
    public void SaveToFile() { }
}
```

### ✅ Good
```csharp
public class ReportGenerator {
    public void GenerateReport() { }
}

public class ReportSaver {
    public void SaveToFile() { }
}
```

---

## ✅ 4. Use of Comments Wisely

### ❌ Bad
```csharp
// Increment i by 1
i = i + 1;
```

### ✅ Good
```csharp
// Retry because previous attempt failed due to timeout
RetryOperation();
```

---

## ✅ 5. Avoid Magic Numbers

### ❌ Bad
```csharp
if (user.Age > 18) { }
```

### ✅ Good
```csharp
const int LegalAge = 18;
if (user.Age > LegalAge) { }
```

---

## ✅ 6. Consistent Formatting

Use consistent indentation, spacing, and brackets.

---

## ✅ 7. Handle Errors Gracefully

### ❌ Bad
```csharp
var result = 1 / int.Parse(input);
```

### ✅ Good
```csharp
if (int.TryParse(input, out var number) && number != 0) {
    var result = 1 / number;
}
```

---

## ✅ 8. Write Unit Tests

Ensure that your code can be verified through automated tests.

```csharp
[Test]
public void Add_ReturnsSum_WhenCalled() {
    var result = Calculator.Add(2, 3);
    Assert.AreEqual(5, result);
}
```

---

## 🧙‍♂️ Famous Quote
> "Clean code always looks like it was written by someone who cares." — Robert C. Martin

---

## 📘 Resources
- *Clean Code* by Robert C. Martin
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)


---


# 📘 Microsoft C# Coding Conventions

These conventions help developers write consistent, maintainable, and readable C# code.

---

## ✅ 1. Naming Conventions

### ✔ PascalCase for:
- Class names
- Method names
- Properties

```csharp
public class InvoiceProcessor { }
public void ProcessInvoice() { }
public string CustomerName { get; set; }
```

### ✔ camelCase for:
- Private fields (use `_` prefix if desired)
- Local variables
- Parameters

```csharp
private int _itemCount;
void AddItem(int itemCount) {
    var total = _itemCount + itemCount;
}
```

---

## ✅ 2. Layout Conventions

### ✔ Use Allman Style Braces
```csharp
public void PrintMessage()
{
    Console.WriteLine("Hello, world!");
}
```

### ✔ Use one blank line between method definitions

---

## ✅ 3. Spacing Conventions

- Use a single space after keywords and commas:
```csharp
if (condition) { }
var values = new List<int> { 1, 2, 3 };
```

- No space after method names before parentheses:
```csharp
DoWork();
```

---

## ✅ 4. Use `var` when the type is obvious
```csharp
var stream = new FileStream("file.txt", FileMode.Open);
```

Avoid `var` when the type isn't clear:
```csharp
Customer customer = GetCustomer();
```

---

## ✅ 5. Use expression-bodied members when appropriate
```csharp
public int Age => DateTime.Now.Year - birthYear;
```

---

## ✅ 6. Organize `using` statements
- Place `using` statements outside the namespace.
- Sort them alphabetically.
- Group system namespaces first.

```csharp
using System;
using System.Collections.Generic;

namespace MyApp
{
    // Code here
}
```

---

## ✅ 7. File and Folder Organization
- One class per file.
- File name matches class name.

---

## ✅ 8. Avoid Hungarian Notation
❌ `string strName`  
✔ `string name`

---

## ✅ 9. Constants and readonly fields
- Use `PascalCase` for constants:
```csharp
public const int MaxItems = 100;
```

- Use `readonly` for fields set in the constructor:
```csharp
private readonly string _id;
```

---

## 🔗 Resources
- [Official Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)


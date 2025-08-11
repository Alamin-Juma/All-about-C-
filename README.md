
# The Ultimate Hands-On Visual Studio Debugging Course

## 🎯 Course Overview
**Objective:** To transform you into a world-class debugging expert, capable of tackling any bug—compile-time, runtime, logic, performance, memory, or production-related—using Visual Studio and Visual Studio Code. This course is hands-on, guiding you through real-world labs to master debugging tools and techniques.

**Target Audience:** Software engineers (beginner to advanced) who want to slash debugging time, improve code quality, and become indispensable problem-solvers on their teams.

**Prerequisites:**
*   Basic knowledge of C# and .NET.
*   Visual Studio 2022 (Community, Professional, or Enterprise) with the ".NET desktop development" workload.
*   Visual Studio Code (for Lab 13) with the C# extension and .NET SDK installed.
*   A code editor and PDF viewer for generating the final PDF.

## Why Mastering Debugging Matters
Debugging is the backbone of software development. Writing code is only half the job—ensuring it works reliably is the other half. Mastering debugging saves hours of frustration, boosts code quality, and makes you the go-to person for solving tough problems. This course builds systematic debugging skills through practical, real-world scenarios.

## 🧪 Lab 1: Fixing Compile-Time Errors (The "Red Squiggles")
### 🏆 Learning Goal: Resolve compiler errors using Visual Studio’s Error List and inline code analysis before the program runs.

### Step 1: Create the Project and See It Fail
1.  **Create a new project:** In Visual Studio, select **File → New → Project → Console App (.NET)**.
2.  **Name it:** `DebuggingMasteryLab`.
3.  **Add the buggy code:** Replace the default code in `Program.cs` with the code below.
4.  **Attempt to build:** Press `Ctrl+Shift+B`. The build will fail (expected).

**Starting Code:**
```csharp
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace DebuggingMasteryLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var localDB = LoadRecords();
            string data = GetJsonData();
            User[] users = ReadToObject(data);
            UpdateRecords(localDB, users);
            
            for (int i = 0; i < users.Length; i++)
            {
                List<User> result = localDB.FindAll(delegate (User u) {
                    return u.lastname == users[i].lastname;
                });
                foreach (var item in result)
                {
                    Console.WriteLine($"Matching Record, got name={item.firstname}, lastname={item.lastname}, age={item.totalpoints}");
                }
            }
            Console.ReadKey();
        }

        public static User[] ReadToObject(string json)
        {
            User[] users = { };
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(users.GetType());
            users = ser.ReadObject(ms) as User[];
            ms.Close();
            return users;
        }

        public static string GetJsonData()
        {
            string str = "[{ \"points\":4o,\"firstname\":\"Fred\",\"lastname\":\"Smith\"},{\"lastName\":\"Jackson\"}]";
            return str;
        }

        public static List<User> LoadRecords()
        {
            var db = new List<User> { };
            User user1 = new User();
            user1.firstname = "Joe";
            user1.lastname = "Smith";
            user1.totalpoints = 41;
            db.Add(user1);
            User user2 = new User();
            user2.firstname = "Pete";
            user2.lastname = "Peterson";
            user2.totalpoints = 30;
            db.Add(user2);
            return db;
        }

        public static void UpdateRecords(List<User> db, User[] users)
        {
            bool existingUser = false;
            for (int i = 0; i < users.Length; i++)
            {
                foreach (var item in db)
                {
                    if (item.lastname == users[i].lastname && item.firstname == users[i].firstname)
                    {
                        existingUser = true;
                        item.totalpoints += users[i].points;
                    }
                }
                if (existingUser == false)
                {
                    User user = new User();
                    user.firstname = users[i].firstname;
                    user.lastname = users[i].lastname;
                    user.totalpoints = users[i].points;
                    db.Add(user);
                }
            }
        }
    }

    [DataContract]
    internal class User
    {
        [DataMember]
        internal string firstname;
        [DataMember]
        internal string lastname;
        [DataMember]
        internal string points; // BUG: Should be int
        [DataMember]
        internal int totalpoints;
    }
}
```

### Step 2: Use the Error List to Isolate the Problem
1.  **Open the Error List:** Go to **View → Error List** (`Ctrl+\, E`).
2.  **Read the error:** You’ll see: `CS0029: Cannot implicitly convert type 'string' to 'int'` at `item.totalpoints += users[i].points;`.
3.  **Locate the code:** Double-click the error to jump to the problematic line in `UpdateRecords`.

### Step 3: Investigate and Fix the Type Mismatch
1.  **Investigate `totalpoints`:** Right-click `totalpoints` → **Go To Definition** (`F12`). It’s an `int`.
2.  **Investigate `points`:** Check `points`. It’s a `string`.
3.  **Fix:** Change `points` to `int` in the `User` class.

```csharp
// ❌ BEFORE
[DataMember]
internal string points;

// ✅ AFTER
[DataMember]
internal int points;
```

### Step 4: Verify the Fix
1.  **Build again:** Press `Ctrl+Shift+B`.
2.  **Result:** Build Succeeded. Error List shows 0 Errors.

### ✅ Lab 1 Success Criteria
*   Use the Error List to find compile-time issues.
*   Investigate type mismatches using Go To Definition.
*   Build the project successfully.

## 🧪 Lab 2: Debugging Runtime Exceptions
### 🏆 Learning Goal: Catch and fix crashes using breakpoints, the Exception Helper, and defensive coding.

### Step 1: Run the Code and Witness the Crash
1.  **Start Debugging:** Press `F5`.
2.  **Observe:** The app crashes with a `System.Runtime.Serialization.SerializationException` at `users = ser.ReadObject(ms) as User[];` in `ReadToObject`.

### Step 2: Set a Breakpoint to Investigate
1.  **Set a Breakpoint:** In `GetJsonData`, click the left margin next to `string str = ...`.
2.  **Restart Debugging:** Press `F5`. Execution pauses at the breakpoint.

### Step 3: Inspect Variables with the Watch Window
1.  **Step Over:** Press `F10`.
2.  **Inspect `str`:** Hover over `str` or right-click → **Add to Watch**. Notice:
    *   `"points":4o`: Should be `40` (number, not letter 'o').
    *   `"lastName":"Jackson"`: Case mismatch with `lastname` in `User`.
    *   Missing `points` and `firstname` for Jackson.

### Step 4: Fix the Data and Add Exception Handling
**Fix the JSON in `GetJsonData`:**
```csharp
public static string GetJsonData()
{
    string str = "[{\"points\":40,\"firstname\":\"Fred\",\"lastname\":\"Smith\"},{\"points\":25,\"firstname\":\"John\",\"lastname\":\"Jackson\"}]";
    return str;
}
```

**Fix `ReadToObject` and add try-catch:**
```csharp
public static User[] ReadToObject(string json)
{
    User[] users = { };
    try
    {
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(User[]));
        users = ser.ReadObject(ms) as User[];
        ms.Close();
    }
    catch (SerializationException ex)
    {
        Console.WriteLine($"❌ JSON parsing failed: {ex.Message}");
        return new User[] { };
    }
    return users;
}
```

### Step 5: Verify the Fix
1.  **Remove Breakpoints:** **Debug → Delete All Breakpoints** (`Ctrl+Shift+F9`).
2.  **Run:** Press `F5`. The app runs without crashing.

### ✅ Lab 2 Success Criteria
*   Set breakpoints to pause execution.
*   Use Watch window and DataTips to inspect variables.
*   Interpret Exception Helper messages.
*   Implement `try-catch` for robust error handling.

## 🧪 Lab 3: Uncovering Logic Bugs with Step-by-Step Execution
### 🏆 Learning Goal: Fix bugs that produce incorrect results by tracing program flow.

### Step 1: Analyze the Incorrect Output
Run the app (`F5`). Output:
> Matching Record, got name=Joe, lastname=Smith, age=81

**Issue:** John Jackson is missing from the output.

### Step 2: Strategize and Set a Breakpoint
1.  **Set a Breakpoint:** In `UpdateRecords`, on `for (int i = 0; i < users.Length; i++)`.
2.  **Open Locals:** **Debug → Windows → Locals**.

### Step 3: Step Through and Find the Flaw
1.  **Debug** (`F5`). Stop at the breakpoint.
2.  **First Iteration (Fred Smith):**
    *   Step with `F10`. `existingUser` becomes `true`, `totalpoints` updates to 81.
3.  **Second Iteration (John Jackson):**
    *   `existingUser` is still `true` from the previous iteration, so Jackson is not added.

### Step 4: Fix the Variable Scope Bug
Move `existingUser` inside the loop:
```csharp
public static void UpdateRecords(List<User> db, User[] users)
{
    for (int i = 0; i < users.Length; i++)
    {
        bool existingUser = false; // ✅ Moved inside loop
        foreach (var item in db)
        {
            if (item.lastname == users[i].lastname && item.firstname == users[i].firstname)
            {
                existingUser = true;
                item.totalpoints += users[i].points;
            }
        }
        if (!existingUser)
        {
            User user = new User();
            user.firstname = users[i].firstname;
            user.lastname = users[i].lastname;
            user.totalpoints = users[i].points;
            db.Add(user);
        }
    }
}
```

### Step 5: Verify the Fix
1.  **Debug again**, stepping through to confirm `existingUser` resets.
2.  **Run** (`F5`). Output:
> Matching Record, got name=Joe, lastname=Smith, age=81
> Matching Record, got name=John, lastname=Jackson, age=25

### ✅ Lab 3 Success Criteria
*   Use `F10` to trace logic.
*   Monitor variables in the Locals window.
*   Fix a logic bug caused by incorrect variable scope.

## 🧪 Lab 4: Mastering Advanced Debugging Tools
### 🏆 Learning Goal: Use Conditional Breakpoints, Call Stack, Immediate Window, and Edit and Continue for efficient debugging.

### Step 1: Use a Conditional Breakpoint
1.  **Set a Conditional Breakpoint:**
    *   In `UpdateRecords`, on `if (item.lastname == users[i].lastname && ...)`.
    *   Right-click the breakpoint → **Conditions** → `users[i].lastname == "Smith"`.
2.  **Debug** (`F5`). Execution stops only for "Smith".

### Step 2: Navigate the Call Stack
1.  Stop at the breakpoint.
2.  **Open Call Stack:** **Debug → Windows → Call Stack**.
3.  **Navigate:** Double-click `Main` to jump to the caller, then back to `UpdateRecords`.

### Step 3: Use the Immediate Window
1.  **Open Immediate Window:** **Debug → Windows → Immediate**.
2.  **Execute:**
    *   `? db.Count` → Shows the list size.
    *   `? users[i].firstname` → Shows "Fred".
    *   `item.totalpoints = 999;` → Modifies the state.
3.  **Step** (`F10`) to confirm `totalpoints` is `999`.

### Step 4: Use Edit and Continue
1.  At the breakpoint, change `item.totalpoints += users[i].points` to `item.totalpoints *= users[i].points`.
2.  **Resume** (`F5`). The change applies without restarting.

### ✅ Lab 4 Success Criteria
*   Create conditional breakpoints.
*   Navigate the Call Stack.
*   Use the Immediate Window to modify state.
*   Apply fixes with Edit and Continue.

## 🧪 Lab 5: Analyzing Performance and Memory
### 🏆 Learning Goal: Use Visual Studio’s Diagnostic Tools to fix performance bottlenecks and memory issues.

### Step 1: Introduce a Performance Bottleneck
**Modify `LoadRecords`:**
```csharp
public static List<User> LoadRecords()
{
    var db = new List<User>();
    Console.WriteLine("Loading a large database...");
    for (int i = 0; i < 50000; i++)
    {
        db.Add(new User { firstname = $"User{i}", lastname = $"LastName{i % 1000}", totalpoints = i });
    }
    db.Add(new User { firstname = "Joe", lastname = "Smith", totalpoints = 41 });
    Console.WriteLine($"Database loaded with {db.Count} records.");
    return db;
}
```

**Modify `UpdateRecords`:**
```csharp
public static void UpdateRecords(List<User> db, User[] users)
{
    for (int i = 0; i < users.Length; i++)
    {
        bool existingUser = false;
        foreach (var item in db) // Inefficient O(n*m)
        {
            if (item.firstname == "Joe" && item.lastname == "Smith" && users[i].lastname == "Smith")
            {
                existingUser = true;
                item.totalpoints += users[i].points;
            }
        }
        if (!existingUser)
        {
            User user = new User();
            user.firstname = users[i].firstname;
            user.lastname = users[i].lastname;
            user.totalpoints = users[i].points;
            db.Add(user);
        }
    }
}
```

**Add `Stopwatch` to `Main`:**
```csharp
static void Main(string[] args)
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    var localDB = LoadRecords();
    string data = GetJsonData();
    User[] users = ReadToObject(data);
    UpdateRecords(localDB, users);
    for (int i = 0; i < users.Length; i++)
    {
        List<User> result = localDB.FindAll(u => u.lastname == users[i].lastname);
        foreach (var item in result)
        {
            Console.WriteLine($"Matching Record, got name={item.firstname}, lastname={item.lastname}, age={item.totalpoints}");
        }
    }
    stopwatch.Stop();
    Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds}ms");
    Console.ReadKey();
}
```

### Step 2: Profile CPU Usage
1.  **Run without debugging** (`Ctrl+F5`). Note the slow execution.
2.  **Open Performance Profiler:** **Debug → Performance Profiler** (`Alt+F2`).
3.  **Select CPU Usage** and run. The report shows `UpdateRecords` as the hot path.

### Step 3: Fix the Performance
Use a `Dictionary`:
```csharp
using System.Linq;

public static void UpdateRecords(List<User> db, User[] users)
{
    var dbLookup = db.ToDictionary(u => $"{u.firstname}|{u.lastname}");
    Console.WriteLine("Updating records with optimized method...");
    for (int i = 0; i < users.Length; i++)
    {
        string key = $"{users[i].firstname}|{users[i].lastname}";
        if (dbLookup.TryGetValue(key, out User userToUpdate))
        {
            userToUpdate.totalpoints += users[i].points;
        }
        else
        {
            User user = new User();
            user.firstname = users[i].firstname;
            user.lastname = users[i].lastname;
            user.totalpoints = users[i].points;
            db.Add(user);
        }
    }
}
```

### Step 4: Verify the Improvement
1.  Run the profiler again. Execution time drops significantly.
2.  Run (`Ctrl+F5`). `Stopwatch` confirms the improvement.

### ✅ Lab 5 Success Criteria
*   Use the CPU Usage profiler to identify bottlenecks.
*   Optimize an O(n²) algorithm to O(n) using a Dictionary.
*   Measure performance improvements.

## 🧪 Lab 6: Debugging Async/Await Issues
### 🏆 Learning Goal: Debug asynchronous code to resolve deadlocks and task issues.

### Step 1: Introduce an Async Bug
Add an async method to `Program.cs`:
```csharp
public static async Task<string> GetDataAsync()
{
    await Task.Delay(1000); // Simulate async work
    return "[{\"points\":40,\"firstname\":\"Fred\",\"lastname\":\"Smith\"},{\"points\":25,\"firstname\":\"John\",\"lastname\":\"Jackson\"}]";
}
```

Modify `Main` to use it with a deadlock:
```csharp
static void Main(string[] args)
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    var localDB = LoadRecords();
    string data = GetDataAsync().Result; // BUG: Deadlock
    User[] users = ReadToObject(data);
    UpdateRecords(localDB, users);
    for (int i = 0; i < users.Length; i++)
    {
        List<User> result = localDB.FindAll(u => u.lastname == users[i].lastname);
        foreach (var item in result)
        {
            Console.WriteLine($"Matching Record, got name={item.firstname}, lastname={item.lastname}, age={item.totalpoints}");
        }
    }
    stopwatch.Stop();
    Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds}ms");
    Console.ReadKey();
}
```

### Step 2: Debug the Deadlock
1.  **Set a Breakpoint:** In `GetDataAsync`, on `await Task.Delay(1000);`.
2.  **Open Tasks Window:** **Debug → Windows → Tasks**.
3.  **Debug** (`F5`). Execution hangs at `.Result`.
4.  **Inspect Tasks:** The Tasks window shows `GetDataAsync` as "Waiting" due to the deadlock.

### Step 3: Fix the Deadlock
Make `Main` async:
```csharp
static async Task Main(string[] args)
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    var localDB = LoadRecords();
    string data = await GetDataAsync(); // ✅ Use await
    User[] users = ReadToObject(data);
    UpdateRecords(localDB, users);
    for (int i = 0; i < users.Length; i++)
    {
        List<User> result = localDB.FindAll(u => u.lastname == users[i].lastname);
        foreach (var item in result)
        {
            Console.WriteLine($"Matching Record, got name={item.firstname}, lastname={item.lastname}, age={item.totalpoints}");
        }
    }
    stopwatch.Stop();
    Console.WriteLine($"\nTotal execution time: {stopwatch.ElapsedMilliseconds}ms");
    Console.ReadKey();
}
```

### Step 4: Verify the Fix
1.  **Debug again**. The Tasks window shows the task completing.
2.  **Run** (`F5`). The app runs without hanging.

### ✅ Lab 6 Success Criteria
*   Use the Tasks window to inspect async task states.
*   Identify and fix an async deadlock.
*   Understand async/await best practices.

## 🧪 Lab 7: Using Unit Tests to Isolate and Prevent Bugs
### 🏆 Learning Goal: Use unit tests to reproduce bugs and verify fixes.

### Step 1: Create a Test Project
1.  **Add a Test Project:** **File → New → Project → Unit Test Project (.NET)** → Name it `DebuggingMasteryLab.Tests`.
2.  **Reference the main project:** Right-click `DebuggingMasteryLab.Tests` → **Add → Project Reference** → Select `DebuggingMasteryLab`.

### Step 2: Write a Failing Test
Add a test in `UnitTest1.cs`:
```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebuggingMasteryLab;
using System.Collections.Generic;

[TestClass]
public class UpdateRecordsTests
{
    [TestMethod]
    public void UpdateRecords_ExistingUser_UpdatesPoints()
    {
        var db = new List<User> { new User { firstname = "Joe", lastname = "Smith", totalpoints = 41 } };
        var users = new[] { new User { firstname = "Joe", lastname = "Smith", points = 40 } };
        
        Program.UpdateRecords(db, users);
        
        Assert.AreEqual(81, db.totalpoints);
    }
}
```

### Step 3: Debug the Test
1.  **Open Test Explorer:** **Test → Test Explorer**.
2.  **Run the test**. It passes (assuming Lab 3 is fixed).
3.  **Introduce a bug** in `UpdateRecords` (revert `existingUser` to outside the loop) and re-run. The test fails.
4.  **Debug the test:** Right-click the test → **Debug**. Use breakpoints to diagnose.

### Step 4: Fix and Verify
Revert the fix from Lab 3. The test passes again.

### ✅ Lab 7 Success Criteria
*   Create and run unit tests in Visual Studio.
*   Debug a failing test.
*   Use tests to verify bug fixes.

## 🧪 Lab 8: Diagnosing Memory Issues
### 🏆 Learning Goal: Identify and fix memory leaks using Diagnostic Tools.

### Step 1: Introduce a Memory Leak
Add a static cache:
```csharp
public static class MemoryLeakSimulator
{
    private static List<User> _cache = new List<User>();
    
    public static List<User> LoadRecords()
    {
        var db = new List<User>();
        for (int i = 0; i < 10000; i++)
        {
            var user = new User { firstname = $"User{i}", lastname = $"LastName{i}", totalpoints = i };
            db.Add(user);
            _cache.Add(user); // BUG: Memory leak
        }
        db.Add(new User { firstname = "Joe", lastname = "Smith", totalpoints = 41 });
        return db;
    }
}
```
Update `LoadRecords` call in `Main` to use `MemoryLeakSimulator.LoadRecords()`.

### Step 2: Profile Memory
1.  **Run with Diagnostic Tools:** **Debug → Windows → Show Diagnostic Tools**.
2.  **Take snapshots:** Before and after `LoadRecords`.
3.  **Analyze Heap:** The Heap view shows `User` objects retained by `_cache`.

### Step 3: Fix the Leak
Remove the cache or clear it:
```csharp
public static class MemoryLeakSimulator
{
    private static List<User> _cache = new List<User>();
    
    public static List<User> LoadRecords()
    {
        var db = new List<User>();
        for (int i = 0; i < 10000; i++)
        {
            var user = new User { firstname = $"User{i}", lastname = $"LastName{i}", totalpoints = i };
            db.Add(user);
        }
        db.Add(new User { firstname = "Joe", lastname = "Smith", totalpoints = 41 });
        _cache.Clear(); // ✅ Clear cache to prevent leak
        return db;
    }
}
```

### Step 4: Verify
Run again with Diagnostic Tools. The Heap shows no retained objects.

### ✅ Lab 8 Success Criteria
*   Use Diagnostic Tools to identify memory leaks.
*   Analyze the Heap to find retained objects.
*   Fix a memory leak.

## 🧪 Lab 9: Remote Debugging and Production Scenarios
### 🏆 Learning Goal: Debug a running process or crash dump in production-like scenarios.

### Step 1: Simulate a Production Issue
1.  **Build in Release mode:** **Build → Configuration Manager** → Set to `Release`.
2.  **Run the executable:** Navigate to `bin/Release/netcoreappX.X/DebuggingMasteryLab.exe` and run it.

### Step 2: Attach to Process
1.  **Attach Debugger:** **Debug → Attach to Process** → Select `DebuggingMasteryLab.exe`.
2.  **Set Breakpoints:** In `UpdateRecords` to inspect behavior.

### Step 3: Analyze a Crash Dump
1.  **Generate a dump:** Run the app, open Task Manager, right-click the process → **Create dump file**.
2.  **Open in Visual Studio:** **File → Open** → Select the `.dmp` file.
3.  **Analyze:** Use the **Debug Managed Memory** feature to inspect variables and call stack.

### Step 4: Verify
Debug the process and confirm breakpoints hit correctly.

### ✅ Lab 9 Success Criteria
*   Attach to a running process.
*   Analyze a crash dump.
*   Debug production-like scenarios.

## 🧪 Lab 10: Debugging Multi-Threaded Code
### 🏆 Learning Goal: Debug race conditions in multi-threaded applications.

### Step 1: Introduce a Race Condition
Modify `UpdateRecords`:
```csharp
using System.Threading.Tasks;
using System.Linq;

public static void UpdateRecords(List<User> db, User[] users)
{
    Parallel.ForEach(users, user =>
    {
        var match = db.FirstOrDefault(u => u.lastname == user.lastname);
        if (match != null)
        {
            match.totalpoints += user.points; // BUG: Race condition
        }
    });
}
```

### Step 2: Debug the Race Condition
1.  **Set a Breakpoint:** Inside the `Parallel.ForEach` loop.
2.  **Open Threads Window:** **Debug → Windows → Threads**.
3.  **Debug** (`F5`). Switch between threads to observe inconsistent `totalpoints`.

### Step 3: Fix the Race Condition
Add a `lock`:
```csharp
public static void UpdateRecords(List<User> db, User[] users)
{
    var lockObj = new object();
    Parallel.ForEach(users, user =>
    {
        lock (lockObj)
        {
            var match = db.FirstOrDefault(u => u.lastname == user.lastname);
            if (match != null)
            {
                match.totalpoints += user.points; // ✅ Thread-safe
            }
        }
    });
}
```

### Step 4: Verify
1.  Debug again. The Threads window shows synchronized execution.
2.  Run (`F5`). Output is consistent.

### ✅ Lab 10 Success Criteria
*   Use the Threads window to inspect multi-threaded execution.
*   Identify and fix a race condition.
*   Understand thread synchronization.

## 🧪 Lab 11: Debugging Third-Party Dependencies
### 🏆 Learning Goal: Debug issues in third-party libraries using symbols.

### Step 1: Introduce a Dependency Bug
1.  **Add Newtonsoft.Json:** Right-click `DebuggingMasteryLab` → **Manage NuGet Packages** → Install `Newtonsoft.Json`.
2.  Modify `ReadToObject`:
```csharp
using Newtonsoft.Json;

public static User[] ReadToObject(string json)
{
    // BUG: Missing attributes for correct deserialization by property name
    return JsonConvert.DeserializeObject<User[]>(json); 
}
```

### Step 2: Debug with Symbols
1.  **Enable Symbol Loading:** **Tools → Options → Debugging → Symbols** → Enable **Microsoft Symbol Servers**.
2.  **Set a Breakpoint:** In `ReadToObject`.
3.  **Step Into** (`F11`) `JsonConvert` to inspect deserialization.
4.  **Identify:** Missing `JsonProperty` attributes.

### Step 3: Fix the Bug
Add attributes to `User`:
```csharp
using Newtonsoft.Json;

// Removed [DataContract] and [DataMember] as we now use Newtonsoft
internal class User
{
    [JsonProperty("firstname")]
    internal string firstname;
    [JsonProperty("lastname")]
    internal string lastname;
    [JsonProperty("points")]
    internal int points;
    // This property does not exist in the source JSON, so it won't be populated by default
    internal int totalpoints; 
}
```

### Step 4: Verify
Run (`F5`). The JSON deserializes correctly.

### ✅ Lab 11 Success Criteria
*   Debug third-party code with symbols.
*   Fix deserialization issues.
*   Configure symbol loading.

## 🧪 Lab 12: Preventing Bugs with Static Analysis
### 🏆 Learning Goal: Use static analysis to catch potential bugs early.

### Step 1: Enable Code Analysis
1.  **Configure Rules:** **Tools → Options → Text Editor → C# → Code Style** → Enable various code analysis rules (e.g., CA1822 for unused members).
2.  **Add a buggy method:**
```csharp
public static void UnusedMethod()
{
    int x = 0; // Warning: Unused variable
    Console.WriteLine("Never called");
}
```

### Step 2: Run Code Analysis
1.  **Analyze:** **Analyze → Run Code Analysis on Solution**.
2.  **Review Warnings:** The Code Analysis window shows CA1822 for `UnusedMethod`.

### Step 3: Fix the Issues
Remove unused code or suppress warnings with `[SuppressMessage]` if justified.

### Step 4: Verify
Re-run analysis. No warnings remain.

### ✅ Lab 12 Success Criteria
*   Configure and run code analysis.
*   Fix static analysis warnings.
*   Understand code quality tools.

## 🧪 Lab 13: Cross-Platform Debugging with Visual Studio Code
### 🏆 Learning Goal: Debug .NET apps in Visual Studio Code.

### Step 1: Set Up the Project
1.  **Create a .NET Core project:** In the VS Code terminal, run `dotnet new console -n DebuggingMasteryLab`.
2.  **Copy `Program.cs`** from the Visual Studio project.
3.  **Install C# Extension:** In VS Code, install the C# extension from Microsoft.

### Step 2: Configure Debugging
1.  **Create `launch.json`:** Open the **Run and Debug** view → click **create a launch.json file** → Select **.NET 5+ and .NET Core**.
2.  **Set a Breakpoint:** In `GetJsonData` on the JSON string.

### Step 3: Debug the JSON Bug
Use the original buggy JSON:
```csharp
public static string GetJsonData()
{
    string str = "[{ \"points\":4o,\"firstname\":\"Fred\",\"lastname\":\"Smith\"},{\"lastName\":\"Jackson\"}]";
    return str;
}
```
**Debug:** Run the debugger (`F5`). Inspect `str` and fix as in Lab 2.

### Step 4: Verify
Run (`F5`). The app works correctly in VS Code.

### ✅ Lab 13 Success Criteria
*   Configure debugging in VS Code.
*   Debug a .NET app in a cross-platform environment.
*   Understand VS Code debugging capabilities.

## 🧪 Lab 14: Challenge Mode - Fix a Mystery Bug
### 🏆 Learning Goal: Apply all learned skills to fix a complex bug without step-by-step guidance.

### Step 1: Introduce the Bug
Add a buggy method:
```csharp
public static void ProcessComplexData(List<User> db)
{
    var users = ReadToObject(GetJsonData());
    Parallel.ForEach(users, user =>
    {
        var match = db.FirstOrDefault(u => u.lastname == user.lastname);
        if (match != null)
        {
            match.totalpoints += user.points / 0; // BUG: Division by zero
        }
    });
}
```
Call it from `Main` before `Console.ReadKey()`.

### Step 2: Debug Freestyle
1.  **Run** (`F5`). The app crashes with a `DivideByZeroException`.
2.  Use any tools (breakpoints, Exception Helper, Threads window, etc.) to diagnose and fix.

### Step 3: Fix and Verify
**Fix:** Replace `/ 0` with a valid operation (e.g., `+= user.points`).
Run. Confirm the output is correct.

### ✅ Lab 14 Success Criteria
*   Independently diagnose and fix a complex bug.
*   Combine multiple debugging tools effectively.

## Final Thoughts and Next Steps
### Debugging Best Practices
*   **Reproduce Consistently:** Always ensure you can trigger the bug reliably.
*   **Divide and Conquer:** Use breakpoints to isolate the bug’s location.
*   **Verify Assumptions:** Use DataTips, Watch, or Immediate Window to check variable states.
*   **Read Errors:** Exception messages and stack traces are critical clues.
*   **Write Tests:** Unit tests prevent regressions and help isolate issues.
*   **Profile Early:** Use performance and memory tools to catch inefficiencies before they become problems.

### Where to Go From Here
*   **Advanced Async:** Explore debugging complex async workflows with `Task.WhenAll`.
*   **Memory Dumps:** Learn to analyze production crash dumps with WinDbg.
*   **Cloud Debugging:** Use Azure Application Insights for distributed systems.
*   **Community Resources:** Join forums like Stack Overflow or the Visual Studio Community to share debugging tips.

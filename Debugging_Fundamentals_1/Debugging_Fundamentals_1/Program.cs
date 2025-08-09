
class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Debugging Demo!");

        // Debug/Release mode
#if DEBUG
        Console.WriteLine("Debug Mode");
#else
            Console.WriteLine("Release Mode");
#endif

        // Breakpoints
        Console.WriteLine("Setting breakpoints ...");

        // Step into, over, and out
        StepIntoMethod();
        StepOverMethod();
        StepOutMethod();

        // New section added here
        // Watches
        int watchDegoValue = 10;

        // Immediate - its like the console for backend as console is for frontend
        //use to check something that is on scope , like to have a console like expereince 
        int immediateDemoValue = 20;

        // Inspecting variables
        int inspectDemoValue = 30;

        // Autos and Locals
        int autosAndLocalsDemoValue = 40;

        // Call Stack
        //step into it then 
        //step over the method twice to execute it 
        //step into the method 
        //run it 
        CallStackDemo();

        // Conditional breakpoints
        int conditionDemoValue = 5;
        for (int i = 0; i < 10; i++)
        {
            conditionDemoValue *= 2; // set a break point here 

        }


        Console.WriteLine("Debugging Demo completed!");
    }

    static void StepIntoMethod()
    {
        Console.WriteLine("Stepping into this method...");
    }

    static void StepOverMethod()
    {
        Console.WriteLine("Stepping over this method...");
    }

    static void StepOutMethod()
    {
        Console.WriteLine("Stepping out of this method...");
    }

    static void CallStackDemo()
    {
        Console.WriteLine("Demonstrating call stack...");

        CallStackSubMethod();
    }

    private static void CallStackSubMethod()
    {
        Console.WriteLine("Inside call stack sub method...");
        // You can set a breakpoint here to see the call stack in action
    }
}
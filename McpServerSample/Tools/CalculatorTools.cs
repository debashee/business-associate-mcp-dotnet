using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpServerSample.Tools;

[McpServerToolType]
public class CalculatorTools
{
    [McpServerTool, Description("Adds two numbers together")]
    public double Add(
        [Description("First number")]
        double a,
        [Description("Second number")]
        double b)
    {
        return a + b;
    }

    // [McpServerTool, Description("Subtracts the second number from the first")]
    // public double Subtract(
    //     [Description("First number")]
    //     double a,
    //     [Description("Second number")]
    //     double b)
    // {
    //     return a - b;
    // }

    // [McpServerTool, Description("Multiplies two numbers")]
    // public double Multiply(
    //     [Description("First number")]
    //     double a,
    //     [Description("Second number")]
    //     double b)
    // {
    //     return a * b;
    // }

    // [McpServerTool, Description("Divides the first number by the second")]
    // public double Divide(
    //     [Description("Numerator")]
    //     double a,
    //     [Description("Denominator")]
    //     double b)
    // {
    //     if (b == 0)
    //     {
    //         throw new DivideByZeroException("Cannot divide by zero");
    //     }
    //     return a / b;
    // }
}

namespace HelloDotnet;

public static class Greeting
{
    public static string Greet(string name)
    {
        return $"Hello, {name}!";
    }

    public static int SumRange(int from, int to)
    {
        int sum = 0;
        for (int i = from; i <= to; i++)
        {
            sum += i;
        }
        return sum;
    }

    public static long Fibonacci(int n)
    {
        long a = 0;
        long b = 1;
        for (int i = 0; i < n; i++)
        {
            long next = a + b;
            a = b;
            b = next;
        }
        return a;
    }
}

using System;
using System.Numerics;
using static System.Double;

public static class Util
{
    private static readonly string[] UnitSuffixes = {"", " K", " M", " G", " T", " P", " E", " Z", " Y"};
    
    public static string FormatLargeNumber(BigInteger n)
    {
        var exp = (int) Math.Floor(BigInteger.Log10(n) + Epsilon);
        var thousands = exp / 3;
        if (thousands >= UnitSuffixes.Length)
        {
            var significant = n / BigInteger.Pow(10,  exp);
            return $"{significant}E{exp}";
        }
        else
        {
            var significant = n / BigInteger.Pow(10,  3 * thousands);
            String suffix = UnitSuffixes[thousands];
            return $"{significant}{suffix}";
        }
    }
}
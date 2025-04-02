// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[] sapa_Color = {
          "AC36.208",
"R8017.740",
"AC0.208",
"N8000.740N",
"AC36.208",
"MF",
"MF",
"YB105.240",
"YB105.240",
"MF",
"AC36.208",
"AC36.208",
"AC36.208",
"AC0.208",
"N0510.840U",
"R8017.740",
"R8017.740",
"R8017.740",
"R3018.740 | YB105.240",
"R3018.740 | YB105.240",
"YB105.240 | R3018.740",
"R3018.740 | YB105.240",
"R3018.740 | YB105.240",
"AC0.208",
"R8506.340",
"XBLACK",
"R8506.340 | N8010.840F",
"N8010.840F | R8506.340",
"R8506.340 | N8010.840F",
"R8506.340 | N8010.840F",
"N8010.840F | R8506.340",
"N8010.840F | R8506.340",
"N8010.840F | R8506.340",
"R8506.340",
"N8010.840F",
"N8010.840F",
"R8506.340",
"N8010.840F",
"N8010.840F",
"R8506.340",
"MF"
        };

        List<string> sapa_Color_trimmed = [];

        foreach (string color in sapa_Color)
        {
            if (color.Contains("|"))
            {
                IEnumerable<string> parts = color.Split('|').Select(part => part.Trim());
                IEnumerable<string> cleaned = parts.Select(ShortenCode);
                sapa_Color_trimmed.Add(string.Join("|", cleaned));
            }
            else
            {
                sapa_Color_trimmed.Add(ShortenCode(color));
            }
        }

        foreach (string val in sapa_Color_trimmed)
        {
            Console.WriteLine(val);
        }
    }

    static string ShortenCode(string input)
    {
        // Match LDDDD.DD0L  → keep .DD
        if (Regex.IsMatch(input, @"^[A-Z]\d{4}\.\d{2}0[A-Z]$"))
        {
            return input.Substring(0, input.Length - 2); // remove 0 and final letter
        }

        // Match LDDDD.DD0 → keep .DD
        if (Regex.IsMatch(input, @"^[A-Z]\d{4}\.\d{2}0$"))
        {
            return input.Substring(0, input.Length - 1); // remove last digit
        }

        return input; // unchanged
    }
}


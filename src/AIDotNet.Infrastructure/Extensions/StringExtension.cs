using System.Text;

namespace System;

/// <summary>
/// 提供对字符串的常用扩展方法
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 判断字符串是否为null或空
    /// </summary>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 判断字符串是否不为null或空
    /// </summary>
    public static bool IsNotNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 判断字符串是否为null或空白
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 判断字符串是否不为null或空白
    /// </summary>
    public static bool IsNotNullOrWhiteSpace(this string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 将字符串转换为驼峰命名法
    /// </summary>
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return char.ToLowerInvariant(str[0]) + str[1..];
    }

    /// <summary>
    /// 将字符串转换为帕斯卡命名法
    /// </summary>
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return char.ToUpperInvariant(str[0]) + str[1..];
    }

    /// <summary>
    /// 将字符串转换为蛇形命名法
    /// </summary>
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(str[i]));
            }
            else
            {
                sb.Append(str[i]);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 将字符串转换为短横线命名法
    /// </summary>
    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                sb.Append('-');
                sb.Append(char.ToLowerInvariant(str[i]));
            }
            else
            {
                sb.Append(str[i]);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 去除字符串开头的指定字符串
    /// </summary>
    public static string TrimStart(this string str, string trimString)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.StartsWith(trimString))
        {
            return str[trimString.Length..];
        }

        return str;
    }

    /// <summary>
    /// 去除字符串末尾的指定字符串
    /// </summary>
    public static string TrimEnd(this string str, string trimString)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.EndsWith(trimString))
        {
            return str[..^trimString.Length];
        }

        return str;
    }

    /// <summary>
    /// 去除字符串两端的指定字符串
    /// </summary>
    public static string Trim(this string str, string trimString)
    {
        return str.TrimStart(trimString).TrimEnd(trimString);
    }

    /// <summary>
    /// 去除字符串开头的指定字符
    /// </summary>
    public static string TrimStart(this string str, char trimChar)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str[0] == trimChar)
        {
            return str[1..];
        }

        return str;
    }

    /// <summary>
    /// 去除字符串末尾的指定字符
    /// </summary>
    public static string TrimEnd(this string str, char trimChar)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str[^1] == trimChar)
        {
            return str[..^1];
        }

        return str;
    }

    /// <summary>
    /// 去除字符串两端的指定字符
    /// </summary>
    public static string Trim(this string str, char trimChar)
    {
        return str.TrimStart(trimChar).TrimEnd(trimChar);
    }

    /// <summary>
    /// 去除字符串开头的空白字符
    /// </summary>
    public static string TrimStart(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var i = 0;
        while (i < str.Length && char.IsWhiteSpace(str[i]))
        {
            i++;
        }

        return str[i..];
    }

    /// <summary>
    /// 去除字符串末尾的空白字符
    /// </summary>
    public static string TrimEnd(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var i = str.Length - 1;
        while (i >= 0 && char.IsWhiteSpace(str[i]))
        {
            i--;
        }

        return str[..(i + 1)];
    }

    /// <summary>
    /// 去除字符串两端的空白字符
    /// </summary>
    public static string Trim(this string str)
    {
        return str.TrimStart().TrimEnd();
    }

    /// <summary>
    /// 从字符串中移除指定的子字符串
    /// </summary>
    public static string Remove(this string str, string removeString)
    {
        return str.Replace(removeString, string.Empty);
    }

    /// <summary>
    /// 从字符串中移除指定的字符
    /// </summary>
    public static string Remove(this string str, char removeChar)
    {
        return str.Replace(removeChar.ToString(), string.Empty);
    }
}
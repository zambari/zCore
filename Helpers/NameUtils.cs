using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v .02 
    // v .03 syntax tweak
    // remove all extension
    // b .03 renamed s to name
    public static class NameUtils
    {
        public static readonly char defaultSeparator = (char)8198; // ' '; //set to something invisle
        public static readonly char defaultPreSeparator = (char)2063;
        /// some other space chars =2063, 8198, 8201, 8202 , 8287
        /// 
        /// 
        /// 9312 = ①
        /// 9332 = ⑴
        /// 9352 = ⒈
        /// 9372 = ⒜
        ///  9461 = ⓵
        /// char 9424 = ⓐ

        public enum UnicodeNumberType { circled, smalCircled, tallBrackets, elegant, letters, lettersInCircles }
        public enum NamingConvention { squaredLabel, hook, hexBrackets, oval, smallCurly, triLabel, doubleTri, doubleRound, fatSquare, curlyLabel, none } // BTNSquaredLabel, BTNTriLabel, BTNCurlyLabel,
   //   ⸨ ⸩ ⧘❝ ❞ ﹛ ﹜ ﹝ ﹞ 「 」 〈 〉 《 》  】 〔 〕 ⦗⦘
        public static string OpeningBracket(this NamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case NamingConvention.squaredLabel:
                    return "[";
                case NamingConvention.oval:
                    return "⸦ ";
                case NamingConvention.hook:
                    return "「 ";
                case NamingConvention.smallCurly:
                    return "﹛ ";
                case NamingConvention.hexBrackets:
                    return "〔 ";
                case NamingConvention.triLabel:
                    return "<";
                case NamingConvention.curlyLabel:
                    return "{";
                case NamingConvention.doubleTri:
                    return "《 ";
                case NamingConvention.fatSquare:
                    return "【 ";
                case NamingConvention.doubleRound:
                    return "⸨";
                // case NamingConvention.BTNCurlyLabel:
                //     return "{" ;
                default:
                    return "";
            }
        }
     
        public static string ClosingBracket(this NamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case NamingConvention.squaredLabel:
                    return "]";
                case NamingConvention.oval:
                    return " ⸧";
                case NamingConvention.hook:
                    return " 」";
                case NamingConvention.smallCurly:
                    return " ﹜";
                case NamingConvention.hexBrackets:
                    return " 〕";

                case NamingConvention.triLabel:
                    return ">";
                case NamingConvention.curlyLabel:
                    return "}";
                case NamingConvention.doubleTri:
                    return " 》";
                case NamingConvention.fatSquare:
                    return " 】";
                case NamingConvention.doubleRound:
                    return " ⸩";
                // case NamingConvention.BTNCurlyLabel:
                //     return "}";
                default:
                    return "";

            }
        }

        public static int GetStartChar(UnicodeNumberType code = UnicodeNumberType.circled)
        {
            switch (code)
            {
                default:
                case UnicodeNumberType.smalCircled:
                    return 9312;
                case UnicodeNumberType.tallBrackets:
                    return 9332;
                case UnicodeNumberType.elegant:
                    return 9352;
                case UnicodeNumberType.circled:
                    return 9461;
                case UnicodeNumberType.letters:
                    return 9372;
                case UnicodeNumberType.lettersInCircles:
                    return 9424;
            }
        }

        public static char GetChar(UnicodeNumberType code, int offset)
        {
            return (char)(GetStartChar(code) + offset);
        }
        public static bool HasTag(this string name, char separator = '$')
        {
            if (name == null) return false;
            return name.IndexOf(CheckSeparator(separator)) >= 0;
        }
        public static string RemoveTag(this string name, char separator = '$')
        {
            if (name == null) return name;
            separator = CheckSeparator(separator);
            int pos = name.IndexOf(separator);
            if (pos >= 0) return name.Substring(0, pos);
            return name;
        }
        public static string GetTag(this string name, char separator = '$')
        {
            int pos = name.IndexOf(CheckSeparator(separator));
            if (pos > 0) return name.Substring(pos + 1);
            return null;
        }

        // public static string AddTag(this string s, string tagString, char separator = '$')
        // {
        //     separator = CheckSeparator(separator);
        //     //   if (s == null) return separator + tagString;
        //     int pos = s.IndexOf(separator);
        //     if (pos < 0)
        //         s += separator;
        //     s += tagString;

        //     return s;
        // }
        public static string SetTag(this string name, string tagString, char separator = '$')
        {
            return Tag(name, tagString, separator);
        }
        public static string Tag(this string name, string tagString, char separator = '$')
        {
            separator = CheckSeparator(separator);
            // if (s == null) return s.AddTag(tagString, separator);
            int pos = name.IndexOf(separator);
            if (pos < 0)
                name += separator;
            else
                name = name.Substring(0, pos + 1);
            name += tagString;
            return name;
        }

        static char CheckSeparator(char sep)
        {
            if (sep == '$')
                return defaultSeparator;
            else return sep;
        }

        public static bool HasPreTag(this string name, char separator = '$')
        {
            if (name == null) return false;
            separator = CheckPreSeparator(separator);

            return name.IndexOf(separator) >= 0;
        }
        public static string RemoveAllTags(this string name, char separator = '$')
        {
            return name.RemoveTag().RemovePreTag();
        }
        public static string RemovePreTag(this string name, char separator = '$')
        {
            if (name == null) return name;
            int pos = name.IndexOf(CheckPreSeparator(separator));
            if (pos >= 0) return name.Substring(pos + 1);
            return name;
        }
        public static string GetPreTag(this string name, char separator = '$')
        {
            int pos = name.IndexOf(CheckPreSeparator(separator));
            if (pos >= 0) return name.Substring(0, pos);
            return null;
        }

        // public static string AddPreTag (this string name, string tagString, char separator = '$') {
        //     separator = CheckPreSeparator (separator);
        //     int pos = name.IndexOf (separator);
        //     if (pos < 0) // no tag
        //         name = tagString + separator + name;
        //     else
        //         name = tagString + name; //Substring(pos);

        //     return name;
        // }
        public static string SetPreTag(this string name, string tagString, char separator = '$')
        {
            separator = CheckPreSeparator(separator);
            name = name.RemovePreTag(separator);
            name = tagString + separator + name;
            return name;
        }

        static char CheckPreSeparator(char sep)
        {
            if (sep == '$')
                return defaultPreSeparator;
            else return sep;
        }
        public static void RemoveAllTags(this GameObject game)
        {
            string n = game.name;
            n = n.RemovePreTag();
            n = n.RemoveTag();
            game.name = n;
        }
        public static void SetTag(this GameObject game, string tag)
        {
            game.name = game.name.SetTag(tag);
        }
        public static void SetPreTag(this GameObject game, string tag)
        {
            game.name = game.name.SetPreTag(tag);
        }
        public static string ToFunnyNumber(this int val, UnicodeNumberType type = UnicodeNumberType.tallBrackets)
        {
            if (val < 1 || val > 20) return val.ToString();
            return ((char)(val + GetStartChar(type) - 1)).ToString();
        }
        // static string ToFunnyUnicodeLetters(this int num, int funnyStart)
        // {
        //     string funnyString = num < 0 ? "-" : "";
        //     if (num < 0) num = -num;
        //     string asString = num.ToString();

        //     for (int i = 0; i < asString.Length; i++)
        //     {
        //         char thisChar = asString[i];

        //         int thicChar = thisChar - 48;
        //         funnyString += (char)(thicChar + funnyStart);
        //     }
        //     return funnyString;
        // }
        // public static string ToFunnyUnicodeLettersDouble(this int num)
        // {
        //     return num.ToFunnyUnicodeLetters(9460);

        // }
        // public static string ToFunnyUnicodeLetters(this int num)
        // {
        //     return num.ToFunnyUnicodeLetters(9450);

        // }
    }
}
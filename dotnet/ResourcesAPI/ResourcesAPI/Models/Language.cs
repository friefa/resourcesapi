using System;
using System.Collections.Generic;
using System.Text;

namespace ResourcesAPI.Models
{
    public class Language
    {
        public const string German = "de";
        public const string English = "en";
        public const string Russian = "ru";
        public const string Japanese = "ja";
        public const string Indonesian = "in";
        public const string Spanish = "es";
        public const string French = "fr";

        public string Identifier = English;

        public Language(string identifier) { this.Identifier = identifier; }
    }
}

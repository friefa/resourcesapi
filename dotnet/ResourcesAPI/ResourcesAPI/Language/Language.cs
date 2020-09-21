namespace ResourcesAPI.Language
{
    public class Language
    {
        public static readonly Language German = new Language("de");
        public static readonly Language English = new Language("en");
        public static readonly Language Russian = new Language("ru");
        public static readonly Language Japanese = new Language("ja");
        public static readonly Language Indonesian = new Language("in");
        public static readonly Language Spanish = new Language("es");
        public static readonly Language French = new Language("fr");

        public string Identifier = default;

        public Language(string identifier = "en") { this.Identifier = identifier; }
    }
}

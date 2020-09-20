using System;
using System.Collections.Generic;
using System.Text;

namespace ResourcesAPI.Models
{
    public class Query
    {
        public QueryType Type { get; private set; }

        public Language Language { get; private set; }

        public OutputType OutputType { get; private set; }

        public byte HistoryDays { get; set; } = 30;

        public Query(QueryType type, OutputType outputType, Language language)
        {
            this.Type = type;
            this.OutputType = outputType;
            this.Language = language;
        }

        public Uri GetUri(string apiKey)
        {
            string format = "https://www.resources-game.ch/resapi/?q={0}&f={1}&k={2}&l={3}&d={4}";
            return new Uri(string.Format(format, this.Type, this.OutputType, apiKey, this.Language.Identifier, this.HistoryDays));
        }
    }
}

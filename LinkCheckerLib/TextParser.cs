using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkCheckerLib
{
    public static class TextParser
    {
        public static class LinkFinder
        {
            private static char[] _linkDelimeters = "()[]{},.!?<>;:".ToCharArray();

            /// <summary>
            /// Разделители ссылок
            /// </summary>
            public static char[] LinkDelimeters
            {
                get { return _linkDelimeters; }
                set { _linkDelimeters = value; }
            }

            /// <summary>
            /// Ищет ссылки в строке
            /// </summary>
            /// <param name="text">Строка, в которой будет происходить поиск ссылок</param>
            /// <returns>Массив найденных ссылок</returns>
            public static string[] FindLinks(string text)
            {
                if (text == null)
                    throw new ArgumentNullException("Переменная text является null");

                Regex regex = new Regex(@"((http|https):\/\/|)[A-z0-9,.\\\/:;?<>!@#$%^&*()\-+=_]{0,}(\.ru|\.com|\.net)[A-z0-9,.\\\/:;?<>!@#$%^&*()\-+=_]{0,}");
                var matches = regex.Matches(text);

                if (matches.Count > 0)
                {
                    string[] result = new string[matches.Count];
                    for (int i = 0; i < matches.Count; i++)
                    {
                        result[i] = matches[i].Value.Trim(_linkDelimeters);

                        if ((result[i].Length <= 7 || !result[i].Substring(0, 7).Equals("http://")) && (result[i].Length <= 8 || !result[i].Substring(0, 8).Equals("https://")))
                            result[i] = "http://" + result[i];
                    }

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public static class MatchFinder
        {
            private static string[] _textForFind = Array.Empty<string>();

            public static string[] SearchPatterns
            {
                get { return _textForFind; }
                set { _textForFind = value; }
            }

            /// <summary>
            /// Находит совпадения в исходном тексте с фразами для поиска
            /// </summary>
            /// <param name="text">Текст, в котором будут искаться совпадения</param>
            /// <param name="pattern">Регулярное выражение, по которому будет осущетсвляться поиск в тексте</param>
            /// <returns>Массив найденных фраз</returns>
            public static string[] FindMatches(string text, string pattern)
            {
                if (text == null || pattern == null)
                    throw new ArgumentNullException();

                if (pattern.Length == 0)
                    return Array.Empty<string>();

                text = text.ToLower();
                pattern = pattern.ToLower();

                Regex regex = new Regex(pattern);
                var matches = regex.Matches(text);

                string[] result = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                    result[i] = matches[i].Value;

                return result;
            }

            /// <summary>
            /// Находит совпадения в исходном тексте с фразами для поиска
            /// </summary>
            /// <param name="text">Текст, в котором будут искаться совпадения</param>
            /// <param name="textForFind">Фразы, которые будут искаться в тексте</param>
            /// <returns>Массив найденных фраз</returns>
            public static string[] FindMatches(string text, string[] textForFind)
            {
                if (text == null || textForFind == null)
                    throw new ArgumentNullException();

                if (textForFind.Length == 0)
                    return Array.Empty<string>();

                StringBuilder patternBuilder = new StringBuilder();
                // построение регулярного выражения
                for (int i = 0; i < textForFind.Length; i++)
                {
                    if (i != 0)
                        patternBuilder.Append('|');
                    patternBuilder.Append(textForFind[i]);
                }

                text = text.ToLower();

                Regex regex = new Regex(patternBuilder.ToString().ToLower());
                var matches = regex.Matches(text);

                string[] result = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                    result[i] = matches[i].Value;

                return result;
            }

            /// <summary>
            /// Находит совпадения в исходном тексте с фразами для поиска (фразы для поиска берутся из соответствующего свойства)
            /// </summary>
            /// <param name="text">Текст, в котором будут искаться совпадения</param>
            /// <returns>Массив найденных фраз</returns>
            public static string[] FindMatches(string text)
            {
                return FindMatches(text, _textForFind);
            }
        }
    }   
}

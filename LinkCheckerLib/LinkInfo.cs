using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCheckerLib
{
    /// <summary>
    /// Хранит данные о ссылке, а также о результатх её проверки.
    /// </summary>
    public class LinkInfo
    {
        private string _link;
        private int _responseCode;
        private string[] _foundErrorMessageMatches;

        public string Link { get => _link; set => _link = value; }
        public int ResponseCode { get => _responseCode; set => _responseCode = value; }
        public string[] FoundErrorMessageMatches { get => _foundErrorMessageMatches; set => _foundErrorMessageMatches = value; }

        public LinkInfo()
        {
            _link = null;
            _responseCode = 0;
            _foundErrorMessageMatches = null;
        }

        public LinkInfo(string link, int responseCode, string[] foundErrorMessageMatches)
        {
            _link = link;
            _responseCode = responseCode;
            _foundErrorMessageMatches = foundErrorMessageMatches;
        }
    }
}

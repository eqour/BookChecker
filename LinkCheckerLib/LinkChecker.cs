using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCheckerLib
{
    public static class LinkChecker
    {
        public static LinkInfo CheckLink(string link)
        {
            LinkInfo linkInfo = new LinkInfo();

            linkInfo.Link = link;
            linkInfo.ResponseCode = (int)WebWorker.GetStatusCode(link);

            if (linkInfo.ResponseCode == 200)
            {
                string text = WebWorker.GetResponse(link);
                linkInfo.FoundErrorMessageMatches = TextParser.MatchFinder.FindMatches(text);
            }

            return linkInfo;
        }
    }
}

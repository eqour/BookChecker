using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using LinkCheckerLib;

namespace WFInterface
{
    public static class ConfigLoader
    {
        public static HandleLinksInfo LoadHandleLinksInfo(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HandleLinksInfo));
            StreamReader reader = new StreamReader(path);
            HandleLinksInfo info = (HandleLinksInfo)serializer.Deserialize(reader);

            reader.Close();
            reader.Dispose();
            reader = null;

            return info;
        }
    }
}

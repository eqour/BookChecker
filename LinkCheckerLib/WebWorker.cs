using System;
using System.Net;

namespace LinkCheckerLib
{
    public static class WebWorker
    {
        /// <summary>
        /// Возвращает код состояния http запроса
        /// </summary>
        /// <param name="url">Ссылка на страницу</param>
        /// <returns>Код состояния</returns>
        public static HttpStatusCode GetStatusCode(string url)
        {
            if (url == null)
                throw new ArgumentNullException();

            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpWebResponse.StatusCode;
                httpWebResponse.Close();
                httpWebResponse.Dispose();
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response) != null)
                    statusCode = ((HttpWebResponse)e.Response).StatusCode;

                return statusCode;
            }

            return statusCode;
        }

        public static string GetResponse(string url)
        {
            WebClient client = new WebClient();
            string response = "";
            try
            {
                response = client.DownloadString(url);
            }
            catch (Exception) { }
            finally
            {
                client.Dispose();
            }
            return response;
        }
    }
}

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace LinkCheckerLib
{
    public static class WebWorker
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Возвращает код состояния http запроса
        /// </summary>
        /// <param name="uri">Ссылка на страницу</param>
        /// <returns>Код состояния</returns>
        public static HttpStatusCode GetStatusCode(string uri)
        {
            validateUri(uri);
            return MakeGetRequestWithRedirect(uri).StatusCode;
        }

        /// <summary>
        /// Возвращает тело http запроса
        /// </summary>
        /// <param name="uri">Ссылка на страницу</param>
        /// <returns>Тело запроса</returns>
        public static string GetResponse(string uri)
        {
            string body = MakeGetRequestWithRedirect(uri).Body;
            return body == null ? "" : body;
        }

        private static void validateUri(string uri)
        {
            if (uri == null) throw new ArgumentNullException();
            new Uri(uri);
        }

        private static RequestResult MakeGetRequestWithRedirect(string uri)
        {
            try
            {
                HttpResponseMessage responseMessage = client.GetAsync(uri).Result;
                if (responseMessage.StatusCode == HttpStatusCode.Moved || responseMessage.StatusCode == HttpStatusCode.Found
                    && responseMessage.Headers.Location != null)
                {
                    Debug.WriteLine($"redirect ({(int)responseMessage.StatusCode}): {responseMessage.Headers.Location}");
                    return MakeGetRequestWithRedirect(responseMessage.Headers.Location.ToString());
                }
                else
                {
                    Debug.WriteLine($"request uri ({(int)responseMessage.StatusCode}): {uri}");
                    return new RequestResult(responseMessage.StatusCode, responseMessage.Content.ReadAsStringAsync().Result);
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"request uri ({404}): {uri}");
                return new RequestResult(e.StatusCode.GetValueOrDefault(HttpStatusCode.NotFound), null);
            }
            catch (Exception)
            {
                Debug.WriteLine($"request uri ({404}): {uri}");
                return new RequestResult(HttpStatusCode.NotFound, null);
            }
        }

        private class RequestResult
        {
            public HttpStatusCode StatusCode;
            public string Body;

            public RequestResult(HttpStatusCode statusCode, string body)
            {
                StatusCode = statusCode;
                Body = body;
            }
        }
    }
}

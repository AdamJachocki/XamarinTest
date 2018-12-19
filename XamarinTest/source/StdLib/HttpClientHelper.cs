using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StdLib
{

    public sealed class HttpClientHelper : IDisposable
    {
        HttpClient client;

        public string BaseAddress { get; private set; }


        void InitClient()
        {
            HttpClientHandler handler = null;

            if (handler != null)
                client = new HttpClient(handler, true);
            else
                client = new HttpClient();

            client.BaseAddress = new Uri(BaseAddress);
        }

        /// <summary>
        /// Creates helper class with base api address
        /// </summary>
        /// <param name="baseAddress"></param>
        public HttpClientHelper(string baseAddress)
        {
            BaseAddress = baseAddress;
            InitClient();
        }

        //async Task<(HttpResponseMessage response, SystemUserModel user)> AuthenticateUser()
        //{
        //    LoginModel model = new LoginModel();
        //    model.Login = username;
        //    model.Password = password;
        //    string content = JsonConvert.SerializeObject(model, Formatting.Indented);

        //    HttpResponseMessage response = await client.PostJsonAsync(model, "token");
        //    if (!response.IsSuccessStatusCode)
        //        return (response, null);
        //    string str = await response.Content.ReadAsStringAsync();


        //    try
        //    {
        //        tokenModel = (AuthToken)JsonConvert.DeserializeObject(str, typeof(AuthToken));
        //    }
        //    catch (Exception)
        //    {
        //        return (response, null);
        //    }

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.Token);
        //    return (response, tokenModel.CurrentUser);
        //}


        public async Task<HttpResponseMessage> SendAsyncRequest(string uri, string content, HttpMethod method)
        {
            HttpRequestMessage rm = new HttpRequestMessage(method, uri);
            if (!string.IsNullOrWhiteSpace(content))
                rm.Content = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(rm);
            return response;
        }


        public async Task<(HttpResponseMessage response, T obj)> GetObjectFromRequest<T>(string uri) where T : class
        {
            HttpResponseMessage response = await SendAsyncRequest(uri, null, HttpMethod.Get);
            if (!response.IsSuccessStatusCode)
                return (response, null);

            T obj = await GetObjectFromContent<T>(response.Content);
            return (response, obj);
        }

        public async Task<T> GetObjectFromContent<T>(HttpContent content) where T : class
        {
            string response = await content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(response))
                return null;

            try
            {
                T obj = JsonConvert.DeserializeObject<T>(response);
                return obj;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HttpClientHelper() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

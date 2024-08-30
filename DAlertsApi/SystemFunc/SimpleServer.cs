using DAlertsApi.Logger;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Auth.Inplicit;
using System.Net;
using System.Web;

namespace DAlertsApi.SystemFunc
{
    public class SimpleServer : IDisposable
    {
        private readonly string url;
        private readonly string port;
        private readonly ILogger logger;
        private HttpListener listener; 
        private bool isRunning;

        public SimpleServer(string url, string port = null)
        {
            this.url = url;
            this.port = port;
        }
        public SimpleServer(string url, string port = null, ILogger logger = null)
        {
            this.url = url;
            this.port = port;
            this.logger = logger;
        } 

        public void Start()
        {
            isRunning = true;
            listener = new HttpListener();
            var url = StaticMethods.GetUrl(this.url, port);
            listener.Prefixes.Add(url);
            listener.Start();
            logger?.Log($"Server was started with: {url}");
        }
        public async Task<CodeModel> AwaitCode()
        {
            CodeModel codeModel = new();
            while (isRunning)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;

                    // Парсим query параметры 
                    var query = HttpUtility.ParseQueryString(request.Url.Query);
                    var queryDict = query.AllKeys.ToDictionary(k => k?.ToLower(), k => query[k]);

                    codeModel.Code = queryDict.GetValueOrDefault("code", string.Empty); 
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Close();

                    logger?.Log($"Code received and processed: {codeModel}"); 
                    return codeModel;
                }
                catch (Exception ex)
                {
                    logger?.Log(ex.Message);
                }
            } 
            return codeModel;
        }
        public async Task<AccessTokenImplicitResponse> AwaitInplicitToken()
        {
            AccessTokenImplicitResponse codeModel = new();
            while (isRunning)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;
                     
                    var query = HttpUtility.ParseQueryString(request.Url.Query);
                    var queryDict = query.AllKeys.ToDictionary(k => k?.ToLower(), k => query[k]);

                    codeModel.Access_token = queryDict.GetValueOrDefault("access_token", string.Empty);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Close();

                    logger?.Log($"AccessToken received and processed: {codeModel}");
                    return codeModel;
                }
                catch (Exception ex)
                {
                    logger?.Log(ex.Message);
                }
            }

            return codeModel;
        }

        public void Dispose()
        {
            isRunning = false;
            listener.Stop();
            listener.Close(); 
            logger?.Log("Server was stopped");
        }

        
    }
}

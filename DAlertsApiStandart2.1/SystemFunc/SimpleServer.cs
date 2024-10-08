﻿using DAlertsApi.Logger;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Auth.Inplicit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DAlertsApi.SystemFunc
{
    public class SimpleServer : IDisposable
    {
        private readonly string url;
        private readonly string? port;
        private readonly ILogger? logger;
        private HttpListener? listener; 
        private bool isRunning;

        public SimpleServer(string url, string? port = null)
        {
            this.url = url;
            this.port = port;
        }
        public SimpleServer(string url, string? port = null, ILogger? logger = null) : this(url, port)
        { 
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
        public async Task<CodeModel?> AwaitCode(CancellationToken cancellationToken)
        {
            if (!isRunning)
            {
                logger?.Log("Server is not running");
                return null;
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested(); 
                CodeModel codeModel = new CodeModel();
                while (isRunning)
                {
                    try
                    {
                        HttpListenerContext context = await listener?.GetContextAsync();
                        HttpListenerRequest request = context.Request;

                        if (request == null)
                        {
                            logger?.Log("SIMPLE SERVER: Request is null", LogLevel.Warning);
                            continue;
                        }
                        if (request.Url == null)
                        {
                            logger?.Log("SIMPLE SERVER: Request URL is null", LogLevel.Warning);
                            continue;
                        }
                    

                        // Парсим query параметры 
                        var query = HttpUtility.ParseQueryString(request.Url.Query);
                        if (query == null )
                        {
                            logger?.Log("SIMPLE SERVER: Query is null", LogLevel.Warning);
                            continue;
                        }
                        if (query.AllKeys == null)
                        {
                            logger?.Log("SIMPLE SERVER: Query AllKeys is null", LogLevel.Warning);
                            continue;
                        }
                        Dictionary<string?, string?> queryDict = query.AllKeys.ToDictionary(k => k?.ToLower(), k => query[k]);

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
            catch (OperationCanceledException)
            {
                logger?.Log("Operation was canceled");
                return null;
            }
        }
        public async Task<AccessTokenImplicitResponse> AwaitInplicitToken(CancellationToken cancellationToken)
        {
            if (!isRunning)
            {
                logger?.Log("Server is not running");
                return null;
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested(); 
                AccessTokenImplicitResponse codeModel = new AccessTokenImplicitResponse();
                while (isRunning)
                {
                    try
                    {
                        HttpListenerContext context = await listener?.GetContextAsync();
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
            catch (OperationCanceledException)
            {
                logger?.Log("Operation was canceled");
                return null;
            }
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

﻿namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : IResponse
    {
        private readonly IHttp _http;
        private const int CacheAgeMinutes = 5;

        public CachedCssResponse(IHttp http)
        {
            _http = http;
        }

        public void WriteCss(string css)
        {
            var response = _http.Context.Response;
            var request = _http.Context.Request;

            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(new TimeSpan(0, CacheAgeMinutes, 0));

            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(CacheAgeMinutes));
            response.Cache.SetETagFromFileDependencies();

            response.ContentType = "text/css";
            response.Write(css);

/*
            if (request.Headers.Get("If-None-Match") == response.Headers.Get("ETag"))
            {
                response.StatusCode = 304;
                response.StatusDescription = "Not Modified";

                // Explicitly set the Content-Length header so client
                // keeps the connection open for other requests.
                response.AddHeader("Content-Length", "0");
            }
            else
            {
                response.ContentType = "text/css";
                response.Write(css);
            }

            response.End();
*/
        }
    }
}
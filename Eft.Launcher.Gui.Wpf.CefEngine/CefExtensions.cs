using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CefSharp;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher.Gui.Wpf.CefEngine;

public static class CefExtensions
{
	public static IServiceCollection AddCustomCefHandlers(this IServiceCollection services)
	{
		return services.AddTransient<ILifeSpanHandler, _E011>().AddTransient<IRequestHandler, _E010>().AddTransient<IContextMenuHandler, _E012>()
			.AddTransient<IDisplayHandler, _E00D>();
	}

	public static async Task LoadCookiesAsync(this IWebBrowser browser, Uri uri, CookieContainer source)
	{
		ICookieManager cookieManager = browser.GetCookieManager();
		foreach (System.Net.Cookie cookie3 in source.GetCookies(uri))
		{
			CefSharp.Cookie cookie2 = new CefSharp.Cookie
			{
				Name = cookie3.Name,
				Value = cookie3.Value,
				Domain = cookie3.Domain,
				Path = cookie3.Path,
				Expires = cookie3.Expires,
				HttpOnly = cookie3.HttpOnly,
				Secure = cookie3.Secure
			};
			if (!(await cookieManager.SetCookieAsync(uri.GetLeftPart(UriPartial.Authority), cookie2)))
			{
				throw new Exception(_E05B._E000(31151) + cookie2.Name + _E05B._E000(31111) + cookie2.Domain + _E05B._E000(27975));
			}
		}
	}

	public static async Task<IRequest> CreateRequestAsync(this IFrame frame, HttpRequestMessage httpRequest)
	{
		IRequest request = frame.CreateRequest();
		request.Flags = UrlRequestFlags.AllowStoredCredentials | UrlRequestFlags.NoRetryOn5XX;
		NameValueCollection nameValueCollection = new NameValueCollection();
		foreach (KeyValuePair<string, IEnumerable<string>> header in httpRequest.Headers)
		{
			nameValueCollection.Add(header.Key, string.Join(_E05B._E000(31120), header.Value));
		}
		request.Headers = nameValueCollection;
		request.Method = httpRequest.Method.ToString();
		request.Url = httpRequest.RequestUri.ToString();
		if (httpRequest.Content != null)
		{
			request.InitializePostData();
			IPostData postData = request.PostData;
			postData.AddData(await httpRequest.Content.ReadAsByteArrayAsync());
		}
		return request;
	}
}

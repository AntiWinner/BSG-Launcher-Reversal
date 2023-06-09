using System;
using CefSharp;
using CefSharp.Handler;

namespace Eft.Launcher.Gui.Wpf.CefEngine;

public class ResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
{
	protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
	{
		if (request.TransitionType == TransitionType.LinkClicked && request.ResourceType == ResourceType.SubFrame && request.Url.StartsWith(Uri.UriSchemeFile))
		{
			UriBuilder uriBuilder = new UriBuilder(request.Url)
			{
				Scheme = Uri.UriSchemeHttp
			};
			request.Url = uriBuilder.ToString();
		}
		return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
	}
}

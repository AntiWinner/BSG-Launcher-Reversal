using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bsg.Launcher.Services.BugReportService;
using Eft.Launcher.Base.Network.Http;
using Eft.Launcher.Network.Http;
using Eft.Launcher.Services.InformationCollectionService;
using Eft.Launcher.Services.SettingsService;
using Eft.Launcher.Services.SiteCommunicationService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.SiteCommunicationService;

public class SiteCommunicationService : ISiteCommunicationService
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SiteCommunicationService _E000;

		public ISettingsService settingsService;

		internal void _E000(object s, EventArgs eArgs)
		{
			this._E000._E000(settingsService.SelectedBranch.SiteUri);
		}

		internal void _003C_002Ector_003Eb__1(object s, OnBranchChangedEventArgs eArgs)
		{
			this._E000._E000(eArgs.NewBranch.SiteUri);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public SiteCommunicationService _E000;

		public string _E001;

		internal Task<JToken> _E000()
		{
			return this._E000.RequestAsync(_E05B._E000(20789), new JsonContent(new JObject
			{
				{
					_E05B._E000(21599),
					_E001
				},
				{
					_E05B._E000(20749),
					this._E000._settingsService.Language
				}
			}));
		}
	}

	private static readonly Regex m__E000 = new Regex(_E05B._E000(21642), RegexOptions.Compiled);

	[CompilerGenerated]
	private Action<SiteNetworkState> _E001;

	private readonly ILogger _E002;

	private readonly ISettingsService _settingsService;

	private readonly IInformationCollectionService _E003;

	private readonly JsonSerializer _E004;

	private readonly JsonSerializerSettings _E005;

	private readonly IHttpClientBuilder m__E006;

	private HttpClient _E007;

	private int _E008;

	public event Action<SiteNetworkState> OnNetworkStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<SiteNetworkState> action = this._E001;
			Action<SiteNetworkState> action2;
			do
			{
				action2 = action;
				Action<SiteNetworkState> value2 = (Action<SiteNetworkState>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<SiteNetworkState> action = this._E001;
			Action<SiteNetworkState> action2;
			do
			{
				action2 = action;
				Action<SiteNetworkState> value2 = (Action<SiteNetworkState>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this._E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public SiteCommunicationService(IHttpClientBuilder httpClientBuilder, ILogger<SiteCommunicationService> logger, ISettingsService settingsService, IInformationCollectionService informationCollectionService, JsonSerializer jsonSerializer, JsonSerializerSettings jsonSerializerSettings)
	{
		SiteCommunicationService siteCommunicationService = this;
		this._E002 = logger;
		_settingsService = settingsService;
		this._E003 = informationCollectionService;
		this._E004 = jsonSerializer;
		this._E005 = jsonSerializerSettings;
		this.m__E006 = httpClientBuilder.AddExceptionHandling(ensureSuccessStatusCodes: true).AddRequestMetadata(addLanguage: true, addBranchName: true, addGameVersion: true).AddAuthentication()
			.AddLogging<SiteCommunicationService>()
			.AddApiResponses(_E05B._E000(21599), _E05B._E000(21666), _E05B._E000(21670), _E05B._E000(21673))
			.AddJsonResponses()
			.AddCallback(_E000, _E000);
		settingsService.OnUserProfileLoaded += delegate
		{
			siteCommunicationService._E000(settingsService.SelectedBranch.SiteUri);
		};
		settingsService.OnBranchChanged += delegate(object s, OnBranchChangedEventArgs eArgs)
		{
			siteCommunicationService._E000(eArgs.NewBranch.SiteUri);
		};
	}

	private Task _E000(HttpRequestMessage request)
	{
		if (Interlocked.Increment(ref _E008) == 1)
		{
			this._E001?.Invoke(SiteNetworkState.Busy);
		}
		return Task.CompletedTask;
	}

	private Task _E000(HttpResponseMessage response)
	{
		if (Interlocked.Decrement(ref _E008) == 0)
		{
			this._E001?.Invoke(SiteNetworkState.Idle);
		}
		return Task.CompletedTask;
	}

	private void _E000(Uri baseAddress)
	{
		this._E007 = this.m__E006.WithBaseAddress(baseAddress).Build();
	}

	public async Task<BugReportSendingResult> SendBugReport(BugReport bugReport, int gameLogsSizeLimit, IEnumerable<string> sids, Action<int> onProgress)
	{
		if (bugReport == null)
		{
			throw new ArgumentNullException(_E05B._E000(21730));
		}
		onProgress?.Invoke(0);
		using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{
				_E05B._E000(21736),
				bugReport.Message
			},
			{
				_E05B._E000(21744),
				bugReport.CategoryId
			},
			{
				_E05B._E000(21759),
				_settingsService.Language
			},
			{
				_E05B._E000(21702),
				bugReport.GameVersion.ToString()
			},
			{
				_E05B._E000(21713),
				string.Join(_E05B._E000(21716), sids)
			}
		})
		{
			StringContent content = new StringContent(item.Value ?? string.Empty);
			multipartFormDataContent.Add(content, item.Key);
		}
		IList<AttachmentStream> list = null;
		JToken jToken;
		try
		{
			list = bugReport.GetAllAttachments(gameLogsSizeLimit, this._E005, this._E002);
			for (int i = 0; i < list.Count; i++)
			{
				AttachmentStream attachmentStream = list[i];
				string text = string.Format(_E05B._E000(21722), i);
				StreamContent streamContent = new StreamContent(attachmentStream);
				streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(attachmentStream.FileName));
				string fileName = (SiteCommunicationService.m__E000.IsMatch(attachmentStream.FileName) ? attachmentStream.FileName : (SiteCommunicationService.m__E000.IsMatch(Path.GetExtension(attachmentStream.FileName)) ? (text + Path.GetExtension(attachmentStream.FileName)) : text));
				streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(_E05B._E000(21291))
				{
					Name = text,
					FileName = fileName,
					FileNameStar = attachmentStream.FileName
				};
				multipartFormDataContent.Add(streamContent);
			}
			jToken = await RequestAsync(_E05B._E000(21297), multipartFormDataContent);
		}
		finally
		{
			if (list != null)
			{
				foreach (AttachmentStream item2 in list)
				{
					item2.Dispose();
				}
			}
		}
		return new BugReportSendingResult(jToken.Value<int>(_E05B._E000(21256)), jToken.Value<string>(_E05B._E000(21279)));
	}

	public async Task<BugReportSendingResult> SendGameCrashReport(CrashReport crashReport)
	{
		if (crashReport == null)
		{
			throw new ArgumentNullException(_E05B._E000(21344));
		}
		using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{
				_E05B._E000(21356),
				((!crashReport.HaveDump) ? 1 : 3).ToString()
			},
			{
				_E05B._E000(21702),
				crashReport.GameVersion.ToString()
			},
			{
				_E05B._E000(21366),
				crashReport.ExitCode.ToString()
			},
			{
				_E05B._E000(21372),
				crashReport.HaveDump.ToString()
			}
		})
		{
			StringContent content = new StringContent(item.Value ?? string.Empty);
			multipartFormDataContent.Add(content, item.Key);
		}
		using AttachmentStream attachmentStream = crashReport.GetAttachmentsStream(this._E005);
		if (attachmentStream != null)
		{
			new StreamContent(attachmentStream).Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(attachmentStream.FileName));
			multipartFormDataContent.Add(new StreamContent(attachmentStream), _E05B._E000(21322), attachmentStream.FileName);
		}
		JToken jToken = await RequestAsync(_E05B._E000(21297), multipartFormDataContent);
		return new BugReportSendingResult(jToken.Value<int>(_E05B._E000(21256)), jToken.Value<string>(_E05B._E000(21279)));
	}

	public async Task SendSystemInfo()
	{
		SystemInfo systemInfo = this._E003.GetSystemInfo();
		if (systemInfo == null)
		{
			this._E002.LogWarning(_E05B._E000(21335));
		}
		else if (systemInfo.Checksum != _settingsService.SystemInfoChecksum)
		{
			this._E002.LogDebug(_E05B._E000(21381));
			JsonContent content = new JsonContent(new JObject { 
			{
				_E05B._E000(21407),
				JToken.FromObject(systemInfo, this._E004)
			} });
			string text = (await RequestAsync(_E05B._E000(21476), content)).Value<string>(_E05B._E000(21279));
			if (text == _E05B._E000(21441))
			{
				_settingsService.SystemInfoChecksum = systemInfo.Checksum;
				_settingsService.Save();
				this._E002.LogDebug(_E05B._E000(21444));
			}
			else
			{
				this._E002.LogWarning(_E05B._E000(21031), text);
			}
		}
		else
		{
			this._E002.LogTrace(_E05B._E000(21099));
		}
	}

	public async Task<bool> CheckIfLicenseAgreementIsRequired()
	{
		this._E002.LogDebug(_E05B._E000(21066));
		bool flag = (await RequestAsync(_E05B._E000(21173))).Value<bool>(_E05B._E000(21279));
		this._E002.Log(flag ? LogLevel.Debug : LogLevel.Information, null, _E05B._E000(21139), flag ? _E05B._E000(21197) : _E05B._E000(21190));
		return !flag;
	}

	public async Task<string> GetFeedbackFormData()
	{
		return (await RequestAsync(_E05B._E000(21208))).ToString(Formatting.None);
	}

	public Task<JToken> ActivateCode(string activationCode)
	{
		return this._E002.WithExceptionsHandling(_E05B._E000(21676), () => RequestAsync(_E05B._E000(20789), new JsonContent(new JObject
		{
			{
				_E05B._E000(21599),
				activationCode
			},
			{
				_E05B._E000(20749),
				_settingsService.Language
			}
		})));
	}

	public async Task<JToken> RequestAsync(string requestUri, HttpContent content = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
		{
			Content = content
		};
		if (this._E007 == null)
		{
			throw new NetworkException(requestMessage, GetType().Name + _E05B._E000(20752));
		}
		using HttpResponseMessage httpResponseMessage = await this._E007.SendAsync(requestMessage, cancellationToken);
		if (!(httpResponseMessage.Content is ApiContent apiContent))
		{
			throw new WrongResponseHttpNetworkException(httpResponseMessage);
		}
		if (apiContent.Code != 0)
		{
			throw new ApiNetworkException(httpResponseMessage, apiContent.Code, apiContent.Message, apiContent.Data, apiContent.Args);
		}
		return apiContent.Data;
	}
}

using oauth2.dingtalk.aspnetcore.Extensions;
using oauth2.dingtalk.aspnetcore.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace oauth2.dingtalk.aspnetcore
{
    public class DingTalkClient
    {
        private readonly HttpClient _httpClient;
        private readonly DingTalkConfig _options;

        public DingTalkClient(HttpClient httpClient, DingTalkConfig options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public string BuildAuthorizationUrl(string state)
        {
            return _options.BuildAuthorizationUrl(state);
        }
        public async Task<DingTalk_User_ResultEntity> GetOpenId(string code, CancellationToken cancellationToken)
        {
            var openIdUrl = _options.BuildOpenIdUrl();
            var result = await PostStringAsync(openIdUrl, new { tmp_auth_code = code }.ToJson(), cancellationToken);

            var ro = result.ToJObject();
            if (ro["errcode"].ToString() == "0")
            {
                result = result.ToJObject()["user_info"].ToJson();
            }
            else
            {
                return null;
            }

            var outmo = Newtonsoft.Json.JsonConvert.DeserializeObject<DingTalk_User_ResultEntity>(result);

            return outmo;
        }

        private async Task<string> GetStringAsync(string url, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        private async Task<string> PostStringAsync(string url, string content, CancellationToken cancellationToken)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, httpContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

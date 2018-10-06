using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mv_api
{
    public class mvapi
    {
        const String apiUrl = @"https://api.mobilevikings.be/v3/";
        const String permissionsUrl = @"https://vikingco.com/en/partners/authorize/";

        private HttpClient client = new HttpClient();

        /// <summary>
        /// To be able to access the data of a Viking, each API client should request access. 
        /// </summary>
        /// <param name="hash">The form hash is provided by VikingCo and is unique for each client</param>
        /// <param name="url">After dismissing or allowing the request to access data, the Viking will be redirected to the url</param>
        /// <returns></returns>
        public async Task<String> getAccessPermissions(String hash, String url)
        {
            String requestUrl = $"{permissionsUrl}?form_hash={hash}&redirect_to={url}";
            return await getRequest(requestUrl);
        }

        /// <summary>
        /// Request an access token for authorization
        /// </summary>
        /// <param name="client_id">the client id of the client that is used to access the api</param>
        /// <param name="client_secret">the client secret of the client</param>
        /// <param name="grant_type">the type of authentication that you want to use: password, authorization_code or refresh_token</param>
        /// <param name="username">username or email of the user, <paramref name="grant_type"/>=password</param>
        /// <param name="password">password of the user, <paramref name="grant_type"/>=password</param>
        /// <param name="code">authorization code that is made via the oauth2 authorization flow, <paramref name="grant_type"/>=authorization_code</param>
        /// <param name="refresh_token">refresh token to obtain a new access token, <paramref name="grant_type"/>=refresh_token</param>
        /// <returns>{"access_token": "OAUTH_TOKEN", "scope": "read", "expires_in": 31535999, "refresh_token": "OAUTH_REFRESH_TOKEN"}</returns>
        public async Task<String> postAccessToken(String client_id, String client_secret, eGrant_type grant_type,
                String username = null, String password = null, String code = null, String refresh_token = null)
        {
            String requestUrl = $"{apiUrl}oauth2/access_token/?client_id={client_id}&client_secret={client_secret}&grant_type={grant_type}&";

            requestUrl = grant_type == eGrant_type.password ? $"username={username}&password={password}" :
                grant_type == eGrant_type.authorization_code ? $"code={code}" :
                $"refresh_token={refresh_token}";

            return await postRequest(requestUrl, new StringContent(""));
        }

        private async Task<String> getRequest(String requestUri)
        {
            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            client.Dispose();

            return responseBody;
        }

        private async Task<String> postRequest(String requestUri, HttpContent content)
        {
            HttpResponseMessage response = await client.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            client.Dispose();

            return responseBody;
        }

        public enum eGrant_type
        {
            password,
            authorization_code,
            refresh_token,
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using QuizEngine.Utilities.Extensions;
using QuizEngine.Models;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace QuizEngine.SDK
{
    public class RestApiClient
    {
        private string _baseUrl;

        public RestApiClient(string baseUrl)
        {
            if(string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl", "RestApiClient cannot be initiated with a null or empty baseUrl.");
            }
            _baseUrl = baseUrl;
        }

        
        public virtual ApiResponse<T> Get<T>(string relativePath, KeyValuePair<string, string>[] parameters)
            where T : class, new()
        {
            var fullUrl = GenerateFullUrl(relativePath, parameters);

            ApiResponse<T> apiResponse;
            using(var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(fullUrl).Result;
                ProcessResponse<T>(response, out apiResponse);
            }

            return apiResponse;
        }

        public virtual ApiResponse<T> Post<T>(string relativePath, KeyValuePair<string, string>[] parameters, T data)
            where T : class, new()
        {
            var fullUrl = GenerateFullUrl(relativePath, parameters);

            ApiResponse<T> apiResponse;
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpContent = CreateContentForSubmission<T>(data);
                var response = httpClient.PostAsync(fullUrl, httpContent).Result;
                ProcessResponse<T>(response, out apiResponse);
            }

            return apiResponse;
        }

        public virtual ApiResponse<T> Put<T>(string relativePath, KeyValuePair<string, string>[] parameters, T data)
            where T : class, new()
        {
            var fullUrl = GenerateFullUrl(relativePath, parameters);

            ApiResponse<T> apiResponse;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpContent = CreateContentForSubmission<T>(data);
                var response = httpClient.PutAsync(fullUrl, httpContent).Result;
                ProcessResponse<T>(response, out apiResponse);
            }

            return apiResponse;
        }

        public virtual ApiResponse<T> Delete<T>(string relativePath, KeyValuePair<string, string>[] parameters)
            where T : class, new()
        {
            var fullUrl = GenerateFullUrl(relativePath, parameters);

            ApiResponse<T> apiResponse;
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.DeleteAsync(fullUrl).Result;
                ProcessResponse<T>(response, out apiResponse, true);
            }

            return apiResponse;
        }

        private HttpContent CreateContentForSubmission<T>(T data)
        {
            JsonMediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            return new ObjectContent<T>(data, jsonFormatter);
        }

        private void ProcessResponse<T>(HttpResponseMessage response, out ApiResponse<T> apiResponse, bool requestWasDelete = false)
            where T : class, new()
        {
            apiResponse = new ApiResponse<T>();
            apiResponse.StatusCode = response.StatusCode;

            string content = response.Content.ReadAsStringAsync().Result;
            if(response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.MultipleChoices)
            {
                // within the "successful" status codes
                if (requestWasDelete)
                {
                    // the request was a deletion, and was success, just set the IsSuccessful flag, there is no model to deserialize
                    apiResponse.IsSuccessful = true;
                }
                else
                {
                    try
                    {
                        T model = JsonConvert.DeserializeObject<T>(content);
                        apiResponse.Model = model;
                        apiResponse.IsSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        // the JSON deserialization threw an exception
                        apiResponse.ErrorMessage = string.Format("JSON deserialization failed: {0}", e.Message);
                        apiResponse.IsSuccessful = false;
                    }
                }
            }
            else
            {
                // the HTTP request was not successful
                var jObject = JObject.Parse(content);
                var errorMessage = (string)jObject["message"];
                if(!string.IsNullOrEmpty(errorMessage))
                {
                    apiResponse.ErrorMessage = errorMessage;
                }
                else
                {
                    apiResponse.ErrorMessage = "API request failed, but could not parse error message from server response.";
                }
                apiResponse.IsSuccessful = false;
            }
        }

        private string GenerateQueryString(KeyValuePair<string, string>[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                var concatParameters = parameters.Select(kvp => String.Concat(kvp.Key.UrlEncode(), "=", kvp.Value.UrlEncode()));

                return string.Join("&", concatParameters);
            }
            return string.Empty;
        }

        private string GenerateFullUrl(string relativePath, KeyValuePair<string, string>[] parameters)
        {
            var queryString = GenerateQueryString(parameters);

            return GenerateFullUrl(relativePath, queryString);
        }

        private string GenerateFullUrl(string relativePath, string queryString)
        {
            var stringBuilder = new StringBuilder(_baseUrl.TrimEnd('/')); // clean up trailing "/"
            if (!string.IsNullOrEmpty(relativePath))
            {
                stringBuilder.Append("/");
                stringBuilder.Append(relativePath.TrimStart('/').TrimEnd('/')); // clean up starting and trailing "/"s
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                if(stringBuilder.ToString().Contains('?')) // if "?" already exists, assume there is already a query string
                {
                    stringBuilder.Append("&");
                }
                else
                {
                    stringBuilder.Append("?");
                }
                stringBuilder.Append(queryString);
            }

            return stringBuilder.ToString();
        }
    }
}

using Models.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Client.Core.Tools;

internal static class APIClient
{
    private static readonly string _host = Config.GetValue("host");

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    internal static async Task<IEnumerable<T>> GetCollectionAsync<T>(string path, object? by = null) where T : Model
    {
        var requestPath = _host + path;

        if (by is not null) requestPath += by;

        HttpClient client = new();

        var response = await client.GetAsync(requestPath);

        var content = await response.Content.ReadAsStringAsync();

        client.Dispose();

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(JsonSerializer.Deserialize<string>(content, _jsonOptions)!, "Произошла ошибка");

            return null;
        }

        return JsonSerializer.Deserialize<IEnumerable<T>>(content, _jsonOptions)!;
    }

    internal static async Task<T> GetAsync<T>(string path) where T : Model
    {
        var requestPath = _host + path;

        HttpResponseMessage response;

        using (HttpClient client = new())
        {
            response = await client.GetAsync(requestPath);
        }

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(JsonSerializer.Deserialize<string>(content, _jsonOptions)!, "Произошла ошибка");

            return null;
        }

        return JsonSerializer.Deserialize<T>(content, _jsonOptions)!;
    }

    internal static async Task<T> GetAsync<T>(string path, int id) where T : Model
    {
        return await GetAsync<T>(path + $"/{id}");
    }

    internal static async Task<T> PostAsync<T>(string path, T value) where T : Model
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        using (HttpClient client = new())
        {
            response = await client.PostAsync(_host + path, content);
        }

        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(JsonSerializer.Deserialize<string>(responseData, _jsonOptions)!, "Произошла ошибка");

            return null;
        }

        return JsonSerializer.Deserialize<T>(responseData, _jsonOptions)!;
    }

    internal static async Task DeleteAsync<T>(string path, T value) where T : Model
    {
        var requestString = _host + path + $"/{value.ID}";

        using (HttpClient client = new())
        {
            await client.DeleteAsync(requestString);
        }
    }

    internal static async Task<T> PutAsync<T>(string path, T value) where T : Model
    {
        var requestString = _host + path;

        var json = JsonSerializer.Serialize(value, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        using (HttpClient client = new())
        {
            response = await client.PutAsync(requestString, content);
        }

        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(JsonSerializer.Deserialize<string>(responseData, _jsonOptions)!, "Произошла ошибка");

            return null;
        }

        return JsonSerializer.Deserialize<T>(responseData, _jsonOptions)!;
    }
}
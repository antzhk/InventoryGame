using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class ServerRequest
{
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AUTH_TOKEN = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    public static IEnumerator SendPostRequest(string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AUTH_TOKEN);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Server Error: " + request.error);
            }
        }
    }
}
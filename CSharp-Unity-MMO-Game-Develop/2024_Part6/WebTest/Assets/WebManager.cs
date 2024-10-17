using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameResult
{
    public string UserName;
    public int Score;
}

// Login - Auth Token
// Game Start - DB (Radis, RDBMS) / Auth Token
// Gmae Result - / Auth Token

public class WebManager : MonoBehaviour
{
    string _baseUrl = "http://localhost:44360";

    void Start()
    {
        GameResult res = new GameResult()
        {
            UserName = "Shung",
            Score = 555
        };

        // CRUD
        SendPostRequest("ranking", res, (uwr) =>
        {
            Debug.Log("TODO");
        });

        SendGetAllRequest("ranking", (uwr) =>
        {
            Debug.Log("TODO");
        });
    }

    public void SendPostRequest(string url, object obj, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "POST", obj, callback));
    }

    public void SendGetAllRequest(string url, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "GET", null, callback));
    }

    // url : 123.123.123/api/ranking
    // 123.123.123은 변하지 않음.
    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest> callback)
    {
        yield return null;

        string sendUrl = $"{_baseUrl}/{url}";

        byte[] jsonBytes = null;
        if (obj != null)
        {
            string json = JsonUtility.ToJson(obj);
            jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            callback.Invoke(uwr);
        }
    }
}

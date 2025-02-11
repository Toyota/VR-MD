/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System;
using UnityEngine;
using UnityEngine.Networking;

public class StreamingAssets
{
    public static StreamingAssetsRequest LoadAsync(String path)
    {
        return new StreamingAssetsRequest(path);
    }
}

public class StreamingAssetsRequest : AsyncOperation
{
    private readonly UnityWebRequestAsyncOperation unityWebRequestAsyncOperation;

    public StreamingAssetsRequest(String path)
    {
        path = Application.streamingAssetsPath + "/" + path;
        UnityWebRequest request = UnityWebRequest.Get(path);
        this.unityWebRequestAsyncOperation = request.SendWebRequest();
        this.unityWebRequestAsyncOperation.completed += (operation) =>
        {
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            this.completed(this);
        };
    }

    new public bool allowSceneActivation
    {
        get
        {
            return this.unityWebRequestAsyncOperation.allowSceneActivation;
        }
        set
        {
            this.unityWebRequestAsyncOperation.allowSceneActivation = value;
        }
    }
    new public bool isDone
    {
        get
        {
            return this.unityWebRequestAsyncOperation.isDone;
        }
    }

    new public int priority
    {
        get
        {
            return this.unityWebRequestAsyncOperation.priority;
        }
        set
        {
            this.unityWebRequestAsyncOperation.priority = value;
        }
    }

    new public float progress
    {
        get
        {
            return this.unityWebRequestAsyncOperation.progress;
        }
    }
    new public event Action<AsyncOperation> completed;

    public String text
    {
        get
        {
            return this.unityWebRequestAsyncOperation.webRequest.downloadHandler.text;
        }
    }

}
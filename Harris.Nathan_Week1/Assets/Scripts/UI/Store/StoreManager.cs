using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Overall manager for the demonstration project. It fires events that the editor can use for "scene management" and initiates background tasks for generating models & game objects.
/// </summary>
public class StoreManager : MonoBehaviour
{
    /// <summary>
    /// Event fired when the background request for root data is started.
    /// </summary>
    public UnityEvent OnMakingRequest;
    /// <summary>
    /// Event fired when all tasks have been completed for preparing Store data.
    /// </summary>
    public UnityEvent OnStoreIsReady;

    [SerializeField]
    private GameObject _storeItemPrefab;
    [SerializeField]
    private GameObject _storeContentContainer;

    /// <summary>
    /// Starts making the request for root JSON data and fires the OnMakingRequest event.
    /// </summary>
    public void FetchStoreData()
    {
        StartCoroutine(MakeUrlRequest());
        OnMakingRequest.Invoke();
    }

    private void StartJsonDataTransforming(string json)
    {
        // completion handler stored locally to dereference within the callback
        StoreItemFactory.CompletionHandler taskCompletedHandler = transformedItems =>
        {
            // loop through the generated items and assign the parent appropriately in the UI
            foreach (GameObject item in transformedItems)
            {
                item.transform.SetParent(_storeContentContainer.transform, false);
            }

            // all tasks are done, so invoke the appropriate event.
            OnStoreIsReady.Invoke();
        };
        // Attach the callback handler within a lamdba that dereferences the handler to save a minor amount of memory
        StoreItemFactory.OnTaskCompleted += items =>
        {
            taskCompletedHandler(items);
            StoreItemFactory.OnTaskCompleted -= taskCompletedHandler;
        };

        // actually start the background task
        StoreItemFactory.TransformJsonToModel(json, _storeItemPrefab);
    }

    /// <summary>
    /// Makes the root JSON request and then passes it to the underlying transformation method.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeUrlRequest()
    {
        var request = new WWW("https://jsonplaceholder.typicode.com/photos");
        
        yield return request;
        
        StartJsonDataTransforming(request.text);
    }
}

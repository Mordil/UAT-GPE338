using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoreManager : MonoBehaviour
{
    public UnityEvent OnMakingRequest;
    public UnityEvent OnStoreIsReady;
    
    [SerializeField]
    private GameObject _storeContentContainer;
    [SerializeField]
    private List<GameObject> _storeItems;

    public void FetchStoreData()
    {
        StartCoroutine(MakeUrlRequest());
        OnMakingRequest.Invoke();
    }

    private void StartJsonDataTransforming(string json)
    {
        StoreItemFactory.CompletionHandler taskCompletedHandler = transformedItems =>
        {
            foreach (GameObject item in transformedItems)
            {
                item.transform.SetParent(_storeContentContainer.transform);
                _storeItems.Add(item);
            }

            OnStoreIsReady.Invoke();
        };
        StoreItemFactory.OnTaskCompleted += items =>
        {
            taskCompletedHandler(items);
            StoreItemFactory.OnTaskCompleted -= taskCompletedHandler;
        };

        StoreItemFactory.TransformJsonToModel(json);
    }

    private IEnumerator MakeUrlRequest()
    {
        var request = new WWW("https://jsonplaceholder.typicode.com/photos");
        
        yield return request;
        
        StartJsonDataTransforming(request.text);
    }
}

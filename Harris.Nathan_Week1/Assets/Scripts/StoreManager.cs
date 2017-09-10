using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StoreManager : MonoBehaviour
{
    public UnityEvent OnMakingRequest;
    public UnityEvent OnStoreIsReady;

    [SerializeField]
    private GameObject _storeContentContainer;

    public void FetchStoreData()
    {
        StartCoroutine(MakeUrlRequest());
        OnMakingRequest.Invoke();
    }

    private IEnumerator MakeUrlRequest()
    {
        var request = new WWW("https://jsonplaceholder.typicode.com/photos");
        
        yield return request;
        
        TransformRequestJSON(request.text);
    }

    private void TransformRequestJSON(string json)
    {
        // TODO: StoreItem factory
        Debug.Log(json);
    }
}

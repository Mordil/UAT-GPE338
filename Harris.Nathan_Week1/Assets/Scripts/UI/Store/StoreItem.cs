using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour for StoreItems pulled from an API service.
/// </summary>
public class StoreItem : MonoBehaviour, IModelBacked<StoreItemModel>
{
    /// <summary>
    /// The underlying data used to populate child GameObjects.
    /// </summary>
    public StoreItemModel Model { get; set; }

    [SerializeField]
    private Text ItemTitle;
    [SerializeField]
    private RawImage ItemImage;

    private void Start()
    {
        // Coroutines are finicky when tried to run elsewhere.
        // I tried invoking this method when Model was set - but it wouldn't continue past the first yield
        // Most threads on Stack Overflow referred to the object being destroyed - but through logs
        // I determined that to not be the case, and the same issue reproduced when started from Awake()
        // But apparently Start works.

        // I think it has to do at which point the lifecycle is consistently ready to handle coroutines.
        UpdateUI();
    }

    private void UpdateUI()
    {
        // local variable so that we can unassign it later
        StoreItemModel.ImageDownloadedHandler assignment = () => ItemImage.texture = Model.thumbnailImage;

        // update the UI with the model data
        ItemTitle.text = Model.title;
        
        // if the image is already available, then no need to make a URL request
        if (Model.thumbnailImage == null)
        {
            Model.ImageDownloaded += () =>
            {
                // update the UI and then remove the handler to free up a very small amount of memory
                assignment();
                Model.ImageDownloaded -= assignment;
            };
            StartCoroutine(Model.FetchImageData());
        }
        else
        {
            // the image is available, so just assign it
            ItemImage.texture = Model.thumbnailImage;
        }
    }
}

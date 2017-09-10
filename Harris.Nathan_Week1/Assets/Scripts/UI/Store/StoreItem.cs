using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour, IModelBacked<StoreItemModel>
{
    public StoreItemModel Model { get; set; }

    [SerializeField]
    private Text ItemTitle;
    [SerializeField]
    private RawImage ItemImage;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        StoreItemModel.ImageDownloadedHandler assignment = () => ItemImage.texture = Model.thumbnailImage;

        ItemTitle.text = Model.title;
        
        if (Model.thumbnailImage == null)
        {
            Model.ImageDownloaded += () =>
            {
                assignment();
                Model.ImageDownloaded -= assignment;
            };
            StartCoroutine(Model.FetchImageData());
        }
        else
        {
            ItemImage.texture = Model.thumbnailImage;
        }
    }
}

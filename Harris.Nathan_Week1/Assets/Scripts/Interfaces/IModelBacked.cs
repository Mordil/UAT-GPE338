namespace Assets.Scripts.Interfaces
{
    interface IModelBacked<T>
        where T : IJsonModel
    {
        T Model { get; set; }
    }
}

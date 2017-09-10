namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// An interface for declaring a MonoBehaviour has a backing model that can be serialized to/from JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IModelBacked<T>
        where T : IJsonModel
    {
        /// <summary>
        /// The IJsonModel that backs this object.
        /// </summary>
        T Model { get; set; }
    }
}

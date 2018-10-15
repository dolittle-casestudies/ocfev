namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHubs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>() where T: Hub;
    }
    
}
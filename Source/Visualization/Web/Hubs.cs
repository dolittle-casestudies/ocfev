using System;
using System.Collections.Concurrent;
using Dolittle.Execution;
using Dolittle.DependencyInversion;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    [Singleton]
    public class Hubs : IHubs
    {
        readonly IContainer _container;
        readonly ConcurrentDictionary<Type, Hub> _hubs = new ConcurrentDictionary<Type, Hub>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public Hubs(IContainer container)
        {
            _container = container;
        }
        
        /// <inheritdoc/>
        public T Get<T>() where T : Hub
        {
            var type = typeof(T);
            if( _hubs.ContainsKey(type)) return _hubs[type] as T;
            var hub = _container.Get<T>();
            _hubs[type] = hub;
            return hub;
        }
    }    
}
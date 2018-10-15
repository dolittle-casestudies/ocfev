using System.Collections.Generic;

namespace Web
{
    /// <summary>
    /// Represents a method call
    /// </summary>
    public class MethodCall
    {
        /// <summary>
        /// Gets of sets Name of the method to call
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets parameters for the method
        /// </summary>
        public object[] Arguments { get; set; }
    }

}
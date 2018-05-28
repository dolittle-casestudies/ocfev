/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Logging;
using Dolittle.Serialization.Json;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class VesselOrientationHub : Hub
    {
        public VesselOrientationHub(ILogger logger, ISerializer serializer) : base(logger, serializer) {}


        public void GravityChanged(float x, float y, float z)
        {
            Invoke(() => GravityChanged(x,y,z));
       }
    }
}

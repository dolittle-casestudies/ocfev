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
        public VesselOrientationHub(ILogger logger, ISerializer serializer) : base(logger, serializer) { }

        public void GravityChanged(float x, float y, float z)
        {
            Invoke(() => GravityChanged(x, y, z));
        }


        public void ThrottleChanged(int engine, double target)
        {
            Invoke(() => ThrottleChanged(engine, target));
        }


        public void ChangeGravity(double x, double y, double z)
        {
            _logger.Information($"Change gravity ({x},{y},{z})");

            GravityChanged((float)x,(float)y,(float)z);
        }
    }
}
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Logging;
using Dolittle.Serialization.Json;

namespace Web
{
    public class VesselSettingsHub : Hub
    {
        public VesselSettingsHub(ILogger logger, ISerializer serializer) : base(logger, serializer) {}

        public void IPChanged(string ip)
        {
            Invoke(() => IPChanged(ip));
        }
    }
}

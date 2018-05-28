import { inject } from 'aurelia-framework';
import { VesselOrientationHub } from './VesselOrientationHub';
import { observable } from 'aurelia-framework';

@inject(VesselOrientationHub)
export class index {
  // Speed on water
  // Kilowatt / Throttle

  // Wind speed

  // Wind direction

  // Gravity -> pitch / yaw / roll

  // Friction

  constructor(vesselOrientationHub) {
    this.orientation = vesselOrientationHub;
  }
}

import { containerless, inject } from 'aurelia-framework';
import { VesselOrientationHub } from '../../VesselOrientationHub';


@containerless()
@inject(VesselOrientationHub)
export class gravity {
  constructor(vesselOrientationHub) {
    this.orientation = vesselOrientationHub;
  }
}

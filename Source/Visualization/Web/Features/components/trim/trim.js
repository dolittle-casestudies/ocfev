import { containerless, inject } from 'aurelia-framework';
import { VesselOrientationHub } from '../../VesselOrientationHub';


@containerless()
@inject(VesselOrientationHub)
export class trim {
  constructor(vesselOrientationHub) {
    this.orientation = vesselOrientationHub;
  }
}

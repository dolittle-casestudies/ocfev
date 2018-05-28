import { inject } from 'aurelia-framework';
import { VesselOrientationHub } from "./VesselOrientationHub";
import { observable } from 'aurelia-framework';
import {BindingSignaler} from 'aurelia-templating-resources';


@inject(VesselOrientationHub, BindingSignaler)
export class index {

    // Speed on water
    // Kilowatt / Throttle

    // Wind speed

    // Wind direction

    // Gravity -> pitch / yaw / roll

    // Friction


    @observable gravityX = 42;
    @observable gravityY = 43;
    @observable gravityZ = 44;

    constructor(vesselOrientationHub, signaler) {
        this.vesselOrientationHub = vesselOrientationHub;
    }

    attached() {
        this.vesselOrientationHub._connection.on('gravityChanged', this.gravityChanged, this);

        setTimeout(() => this.gravityX = 43, 1000);
    }

    gravityChanged(x,y,z) {
        this.gravityX = x;
        this.gravityY = y;
        this.gravityZ = z;

        console.log(`Gravity changed ${x},${y},${z}`);
        //setTimeout(() => this.gravityX = 43, 1000);
    }    
}
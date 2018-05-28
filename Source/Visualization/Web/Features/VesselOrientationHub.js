import { HubConnection } from './HubConnection';
import { observable } from 'aurelia-framework';

export class VesselOrientationHub {
    @observable gravityX = 0;
    @observable gravityY = 0;
    @observable gravityZ = 0;
    
    @observable pitch = 0;
    @observable yaw = 0;
    @observable roll = 0;

    constructor() {
        this._connection = HubConnection.createFor('vessel/orientation');
        this._connection.on('gravityChanged', this.gravityChanged, this);
    }

    gravityChanged(x, y, z) {
        this.gravityX = x;
        this.gravityY = y;
        this.gravityZ = z;

        let rad2Deg = 180 / Math.PI;
        this.pitch = Math.asin(y) * rad2Deg;
        this.yaw = Math.atan2(x, z) * rad2Deg;
        this.roll = 0;       
    }        
}

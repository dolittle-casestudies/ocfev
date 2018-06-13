import { HubConnection } from './HubConnection';
import { observable } from 'aurelia-framework';

export class VesselOrientationHub {
    @observable gravityX = 0;
    @observable gravityY = 0;
    @observable gravityZ = 0;
    @observable trim = 0;
    
    @observable pitch = 0;
    @observable pitchRaw = 0;
    @observable yaw = 0;
    @observable roll = 0;

    constructor() {
        this._connection = HubConnection.createFor('vessel/orientation');
        this._connection.on('gravityChanged', this.gravityChanged, this);
        this._pitchCount = 0;
        this._pitchMovingAverage = [];
    }

    gravityChanged(x, y, z) {
        this.gravityX = x;
        this.gravityY = y;
        this.gravityZ = z;

        let rad2Deg = 180 / Math.PI;
        let deg2Rad = Math.PI / 180;

        let length = 41.4;
        let pitch = (-117.7*this.gravityY)+2.8669-1.2;

        this.pitchRaw = pitch;

        
        this._pitchMovingAverage[this._pitchCount%10] = pitch;
        this._pitchCount++;

        let sum = 0;
        this._pitchMovingAverage.forEach(value => sum += value);
        this.pitch = sum / this._pitchMovingAverage.length;

        this.trim = (Math.sin(this.pitch*deg2Rad))*(length/2);

        //this.pitch = Math.asin(y) * rad2Deg;
        //this.yaw = Math.atan2(x, z) * rad2Deg;
        //this.roll = 0;       
    }        
}

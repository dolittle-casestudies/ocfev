import { HubConnection } from './HubConnection';
import { observable, inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(EventAggregator)
export class VesselOrientationHub {
    @observable gravityX = 0;
    @observable gravityY = 0;
    @observable gravityZ = 0;
    @observable trim = 0;
    
    @observable pitch = 0;
    @observable pitchRaw = 0;
    @observable yaw = 0;
    @observable roll = 0;

    @observable throttle = 0;
    @observable has_alert = false;

    constructor(event_aggregator) {
        event_aggregator.subscribe("changeThrottle", response => {
            this.throttle = response.target;
        }); 
        this._connection = HubConnection.createFor('vessel/orientation');
        this._connection.on('gravityChanged', this.gravityChanged, this);
        this._pitchCount = 0;
        this._pitchMovingAverage = [];
        for( var i=0; i<10; i++ ) this._pitchMovingAverage[i] = 0;
        
        for( var i=0; i<10; i++ ) this.gravityChanged(0, 0.03, 0);
    }

    gravityChanged(x, y, z) {
        this.gravityX = x;
        this.gravityY = y;
        this.gravityZ = z;

        let rad2Deg = 180 / Math.PI;
        let deg2Rad = Math.PI / 180;

        let length = 42;
        
        let pitch = ((-117.7*this.gravityY)+2.8669-1.2);
        //pitch = pitch -10;
        this.pitchRaw = pitch;
       
        this._pitchMovingAverage[this._pitchCount%this._pitchMovingAverage.length] = pitch;
        this._pitchCount++;

        let sum = 0;

        this._pitchMovingAverage.forEach(value => sum += value);
        this.pitch = sum / this._pitchMovingAverage.length;

        this.trim = (Math.sin(this.pitch*deg2Rad))*(length/2);

        if( this.trim < -1 || this.trim > 0.5 ) {
            this.has_alert = true;
        } else {
            this.has_alert = false;
        }
    }        
}

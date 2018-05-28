import { HubConnection } from './HubConnection';
import { observable } from 'aurelia-framework';

export class VesselOrientationHub
{
    @observable gravityX = 0;
    @observable gravityY = 0;
    @observable gravityZ = 0;

    constructor() {
        this._connection = HubConnection.createFor('vessel/orientation');
        this._connection.on('gravityChanged', this.gravityChanged, this);
    }

    gravityChanged(x,y,z) {
        this.gravityX = x;
        this.gravityY = y;
        this.gravityZ = z;
    }        
}
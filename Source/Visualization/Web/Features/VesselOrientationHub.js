import { HubConnection } from './HubConnection';


export class VesselOrientationHub
{
    constructor() {
        this._connection = HubConnection.createFor('vessel/orientation');
        //this._connection.on('gravityChanged', this.gravityChanged);
    }

}
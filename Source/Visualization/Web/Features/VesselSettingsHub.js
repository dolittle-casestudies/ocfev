import { HubConnection } from './HubConnection';
import { observable } from 'aurelia-framework';

export class VesselSettingsHub {
    @observable ip = "[not set]";

    constructor() {}

    iPChanged(ip) {
        this.ip = ip;
    }        
}

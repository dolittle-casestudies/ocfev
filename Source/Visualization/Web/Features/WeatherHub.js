import { HubConnection } from './HubConnection';
import { observable } from 'aurelia-framework';

export class WeatherHub {
    @observable wind_speed = 0;
    @observable wind_direction = 0;

    constructor() {
        this._connection = HubConnection.createFor('weather');
        this._connection.on('weatherChanged', this.weatherChanged, this);
    }

    weatherChanged(weatherData) {
        this.wind_speed = weatherData.windSpeed;
        this.wind_direction = weatherData.windDirection;
    }        
}

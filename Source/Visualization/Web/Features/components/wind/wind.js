import { containerless, observable, inject } from 'aurelia-framework';
import { WeatherHub } from '../../WeatherHub';

@containerless()
@inject(WeatherHub)
export class wind {
  constructor(weatherHub) {
      this.weatherHub = weatherHub;
  }
}

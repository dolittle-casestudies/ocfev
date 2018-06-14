import { containerless, observable, inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@containerless()
@inject(EventAggregator)
export class speed {
  @observable speed = 0;
  @observable actual_speed = 0;

  constructor(event_aggregator) {
      event_aggregator.subscribe("changeThrottle", response => {
          this.speed = Math.pow((response.target/100) * (20 / 0.0025), 1/3);
      });      

      setInterval(() => {
          if( this.speed == 0 ) {
              this.actual_speed = 0;
          } else {
            this.actual_speed = new Number((this.speed + (Math.random()/10))).toFixed(2);
          }

      },300);
  }
}

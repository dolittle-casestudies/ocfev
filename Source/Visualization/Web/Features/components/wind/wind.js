import { containerless, observable } from 'aurelia-framework';

@containerless()
export class wind {
  @observable wind_knots = "4,3";
  @observable wind_orientation=300;
}

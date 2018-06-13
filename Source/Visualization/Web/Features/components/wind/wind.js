import { containerless, bindable } from 'aurelia-framework';

@containerless()
export class wind {
  @bindable wind_knots = '4,3';
  @bindable wind_orientation = 300;
}

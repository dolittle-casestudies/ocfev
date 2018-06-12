import { PLATFORM } from 'aurelia-pal';

export function configure(config) {
  config.globalResources(PLATFORM.moduleName('./action-bar/action_bar'));
  config.globalResources(PLATFORM.moduleName('./app_header/app_header'));
  config.globalResources(PLATFORM.moduleName('./app_footer/app_footer'));
  config.globalResources(PLATFORM.moduleName('./chart/chart'));
  config.globalResources(PLATFORM.moduleName('./map/map'));
  config.globalResources(PLATFORM.moduleName('./speed/speed'));
  config.globalResources(PLATFORM.moduleName('./throttle/throttle'));
  config.globalResources(PLATFORM.moduleName('./trim/trim'));
  config.globalResources(PLATFORM.moduleName('./wind/wind'));
}

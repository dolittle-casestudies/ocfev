import { PLATFORM } from 'aurelia-pal';
import style from '../styles/style.scss';
import { inject } from 'aurelia-dependency-injection';

export class app {
  constructor() {}

  configureRouter(config, router) {
    config.options.pushState = true;
    config.map([{ route: ['', 'index'], name: 'index', moduleId: PLATFORM.moduleName('./index'), layoutView: PLATFORM.moduleName('layout_fullscreen.html') }]);

    this.router = router;
  }
}

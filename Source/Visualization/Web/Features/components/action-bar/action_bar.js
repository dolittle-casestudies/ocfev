import { containerless, customElement, bindable, observable } from 'aurelia-framework';
import { CommandCoordinator } from '@dolittle/commands';
import { inject } from 'aurelia-dependency-injection';

import { ChangeThrottle } from './ChangeThrottle';


@customElement('action-bar')
@containerless()
@inject(CommandCoordinator)
export class action_bar {
  @bindable to;
  @observable engine_a_throttle;
  @observable engine_b_throttle;

  constructor(command_coordinator) {
    this._command_coordinator = command_coordinator;
    this.engine_a_throttle = 0;
    this.engine_b_throttle = 0;
  }

  engine_a_throttleChanged(value) {
    let command = new ChangeThrottle();
    command.engine = 0;
    command.target = value;
    this._command_coordinator.handle(command);
  }

  engine_b_throttleChanged(value) {
    let command = new ChangeThrottle();
    command.engine = 1;
    command.target = value;
    this._command_coordinator.handle(command);
  }
}

import { containerless, observable, inject } from 'aurelia-framework';
import { CommandCoordinator } from '@dolittle/commands';
import { ChangeThrottle } from './ChangeThrottle';

@containerless()
@inject(CommandCoordinator)
export class throttle {
  @observable engine_a_throttle = 0;
  @observable engine_b_throttle = 0;
  @observable engines_connected = true;
  constructor(command_coordinator) {
    this._command_coordinator = command_coordinator;
  }

  engine_a_throttleChanged(value) {
    if (this.engines_connected && this.engine_b_throttle !== value) {
      this.engine_b_throttle = value;
    }
    let command = new ChangeThrottle();
    command.engine = 0;
    command.target = value;
    this._command_coordinator.handle(command);
  }

  engine_b_throttleChanged(value) {
    if (this.engines_connected && this.engine_a_throttle !== value) {
      this.engine_a_throttle = value;
    }
    let command = new ChangeThrottle();
    command.engine = 1;
    command.target = value;
    this._command_coordinator.handle(command);
  }
}

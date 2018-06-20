import { containerless, observable, inject } from 'aurelia-framework';
import { CommandCoordinator } from '@dolittle/commands';
import { ChangeThrottle } from './ChangeThrottle';
import { EventAggregator } from 'aurelia-event-aggregator';
import { VesselSettingsHub } from '../../VesselSettingsHub';

@containerless()
@inject(CommandCoordinator, EventAggregator, VesselSettingsHub)
export class throttle {
  @observable engine_a_throttle = 0;
  @observable engine_b_throttle = 0;
  @observable engines_connected = true;
  constructor(command_coordinator, event_aggregator, vessel_settings_hub) {
    this._command_coordinator = command_coordinator;
    this._event_aggregator = event_aggregator;
    this.vessel_settings_hub = vessel_settings_hub;
  }

  engine_a_throttleChanged(value) {
    if (this.engines_connected && this.engine_b_throttle !== value) {
      this.engine_b_throttle = value;
    }
    let command = new ChangeThrottle();
    command.engine = 0;
    command.target = value;

    this._event_aggregator.publish('changeThrottle', command);
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

  engines_connectedChanged(nv, ov) {
    if (nv) {
      let avg_throttle = (parseInt(this.engine_a_throttle, 10) + parseInt(this.engine_b_throttle, 10)) / 2;
      this.engine_a_throttle = avg_throttle;
    }
  }
}

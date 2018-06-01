import { Command } from '@dolittle/commands';
//this.type = 'PortCostPlanner#commodity_planner.port_commodities-record_price_for_commodity_in_port+Command|Domain';

export class ChangeThrottle extends Command {
  constructor() {
    super();
    this.type = 'OCFEV#visualization.Vessels-ChangeThrottle+Command|Domain';
    this.engine = 0;
    this.target = 0;
  }
}

import {containerless, customElement, bindable} from 'aurelia-framework';

@customElement('action-bar')
@containerless()
export class action_bar {
   @bindable to;
}

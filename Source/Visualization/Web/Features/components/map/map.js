import L from 'leaflet';
import map_marker from './map_marker_icon';


export class map {
  constructor() {
  }

  attached() {
    if (!this.map) {
      this.map = this._createMap();
    }
  }
  
  _createMap() {
    let _map = new L.Map(this.map_container, {
      center: [0, 0],
      zoom: 2
    });

    L.tileLayer('https://maps.wikimedia.org/osm-intl/{z}/{x}/{y}.png', {
      attribution: '<a href="https://wikimediafoundation.org/wiki/Maps_Terms_of_Use">Wikimedia</a>'
    }).addTo(_map);

    return _map;
  }
}

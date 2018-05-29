import { containerless, bindable } from 'aurelia-framework';
import L from 'leaflet';
import map_marker from './map_marker_icon';

@containerless()
export class map {
  constructor() {}

  /*
  map.on('click', function(e) {
    alert("Lat, Lon : " + e.latlng.lat + ", " + e.latlng.lng)
});
*/

  attached() {
    if (!this.map) {
      this.map = this._createMap();
    }
  }

  _createMap() {
    let _map = new L.Map(this.map_container, {
      center: [59.11927, 10.223576],
      zoom: 12
    });

    L.tileLayer('https://maps.wikimedia.org/osm-intl/{z}/{x}/{y}.png', {
      attribution: '<a href="https://wikimediafoundation.org/wiki/Maps_Terms_of_Use">Wikimedia</a>'
    }).addTo(_map);
    _map.on('click', function(e) {
      console.log('Lat, Lon : ' + e.latlng.lat + ', ' + e.latlng.lng);
    });
    return _map;
  }
}

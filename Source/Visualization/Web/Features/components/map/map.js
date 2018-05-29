import L from 'leaflet';
import map_marker from './map_marker_icon';

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
      center: [0, 0],
      zoom: 2
    });

    L.tileLayer('https://maps.wikimedia.org/osm-intl/{z}/{x}/{y}.png', {
      attribution: '<a href="https://wikimediafoundation.org/wiki/Maps_Terms_of_Use">Wikimedia</a>'
    }).addTo(_map);
    _map.on('click', function(e) {
      console.log('Lat, Lon : ' + e.latlng.lat + ', ' + e.latlng.lng);
      console.log(e);
    });
    return _map;
  }
}

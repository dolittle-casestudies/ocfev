import { containerless } from 'aurelia-framework';
import L from 'leaflet';
import map_marker from './map_marker_icon';

@containerless()
export class map {
  constructor() {}

  /*
    let marker = L.marker(new L.LatLng(59.1260972089286, 10.227586687801676), {draggable: true}).addTo(map);
    marker.on('dragend', function (e) {
        document.getElementById('latitude').value = marker.getLatLng().lat;
        document.getElementById('longitude').value = marker.getLatLng().lng;
    });
  */

  attached() {
    if (!this.map) {
      this.map = this._createMap();
      let _map_marker = new map_marker();
      let _latLong = { latitude: 59.1260972089286, longitude: 10.227586687801676 };
      let marker = this._createPortMarker(_latLong, _map_marker);
      marker.addTo(this.map);
    }
  }

  _createMap() {
    let _map = new L.Map(this.map_container, {
      center: [59.11927, 10.223576],
      zoom: 12
    });

    // L.tileLayer('https://maps.wikimedia.org/osm-intl/{z}/{x}/{y}.png', {
    //   attribution: '<a href="https://wikimediafoundation.org/wiki/Maps_Terms_of_Use">Wikimedia</a>'
    // }).addTo(_map);
    L.tileLayer('https://{s}.tile.openstreetmap.se/hydda/base/{z}/{x}/{y}.png', {
      attribution:
        'Tiles courtesy of <a href="http://openstreetmap.se/" target="_blank">OpenStreetMap Sweden</a> &mdash; Map data &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(_map);
    _map.on('click', function(e) {
      console.log('Lat, Lon : ' + e.latlng.lat + ', ' + e.latlng.lng);
    });
    return _map;
  }

  _createPortMarker(latLong, markerIcon) {
    let _marker = L.marker([latLong.latitude, latLong.longitude], { icon: markerIcon, riseOnHover: true, draggable: true });
    _marker.on('dragend', function(e) {
      console.log('Lat, Lon : ' + _marker.getLatLng().lat + ', ' + _marker.getLatLng().lng);
    });
    return _marker;
  }
}

import { containerless, observable, inject } from 'aurelia-framework';
import L from 'leaflet';
import lr from 'leaflet-rotatedmarker';
import map_marker from './map_marker_icon';
import { HttpClient } from 'aurelia-http-client';

@containerless()
@inject(HttpClient)
export class map {
    @observable vessel_marker_rotation = 23;
    constructor(httpClient) {
        this.vessel_marker;
        this.lat_long_center_init = { lat: 59.11927, lng: 10.223576 };
        this.vessel_marker_location = this._get_vessel_location_from_local_storage();
        this._httpClient = httpClient;
    }

    attached() {
        if (!this.map) {
            this.map = this._createMap();
            if (this.vessel_marker_location) {
                this._add_vessel_marker(this.vessel_marker_location);
                //replace map
            }
        }
    }

    vessel_marker_rotationChanged(nv, ov) {
        console.log(nv, ov);
        this._update_vessel_marker(this.vessel_marker_location);
    }

    _createMap() {
        let _self = this;
        let _map = new L.Map(this.map_container, {
            center: [this.lat_long_center_init.lat, this.lat_long_center_init.lng],
            zoom: 12
        }).on('click', function (e) {
            let _latlng = { lat: e.latlng.lat, lng: e.latlng.lng };
            _self._update_vessel_marker(_latlng);
        });

        L.tileLayer('https://{s}.tile.openstreetmap.se/hydda/base/{z}/{x}/{y}.png', {
            attribution:
                'Tiles courtesy of <a href="http://openstreetmap.se/" target="_blank">OpenStreetMap Sweden</a> &mdash; Map data &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(_map);

        return _map;
    }
    _add_vessel_marker(latlng) {
        let _map_marker = new map_marker();
        this.vessel_marker = this._create_vessel_marker(latlng, _map_marker);
        this.vessel_marker.addTo(this.map);
        this._update_vessel_location_storage(latlng);
        this._update_vessel_location(latlng);
    }

    _update_vessel_marker(latlng) {
        if (this.vessel_marker) {
            this.map.removeLayer(this.vessel_marker);
        }
        this._add_vessel_marker(latlng);
    }

    _update_vessel_marker_location() {
        this.vessel_marker_location = this._get_vessel_location_from_local_storage();
    }
    _create_vessel_marker(latlng, markerIcon) {
        let _self = this;
        let _marker = L.marker([latlng.lat, latlng.lng], {
            icon: markerIcon,
            riseOnHover: true,
            draggable: true,
            autoPan: true,
            rotationAngle: _self.vessel_marker_rotation
        });
        _marker.on('dragend', function (e) {
            var latlng = _marker.getLatLng();
            _self._update_vessel_location_storage(latlng);
            _self._update_vessel_location(latlng);
        });
        return _marker;
    }

    _update_vessel_location_storage(latlng) {
        localStorage.setItem('vessel_latitude', latlng.lat);
        localStorage.setItem('vessel_longitude', latlng.lng);
        this._update_vessel_marker_location();
    }
    _get_vessel_location_from_local_storage() {
        let vessel_lat = localStorage.getItem('vessel_latitude');
        let vessel_lng = localStorage.getItem('vessel_longitude');
        if (vessel_lat === null || vessel_lng === null) {
            return null;
        }
        let _latlng = { lat: vessel_lat, lng: vessel_lng };
        return _latlng;
    }

    _update_vessel_location(latlng) {
        this._httpClient.createRequest(`/api/location?latitude=${latlng.lat}&longitude=${latlng.lng}`)
            .asGet()
            .send()
            .then(result => {
                console.log("Location sent");
            });        
    }
}

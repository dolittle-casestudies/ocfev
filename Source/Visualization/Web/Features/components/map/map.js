import { containerless, observable, inject } from 'aurelia-framework';
import L from 'leaflet';
import map_marker from './map_marker_icon';
import { HttpClient } from 'aurelia-http-client';
import 'leaflet-rotatedmarker';
import vessel_icon from './vessel_control_icon.svg';

@containerless()
@inject(HttpClient)
export class map {
    @observable vessel_marker_rotation = 0;

    constructor(httpClient) {
        this.vessel_marker;
        this.lat_long_center_init = { lat: 59.11927, lng: 10.223576 };
        this.vessel_marker_location;
        this._httpClient = httpClient;
    }

    attached() {
        if (!this.map) {
            this.map = this._createMap();
            if (this.vessel_marker_location) {
                this._add_vessel_marker(this.vessel_marker_location);
                this.map.setView(this.vessel_marker_location, 10);
            }
        }
        this.vessel_marker_rotation = parseInt(this._get_vessel_rotation_from_local_storage(), 10);
    }

    vessel_marker_rotationChanged(nv, ov) {
        this._update_vessel_rotation_storage();
        this._update_vessel_marker(this.vessel_marker_location);
    }

    _createMap() {
        let self = this;
        let _map = new L.Map(this.map_container, {
            center: [this.lat_long_center_init.lat, this.lat_long_center_init.lng],
            zoom: 10
        }).on('click', function(e) {
            let _latlng = { lat: e.latlng.lat, lng: e.latlng.lng };
            self._update_vessel_marker(_latlng);
        });

        L.tileLayer('https://{s}.tile.openstreetmap.se/hydda/base/{z}/{x}/{y}.png', {
            attribution:
                'Tiles courtesy of <a href="http://openstreetmap.se/" target="_blank">OpenStreetMap Sweden</a> &mdash; Map data &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(_map);

        this._create_vessel_control(self);
        L.control.vessel({ position: 'topright' }).addTo(_map);

        return _map;
    }

    _create_vessel_control(self) {
        L.Control.Vessel = L.Control.extend({
            onAdd: function() {
                let wrapper = L.DomUtil.create('div');
                L.DomUtil.addClass(wrapper, 'vessel_control');
                L.DomUtil.addClass(wrapper, 'leaflet-bar');

                let center_button = L.DomUtil.create('a', 'center_map', wrapper);
                L.DomUtil.addClass(center_button, 'center_map_to_vessel');
                center_button.title = 'Center map to vessel location';
                center_button.setAttribute('role', 'button');
                let icon = L.DomUtil.create('img', 'vessel_icon', center_button);
                icon.src = vessel_icon;
                // center_button.innerHTML = '&#9737;';

                let rotate_cw = L.DomUtil.create('a', 'rotate_cw', wrapper);
                rotate_cw.title = 'rotate the vessel clockwise';
                rotate_cw.setAttribute('role', 'button');
                rotate_cw.innerHTML = '&cudarrl;';

                let rotate_ccw = L.DomUtil.create('a', 'rotate_ccw', wrapper);
                rotate_ccw.title = 'rotate the vessel counter clockwise';
                rotate_ccw.setAttribute('role', 'button');
                rotate_ccw.innerHTML = '&cudarrl;';

                L.DomEvent.disableClickPropagation(center_button);
                L.DomEvent.disableClickPropagation(rotate_cw);
                L.DomEvent.disableClickPropagation(rotate_ccw);
                L.DomEvent.on(center_button, 'click ', this._center_map_to_vessel);
                L.DomEvent.on(rotate_cw, 'click', this._rotate_vessel_cw);
                L.DomEvent.on(rotate_ccw, 'click', this._rotate_vessel_ccw);

                return wrapper;
            },
            onRemove: function() {
                L.DomEvent.off(center_button, 'click ', this._center_map_to_vessel);
                L.DomEvent.off(rotate_cw, 'click', this._rotate_vessel_cw);
                L.DomEvent.off(rotate_ccw, 'click', this._rotate_vessel_ccw);
            },
            _center_map_to_vessel: function(ev) {
                if (!self.vessel_marker_location) {
                    let _latlng = { lat: 59.90700614346053, lng: 10.75143098831177 };
                    self._add_vessel_marker(_latlng);
                    self.vessel_marker_rotation = -65;
                }
                self.map.setView(self.vessel_marker_location, 16);
            },
            _rotate_vessel_cw: function(ev) {
                self.vessel_marker_rotation += 5;
            },
            _rotate_vessel_ccw: function(ev) {
                self.vessel_marker_rotation -= 5;
            }
        });

        L.control.vessel = function(opts) {
            return new L.Control.Vessel(opts);
        };
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
        let self = this;
        let _marker = L.marker([latlng.lat, latlng.lng], {
            icon: markerIcon,
            riseOnHover: true,
            draggable: true,
            autoPan: true,
            rotationAngle: self.vessel_marker_rotation
        });
        _marker.on('dragend', function(e) {
            let _latlng = _marker.getLatLng();
            self._update_vessel_location_storage(_latlng);
            self._update_vessel_location(_latlng);
        });
        return _marker;
    }

    _update_vessel_rotation_storage() {
        localStorage.setItem('vessel_rotation', this.vessel_marker_rotation);
    }

    _get_vessel_rotation_from_local_storage() {
        let rotation = localStorage.getItem('vessel_rotation');
        if (!rotation) {
            return 0;
        }
        return rotation;
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
        this._httpClient
            .createRequest(`/api/location?latitude=${latlng.lat}&longitude=${latlng.lng}`)
            .asGet()
            .send()
            .then(result => {
                console.log('Location sent');
            });
    }
}

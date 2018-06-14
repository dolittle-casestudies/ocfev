import vessel_marker from './vessel.png';
import L from 'leaflet';

class map_marker {
    constructor() {
        this.iconSize = [18, 61];
        this.iconAnchor = [9, 30];
        this.popupAnchor = [0, -61];
        this.iconUrl = this._get_icon_file();
    }

    _get_icon_file(category) {
        return vessel_marker;
    }
}

export default function() {
    let _map_marker = new map_marker();
    return L.icon({
        iconUrl: _map_marker.iconUrl,
        iconSize: _map_marker.iconSize,
        iconAnchor: _map_marker.iconAnchor,
        popupAnchor: _map_marker.popupAnchor
    });
}

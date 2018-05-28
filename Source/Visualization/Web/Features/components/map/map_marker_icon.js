import blue_marker from './icons/port_sign_blue.png';
import green_marker from './icons/port_sign_green.png';
import dark_red_marker from './icons/port_sign_dark_red.png';
import orange_marker from './icons/port_sign_orange.png';
import yellow_marker from './icons/port_sign_yellow.png';
import grey_marker from './icons/port_sign_grey.png';
import black_marker from './icons/port_sign_black.png';
import L from 'leaflet';

class map_marker {
  constructor(price_category, port_suitable) {
    this.iconSize = [32, 32];
    this.iconAnchor = [16, 32];
    this.popupAnchor = [0, -32];
    this.iconUrl = this._get_icon_file(price_category, port_suitable);
  }

  _get_icon_file(category, suitable) {
    if (!suitable) {
      return black_marker;
    }
    return blue_marker;

    switch (category) {
      case 'free':
        return blue_marker;
      case 'cheapest':
        return blue_marker;  
      case 'below_average':
        return green_marker;
      case 'average':
        return yellow_marker;
      case 'above_average':
        return orange_marker;
      case 'highest_prices':
        return dark_red_marker;
      default:
        return grey_marker;
    }
  }
}

export default function(price_category, port_suitable) {
  let _map_marker = new map_marker(price_category, port_suitable);
  return L.icon({
    iconUrl: _map_marker.iconUrl,
    iconSize: _map_marker.iconSize,
    iconAnchor: _map_marker.iconAnchor,
    popupAnchor: _map_marker.popupAnchor
  });
}

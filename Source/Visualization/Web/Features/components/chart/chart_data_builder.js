export function build_datasets(
  automatic_measurements,
  lower_limit_inclusive,
  upper_limit,
  manual_measurements
) {
  return {
    datasets: [
      {
        label: 'Automatic read',
        data: automatic_measurements,
        fill: false,
        pointRadius: 2,
        borderColor: 'rgba(66,130,208,1)'
      },
      {
        label: 'Suggested max-value',
        data: upper_limit,
        fill: false,
        pointRadius: 0,
        borderColor: 'rgba(225,95,85,.5)'
      },
      {
        label: 'Suggested min-value',
        data: lower_limit_inclusive,
        fill: false,
        pointRadius: 0,
        borderColor: 'rgba(225,95,85,.5)'
      },
      {
        label: 'Manual read',
        data: manual_measurements,
        backgroundColor: 'rgba(167,217,188,.5)',
        borderColor: 'rgba(167,217,188,1)',
        showLine: false,
        pointStyle: 'circle',
        pointRadius: 10,
        pointHoverRadius: 12
      }
    ]
  };
}

export function build_options(titleText) {
  return {
    title: {
      display: true,
      text: titleText,
      fontSize: 24,
      padding: 20
    },
    responsive: true,
    maintainAspectRatio: true,
    legend: {
      position: 'right',
      display: false
    },
    layout: {
      padding: {
        left: 20,
        right: 20,
        top: 20,
        bottom: 20
      }
    },
    scales: {
      xAxes: [
        {
          type: 'time',
          distribution: 'series'
        }
      ]
    }
  };
}

export function map_time_value_format(time_value) {
  return { t: time_value.time, y: time_value.value };
}

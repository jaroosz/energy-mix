# Energy Mix API

Backend API for analyzing energy generation data and findong optimal EV charging hours based on clean energy availability.

## Description

This API fetches real-time energy generation data from UK Carbon Intensity API, which provides:
- daily energy mix breakdown with all types of energy percentages
- optimal charging hour recommendations for EV

## Build With

- ASP.NET Core 8.0 - Minimal API
- xUnit - Unit testing for backend logic
- Selenium WebDriver - Automated E2E UI testing (MS Edge)

## API Endpoints

### GET /api/energy-mix

Returns energy mix for three consecutive days (today, tomorrow, day after tomorrow).

Response:
```json
{
  "days": [
    {
      "date": "2026-01-02T00:00:00Z",
      "sources": {
        "biomass": 5.12,
        "coal": 0,
        "imports": 11.38,
        "gas": 15.19,
        "nuclear": 12.16,
        "other": 0,
        "hydro": 0,
        "solar": 2.93,
        "wind": 53.17
      },
      "cleanEnergyPercent": 73.4
    }
  ]
}
```

### GET /api/optimal-window?hours={1-6}

Returns optimal charging hours with the highest clean energy percentage for next two days.

Parameters:
- 'hours' (required): Charging duration in hours (1-6)

Response (int hours = 3):
```json
{
  "startTime": "2026-01-03T14:30:00Z",
  "endTime": "2026-01-03T17:30:00Z",
  "cleanEnergyPercent": 68.5
}
```

## Setup

Prerequisites:
- .NET 8.0 SDK or later

Installation:

1. Clone the repository:
```bash
git clone https://github.com/jaroosz/energy-mix-backend.git
cd energy-mix-backend
```

2. Run application:
```bash
dotnet run --project EnergyMixApi
```

3. Access Swagger UI:
```
https://localhost:7250/swagger
```

## External API

This project uses the [Carbon Intensity API](https://carbonintensity.org.uk/) to fetch UK energy generation data.

Note:
The API provides forecast for approximately 36-40 hours ahead.
The third day may have incomplete data, in which case averages are calculated from available intervals.

## CORS Configuration

The API is configured to accept requests from:
- `http://localhost:3000`

## Author

Dawid Jarosz

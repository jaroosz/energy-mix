/** Single energy source with display properties */
export interface EnergySource {
    name: string;
    value: number;
    color: string;
}

/** Energy sources data from API (source name -> percentage) */
export interface EnergySources {
    [key: string]: number;
}

/** Energy data for single day */
export interface DailyEnergyData {
    date: string;
    sources: EnergySources;
    cleanEnergyPercent: number;
}

/** API response containing data for 3 consecutive days (today, tomorrow, day after tomorrow) */
export interface EnergyMixResponse {
    days: DailyEnergyData[];
}

/** Donut chart segment with angles for SVG rendering */
export interface ChartSegment extends EnergySource {
    startAngle: number;
    endAngle: number;
    angle: number;
}

/** Optimal charging window response from API */
export interface OptimalWindow {
    startTime: string;
    endTime: string;
    cleanEnergyPercent: number;
}
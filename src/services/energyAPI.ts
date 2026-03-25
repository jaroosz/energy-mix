import { EnergyMixResponse, OptimalWindow } from '../types/energy';

const getApiBaseUrl = (): string => {
  if (process.env.REACT_APP_API_URL) {
    return process.env.REACT_APP_API_URL;
  }
  return `https://${window.location.hostname}:7250/api`;
};

const API_BASE_URL =  getApiBaseUrl();

/**
 * Fetch energy mix data for 3 consecutive days from API.
 * @returns Promise containing daily energy source percentages and total clean energy percentage
 * @throws Error if the API fails
 */
export const fetchEnergyMix = async (): Promise<EnergyMixResponse> => {
    const response = await fetch(`${API_BASE_URL}/energy-mix`);

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
};

/**
 * Fetch optimal charging window for a given hours.
 * @param hours - Duration of charging in hours (1-6)
 * @returns Promise containing start time, end time, and clean energy percentage
 * @throws Error if the API fails
 */
export const fetchOptimalWindow = async (hours: number): Promise<OptimalWindow> => {
    const response = await fetch(`${API_BASE_URL}/optimal-window?hours=${hours}`);

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
};
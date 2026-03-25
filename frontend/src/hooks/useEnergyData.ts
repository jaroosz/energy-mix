import { useState, useEffect, useCallback } from 'react';
import { EnergyMixResponse } from '../types/energy';
import { fetchEnergyMix } from '../services/energyAPI';

/**
 * Hook for fetching and managing energy mix data.
 * Fetches data and provides refetch capability.
 * 
 * @returns {EnergyMixResponse | null} energyData - The fetched energy data
 * @returns {boolean} loading - Whether data is being fetched
 * @returns {string | null} error - Error message if fetch failed
 * @returns {Function} refetch - Function to manually refetch data
 */
export const useEnergyData = () => {
    const [energyData, setEnergyData] = useState<EnergyMixResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const loadData = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await fetchEnergyMix();
            setEnergyData(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to fetch energy data');
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        loadData();
    }, [loadData]);

    return { energyData, loading, error, refetch: loadData };
};
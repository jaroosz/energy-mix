import { useState } from 'react';
import { fetchOptimalWindow } from '../services/energyAPI';
import { OptimalWindow } from '../types/energy';

/**
 * Hook for fetching optimal charging window data.
 * 
 * @returns {OptimalWindow | null} optimalWindow - The optimal window data
 * @returns {boolean} loading - Whether data is being fetched
 * @returns {string | null} error - Error message if fetch failed
 * @returns {Function} fetchWindow - Function to fetch optimal window for given duration
 */
export const useOptimalWindow = () => {
    const [optimalWindow, setOptimalWindow] = useState<OptimalWindow | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    /**
     * Fetches optimal charging window
     * @param duration - Charging duration in hours (1-6)
     */
    const fetchWindow = async (duration: number) => {
        try {
            setLoading(true);
            setError(null);
            const data = await fetchOptimalWindow(duration);
            setOptimalWindow(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to fetch optimal window');
        } finally {
            setLoading(false);
        }
    };

    return { optimalWindow, loading, error, fetchWindow };
};
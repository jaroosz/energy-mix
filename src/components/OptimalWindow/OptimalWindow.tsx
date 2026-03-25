import React, { useEffect, useRef } from 'react';
import './OptimalWindow.css';
import { OptimalWindow } from '../../types/energy';

interface OptimalWindowCardProps {
    optimalWindow: OptimalWindow | null;
    loading: boolean;
    error: string | null;
    chargingDuration: number;
    onDurationChange: (duration: number) => void;
    onFind: () => void;
}

/**
 * Component for finding the optimal EV charging window in next 2 days.
 * User can change size of the window (1-6 hours).
 * After submitting display the window start and end time with highest average clean energy percentage.
 */
export const OptimalWindowCard: React.FC<OptimalWindowCardProps> = ({
    optimalWindow,
    loading,
    error,
    chargingDuration,
    onDurationChange,
    onFind
}) => {
    const resultRef = useRef<HTMLDivElement>(null);

    // Auto-scroll to results when optimal window is found
    useEffect(() => {
        if (optimalWindow && resultRef.current) {
            setTimeout(() => {
                window.scrollTo({
                    top: document.documentElement.scrollHeight,
                    behavior: 'smooth'
                });
            }, 125);
        }
    }, [optimalWindow]);


    // Formats ISO date to format that is easier to read.
    const formatDateTime = (isoString: string): string => {
        const date = new Date(isoString);
        return date.toLocaleString('en-GB', {
            year: 'numeric',
            month: 'short',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        }).replace(',', '');
    };

    return (
        <div className='optimal-window-container'>
            <h2 className='section-title'>EV Charging Optimizer</h2>
            <p className='section-subtitle'>
                Find the optimal charging window with maximum clean energy
            </p>

            <div className='selector-container'>
                {/* Window duration slider with dynamic gradient fill */}
                <div className='duration-selector'>
                    <label htmlFor="charging-duration-slider">
                        Select Charging Duration: <span className='duration-value'>{chargingDuration} Hours</span>
                    </label>
                    <input
                        id="charging-duration-slider"
                        type='range'
                        min='1'
                        max='6'
                        value={chargingDuration}
                        onChange={(e) => onDurationChange(Number(e.target.value))}
                        className='duration-slider'
                        style={{
                            background: `linear-gradient(to right, #059669 0%, #059669 ${((chargingDuration - 1) / 5) * 100}%, #e5e7eb ${((chargingDuration - 1) / 5) * 100}%, #e5e7eb 100%)`
                        }}
                    />
                    <div className='slider-labels'>
                        {[1, 2, 3, 4, 5, 6].map(num => (
                            <span key={num}>{num}</span>
                        ))}
                    </div>
                </div>

                {/* Submit button with loading and error states */}
                <button
                    className={`find-window-button ${loading ? 'loading' : ''} ${error ? 'error' : ''}`}
                    onClick={onFind}
                    disabled={loading}
                >
                    {loading ? (
                        <>
                            <span className='spinner'></span>
                            Loading...
                        </>
                    ) : error ? (
                        <>
                            <span className='error-icon'>⚠</span>
                            Error - Try Again
                        </>
                    ) : (
                        'Find Optimal Window'
                    )}
                </button>

                {/* After successful API response, show results card */}
                {optimalWindow && (
                    <div className="optimal-window-result" ref={resultRef}>
                        <div className="optimal-window-icon">
                            <svg width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                <path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z" />
                            </svg>
                        </div>

                        <h3>Optimal Charging Window</h3>
                        <p className="optimal-window-subtitle">
                            Best time to charge your EV with maximum clean energy
                        </p>

                        <div className="optimal-window-details">
                            <div className="detail-box">
                                <span className="detail-label">Start Time</span>
                                <span className="detail-value">{formatDateTime(optimalWindow.startTime)}</span>
                            </div>

                            <div className="detail-box">
                                <span className="detail-label">End Time</span>
                                <span className="detail-value">{formatDateTime(optimalWindow.endTime)}</span>
                            </div>

                            <div className="detail-box">
                                <span className="detail-label">Clean Energy</span>
                                <span className="detail-value clean-energy">
                                    {optimalWindow.cleanEnergyPercent.toFixed(1)}%
                                </span>
                            </div>
                        </div>

                        <div className="optimal-window-info">
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                                <circle cx="12" cy="12" r="10" fill="none" stroke="currentColor" strokeWidth="2" />
                                <path d="M12 16v-4M12 8h.01" />
                            </svg>
                            <p>
                                This charging window offers the highest percentage of renewable energy sources
                                during your selected {chargingDuration}-hour duration.
                            </p>
                        </div>
                    </div>
                )}
            </div>

        </div>
    );
};
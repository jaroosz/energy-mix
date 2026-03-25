import React, { useMemo, useState } from 'react';
import './EnergyChart.css';
import { EnergySources, EnergySource } from '../../types/energy';
import { ENERGY_COLORS, CLEAN_SOURCES, DEFAULT_COLOR } from '../../constants/energyColors';
import EnergyPieChart from './EnergyPieChart';
import EnergySourcesList from './EnergySourcesList';

interface EnergyCardProps {
  date: string;
  sources: EnergySources;
  cleanEnergyPercent: number;
  loading: boolean;
}


/**
 * Displays a single day's energy mix data with an interactive pie chart and sources list.
 * First display clean energy sources, followed by non-clean sources.
 * Hovering on either the donut chart or the list highlights the corresponding source in both.
 */
const EnergyCard: React.FC<EnergyCardProps> = ({ 
  date, 
  sources, 
  cleanEnergyPercent, 
  loading 
}) => {
  const [hoveredSource, setHoveredSource] = useState<string | null>(null);

  /**
   * Transform energy sources object into sorted array and:
   * - filter out sources with 0 value
   * - sort clean sources first (by descending percentage)
   * - sort non-clean sources (by descending percentage)
   * - assign colors from ENERGY_COLORS
   */
  const sourcesList = useMemo((): EnergySource[] => {
    const entries = Object.entries(sources).filter(([, value]) => value > 0);
    
    const cleanEntries = entries.filter(([name]) => CLEAN_SOURCES.includes(name));
    const dirtyEntries = entries.filter(([name]) => !CLEAN_SOURCES.includes(name));

    const sortedClean = cleanEntries.sort(([, a], [, b]) => b - a);
    const sortedDirty = dirtyEntries.sort(([, a], [, b]) => b - a);
    

    const sortedEntries = [...sortedClean, ...sortedDirty];
    
    return sortedEntries.map(([name, value]) => ({
      name,
      value,
      color: ENERGY_COLORS[name] || DEFAULT_COLOR
    }));
  }, [sources]);

  return (
    <div className="energy-card">
      <EnergyPieChart
        date={date}
        sources={sources}
        cleanEnergyPercent={cleanEnergyPercent}
        loading={loading}
        hoveredSource={hoveredSource}
        onHoverSource={setHoveredSource}
      />
      
      <EnergySourcesList
        sources={sourcesList}
        hoveredSource={hoveredSource}
        onHoverSource={setHoveredSource}
      />
    </div>
  );
};

export default EnergyCard;
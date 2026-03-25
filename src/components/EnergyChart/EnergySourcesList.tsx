import React from 'react';
import { EnergySource } from '../../types/energy';
import { ENERGY_ICONS, CLEAN_SOURCES } from '../../constants/energyColors';

interface EnergySourcesListProps {
    sources: EnergySource[];
    hoveredSource: string | null;
    onHoverSource: (name: string | null) => void;
}

/**
 * Renders a list of energy sources with their percentages and small icons.
 * Visual distinction for clean and non-clean sources
 * Hover interactions that sync with EnergyPieChart
 */
const EnergySourcesList: React.FC<EnergySourcesListProps> = ({ 
    sources, 
    hoveredSource, 
    onHoverSource 
}) => {
    return (
        <ul className='energy-sources-list'>
            {sources.map((source) => {
                const Icon = ENERGY_ICONS[source.name] || ENERGY_ICONS.other;
                const isClean = CLEAN_SOURCES.includes(source.name);
                const isHovered = hoveredSource === source.name;
                
                return (
                    <li 
                        key={source.name} 
                        className={`energy-source-item ${isClean ? 'clean-source' : ''} ${isHovered ? 'hovered' : ''}`}
                        onMouseEnter={() => onHoverSource(source.name)}
                        onMouseLeave={() => onHoverSource(null)}
                    >
                        <div className='source-info'>
                            <span
                                className={`energy-source-icon ${isClean ? 'clean-source' : ''}`}
                                style={{ backgroundColor: source.color }}
                                aria-hidden='true'
                            >
                                <Icon size={20} color="white" />
                            </span>
                            <span className='source-name'>
                                {source.name.charAt(0).toUpperCase() + source.name.slice(1)}
                            </span>
                        </div>
                        <span className='source-percentage'>
                            {source.value.toFixed(1)}%
                        </span>
                    </li>
                );
            })}
        </ul>
    );
};

export default EnergySourcesList;
import React, { useMemo } from 'react';
import { ChartSegment, EnergySources } from '../../types/energy';
import { ENERGY_COLORS, CLEAN_SOURCES, DEFAULT_COLOR } from '../../constants/energyColors';
import { RiLeafLine } from '@remixicon/react';

interface EnergyPieChartProps {
  date: string;
  sources: EnergySources;
  cleanEnergyPercent: number;
  loading: boolean;
  hoveredSource: string | null;
  onHoverSource: (name: string | null) => void;
}

/**
 * Render interactive SVG donut chart displaying energy sources.
 * Animated chart reveal on load, with hover interactions that sync with EnergySourcesList.
 */
const EnergyPieChart: React.FC<EnergyPieChartProps> = ({
  date,
  sources,
  cleanEnergyPercent,
  loading,
  hoveredSource,
  onHoverSource
}) => {
  /**
   * Process sources data into chart segments, each one with start/end angle for SVG arc rendering.
   * Formats the date to display in the header.
   */
  const { formattedDate, segments } = useMemo(() => {
    const entries = Object.entries(sources).filter(([, value]) => value > 0);

    // Sorting by: clean sources, non-clean sources, both by percentage descending
    const cleanEntries = entries.filter(([name]) => CLEAN_SOURCES.includes(name));
    const dirtyEntries = entries.filter(([name]) => !CLEAN_SOURCES.includes(name));

    const sortedClean = cleanEntries.sort(([, a], [, b]) => b - a);
    const sortedDirty = dirtyEntries.sort(([, a], [, b]) => b - a);

    const sortedEntries = [...sortedClean, ...sortedDirty];

    const formattedDate = new Date(date).toLocaleDateString('en-GB', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });

    // Calculate cumulative angles for each segment
    let currentAngle = 0;
    const segments: ChartSegment[] = sortedEntries.map(([name, value]) => {
      const angle = (value / 100) * 360;
      const segment: ChartSegment = {
        name,
        value,
        color: ENERGY_COLORS[name] || DEFAULT_COLOR,
        startAngle: currentAngle,
        endAngle: currentAngle + angle,
        angle
      };
      currentAngle += angle;
      return segment;
    });

    return { formattedDate, segments };
  }, [date, sources]);

  /**
   * Calculate position of leaf icon in the middle of clean energy arc.
   * Uses polar to cartesian conversion to place icon on the arc edge.
   */
  const cleanArcEndPosition = useMemo(() => {
    const angle = (cleanEnergyPercent / 100) * 360 / 2;
    const angleRad = (angle - 90) * (Math.PI / 180);
    const radius = 125;
    
    return {
      x: 175 + radius * Math.cos(angleRad),
      y: 175 + radius * Math.sin(angleRad),
    };
  }, [cleanEnergyPercent]);

  return (
    <div className='chart-container'>
      <div className='chart-header'>
        <h3>{formattedDate}</h3>
        <p className='clean-energy-percent'>
          {cleanEnergyPercent.toFixed(1)}% Clean Energy
        </p>
      </div>

      <svg width='100%' height='100%' viewBox='0 0 350 350' className='pie-chart'>
        {/* Donut chart segments - each source rendered as a circle with stroke-dasharray */}
        {!loading && segments.map((segment) => {
          const radius = 80;
          const circumference = 2 * Math.PI * radius;
          const dashLength = (segment.angle / 360) * circumference;
          const gapLength = circumference - dashLength;
          const rotation = segment.startAngle - 90; // -90 angle to start the chart from top
          const isHovered = hoveredSource === segment.name;

          return (
            <circle
              key={segment.name}
              cx="175"
              cy="175"
              r={radius}
              fill="none"
              stroke={segment.color}
              strokeWidth={isHovered ? "65" : "50"}
              strokeDasharray={`${dashLength} ${gapLength}`}
              transform={`rotate(${rotation} 175 175)`}
              className="chart-segment"
              onMouseEnter={() => onHoverSource(segment.name)}
              onMouseLeave={() => onHoverSource(null)}
            />
          );
        })}

        {/* Outer ring showing clean energy percentage */}
        {!loading && (
          <>
            <circle
              cx="175"
              cy="175"
              r="125"
              fill="none"
              stroke="#caf3d1ff"
              strokeWidth="19"
              strokeDasharray={`${(cleanEnergyPercent / 100) * 2 * Math.PI * 125} ${2 * Math.PI * 125}`}
              transform="rotate(-90 175 175)"
              className="clean-arc"
            />
            
            {/* Leaf icon in the middle of clean energy arc */}
            <foreignObject
              x={cleanArcEndPosition.x - 9}
              y={cleanArcEndPosition.y - 9}
              width="18"
              height="18"
            >
              <RiLeafLine size={18} color="#1ba544ff" />
            </foreignObject>
          </>
        )}

        {/* Reveal animation - circle that shrinks to reveal chart */}
        <circle
          cx='175'
          cy='175'
          r='80'
          fill="none" 
          stroke="#ffffffff" 
          strokeWidth="120"
          strokeDasharray="502.65"
          strokeDashoffset="0"
          transform="rotate(-90 175 175)"
          className={!loading ? "reveal-overlay" : ""}
        />
      </svg>
    </div>
  );
};

export default EnergyPieChart;
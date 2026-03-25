import { 
  RiSunLine, 
  RiWindyLine, 
  RiWaterFlashLine, 
  RiAtomLine, 
  RiFireLine, 
  RiTempColdLine, 
  RiArrowLeftRightLine,
  RiLeafFill,
  RiBuilding3Line
} from '@remixicon/react';

/** Color mapping for each energy source type */
export const ENERGY_COLORS: { [key: string]: string } = {
    biomass: '#399735',
    nuclear: '#AFE634',
    hydro: '#2980b9',
    wind: '#77a2f8ff',
    solar: '#FFC90E',
    coal: '#2c3e50',
    gas: '#db96adff',
    imports: '#C8BFE7',
    other: '#5d4037'
};

/** List of clean energy sources */
export const CLEAN_SOURCES: string[] = [
    'biomass',
    'nuclear',
    'hydro',
    'wind',
    'solar'
];

/** Default color for unknown energy source */
export const DEFAULT_COLOR = '#95a5a6';

/** Icons for each energy source type */
export const ENERGY_ICONS: { [key: string]: React.ElementType } = {
  solar: RiSunLine,
  wind: RiWindyLine,
  hydro: RiWaterFlashLine,
  nuclear: RiAtomLine,
  gas: RiFireLine,
  coal: RiTempColdLine,
  imports: RiArrowLeftRightLine,
  biomass: RiLeafFill,
  other: RiBuilding3Line
};
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import App from './App';
import { useEnergyData } from './hooks/useEnergyData';
import { useOptimalWindow } from './hooks/useOptimalWindow';

const mockRefetch = jest.fn();
const mockFetchWindow = jest.fn();

jest.mock('./hooks/useEnergyData');
jest.mock('./hooks/useOptimalWindow');

const mockedUseEnergyData = useEnergyData as jest.MockedFunction<typeof useEnergyData>;
const mockedUseOptimalWindow = useOptimalWindow as jest.MockedFunction<typeof useOptimalWindow>;

const defaultOptimalWindowMock = {
  optimalWindow: null,
  loading: false,
  error: null,
  fetchWindow: mockFetchWindow
};

describe('App', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockedUseOptimalWindow.mockReturnValue(defaultOptimalWindowMock);
  });

  describe('Loading State', () => {
    test('displays loading message when fetching data', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: true,
        error: null,
        refetch: mockRefetch
      });

      render(<App />);
      expect(screen.getByText('Loading data...')).toBeInTheDocument();
    });

    test('displays skeleton cards while loading', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: true,
        error: null,
        refetch: mockRefetch
      });

      const { container } = render(<App />);
      // eslint-disable-next-line testing-library/no-container, testing-library/no-node-access
      const skeletons = container.querySelectorAll('.energy-card-skeleton');
      expect(skeletons).toHaveLength(3);
    });
  });

  describe('Error State', () => {
    test('displays error message when fetch fails', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: false,
        error: 'Failed to fetch energy data',
        refetch: mockRefetch
      });

      render(<App />);
      expect(screen.getByText('Failed to fetch energy data')).toBeInTheDocument();
    });

    test('displays retry button on error', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: false,
        error: 'Network error',
        refetch: mockRefetch
      });

      render(<App />);
      expect(screen.getByText('Click to retry')).toBeInTheDocument();
    });

    test('calls refetch when clicking on error banner', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: false,
        error: 'Network error',
        refetch: mockRefetch
      });

      render(<App />);
      const errorBanner = screen.getByRole('button', { name: /network error/i });
      fireEvent.click(errorBanner);
      
      expect(mockRefetch).toHaveBeenCalledTimes(1);
    });

    test('displays skeleton cards on error state', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: null,
        loading: false,
        error: 'API unavailable',
        refetch: mockRefetch
      });

      const { container } = render(<App />);
      // eslint-disable-next-line testing-library/no-container, testing-library/no-node-access
      const skeletons = container.querySelectorAll('.energy-card-skeleton');
      expect(skeletons).toHaveLength(3);
    });
  });

  describe('Success State', () => {
    const mockEnergyData = {
      days: [
        {
          date: '2026-01-06',
          sources: { solar: 30, wind: 25, coal: 20, gas: 15, nuclear: 10 },
          cleanEnergyPercent: 65
        },
        {
          date: '2026-01-07',
          sources: { solar: 35, wind: 20, coal: 25, gas: 10, nuclear: 10 },
          cleanEnergyPercent: 65
        },
        {
          date: '2026-01-08',
          sources: { solar: 40, wind: 15, coal: 20, gas: 15, nuclear: 10 },
          cleanEnergyPercent: 65
        }
      ]
    };

    test('displays energy cards when data is loaded', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: mockEnergyData,
        loading: false,
        error: null,
        refetch: mockRefetch
      });

      render(<App />);
      expect(screen.getByText('6 Jan 2026')).toBeInTheDocument();
      expect(screen.getByText('7 Jan 2026')).toBeInTheDocument();
      expect(screen.getByText('8 Jan 2026')).toBeInTheDocument();
    });

    test('does not display loading or error states when data is loaded', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: mockEnergyData,
        loading: false,
        error: null,
        refetch: mockRefetch
      });

      render(<App />);
      expect(screen.queryByText('Loading data...')).not.toBeInTheDocument();
      expect(screen.queryByText('Click to retry')).not.toBeInTheDocument();
    });

    test('does not display skeleton cards when data is loaded', () => {
      mockedUseEnergyData.mockReturnValue({
        energyData: mockEnergyData,
        loading: false,
        error: null,
        refetch: mockRefetch
      });

      const { container } = render(<App />);
      // eslint-disable-next-line testing-library/no-container, testing-library/no-node-access
      const skeletons = container.querySelectorAll('.energy-card-skeleton');
      expect(skeletons).toHaveLength(0);
    });
  });

    describe('Optimal Window Section', () => {
    beforeEach(() => {
      mockedUseEnergyData.mockReturnValue({
        energyData: {
          days: [
            { date: '2026-01-06', sources: { solar: 30, wind: 25, coal: 20, gas: 15, nuclear: 10 }, cleanEnergyPercent: 65 },
            { date: '2026-01-07', sources: { solar: 35, wind: 20, coal: 25, gas: 10, nuclear: 10 }, cleanEnergyPercent: 65 },
            { date: '2026-01-08', sources: { solar: 40, wind: 15, coal: 20, gas: 15, nuclear: 10 }, cleanEnergyPercent: 65 }
          ]
        },
        loading: false,
        error: null,
        refetch: mockRefetch
      });
    });

    test('calls fetchWindow', () => {
      mockedUseOptimalWindow.mockReturnValue(defaultOptimalWindowMock);

      render(<App />);
      const slider = screen.getByRole('slider');
      fireEvent.change(slider, { target: { value: '5' } });

      const findButton = screen.getByRole('button', { name: /find optimal window/i });
      fireEvent.click(findButton);

      expect(mockFetchWindow).toHaveBeenCalledWith(5);
    });

    test('displays error state on button when fetch fails', () => {
      mockedUseOptimalWindow.mockReturnValue({
        ...defaultOptimalWindowMock,
        error: 'Failed to fetch'
      });

      render(<App />);
      expect(screen.getByRole('button', { name: /error - try again/i })).toBeInTheDocument();
    });

    test('displays optimal window results when data is loaded', () => {
      mockedUseOptimalWindow.mockReturnValue({
        optimalWindow: {
          startTime: '2026-01-06T10:00:00Z',
          endTime: '2026-01-06T13:00:00Z',
          cleanEnergyPercent: 78.5
        },
        loading: false,
        error: null,
        fetchWindow: mockFetchWindow
      });

      render(<App />);
      expect(screen.getByText('Optimal Charging Window')).toBeInTheDocument();
      expect(screen.getByText('78.5%')).toBeInTheDocument();
    });

    test('does not display results before fetching', () => {
      mockedUseOptimalWindow.mockReturnValue(defaultOptimalWindowMock);

      render(<App />);
      expect(screen.queryByText('Optimal Charging Window')).not.toBeInTheDocument();
    });
  });
});
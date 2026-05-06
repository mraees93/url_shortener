/* eslint-disable no-useless-catch */
import { useState, useEffect, useCallback } from 'react';
import { urlApi, type ShortenedLink } from '../api/urlApi';

export const useHistory = () => {
  const [history, setHistory] = useState<ShortenedLink[]>([]);
  const [isServerStarting, setIsServerStarting] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const fetchHistory = useCallback(async () => {

    const timer = setTimeout(() => setIsServerStarting(true), 1500);

    try {
      const data = await urlApi.getHistory();
      setHistory(Array.isArray(data) ? data : []);
      setIsServerStarting(false);
    } catch (error) {
      console.error("Failed to fetch history:", error);
    } finally {
      clearTimeout(timer);
    }
  }, []);

  const shortenUrl = async (longUrl: string) => {
    setIsLoading(true);
    try {
      const result = await urlApi.shorten(longUrl);
      console.log("Shorten Result:", result);

      // Safety Check: Only update if the result is a valid object with a code
      if (result && typeof result === 'object' && result.shortCode) {
        setHistory(prev => {
          // Prevent duplicates to keep the list clean
          if (prev.some(link => link.shortCode === result.shortCode)) return prev;
          return [result, ...prev];
        });
      } else {
        // If result is invalid, re-fetch as a fallback to keep the list visible
        await fetchHistory();
      }

      return result;
    } catch (error) {
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const loadData = async () => {
      await fetchHistory();
    };

    loadData();
  }, [fetchHistory]);

  return {
    history,
    isLoading,
    shortenUrl,
    isServerStarting
  };
};

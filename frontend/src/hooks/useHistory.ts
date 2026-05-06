/* eslint-disable no-useless-catch */
import { useState, useEffect, useCallback } from 'react';
import { urlApi, type ShortenedLink } from '../api/urlApi';

export const useHistory = () => {
  const [history, setHistory] = useState<ShortenedLink[]>([]);
  const [isServerStarting, setIsServerStarting] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const fetchHistory = useCallback(async () => {

    // If the first request takes long, we assume the server is waking up
    const timer = setTimeout(() => setIsServerStarting(true), 1500);

    try {
      const data = await urlApi.getHistory();
      setHistory(data);
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
      await urlApi.shorten(longUrl);
      await fetchHistory();
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

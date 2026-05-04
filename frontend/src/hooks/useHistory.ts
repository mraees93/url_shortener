/* eslint-disable no-useless-catch */
import { useState, useEffect, useCallback } from 'react';
import { urlApi, type ShortenedLink } from '../api/urlApi';

export const useHistory = () => {
  const [history, setHistory] = useState<ShortenedLink[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchHistory = useCallback(async () => {
    try {
      const data = await urlApi.getHistory();
      setHistory(data);
    } catch (error) {
      console.error("Failed to fetch history:", error);
    }
  }, []);

  const shortenUrl = async (longUrl: string) => {
    setIsLoading(true);
    try {
      await urlApi.shorten(longUrl);
      await fetchHistory();
    } catch (error) {
      throw error; // Let App.tsx handle the alert
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
    shortenUrl
  };
};

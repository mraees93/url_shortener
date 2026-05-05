const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5219/api/url';

export interface ShortenedLink {
  shortCode: string;
  longUrl: string;
}

export const urlApi = {
  shorten: async (longUrl: string): Promise<string> => {
    const response = await fetch(`${API_BASE_URL}/shorten`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(longUrl),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || 'Failed to shorten URL');
    }

    const data = await response.json();
    return data.shortCode;
  },

  getStats: async (code: string): Promise<number> => {
    const response = await fetch(`${API_BASE_URL}/stats/${code}`);
    if (!response.ok) return 0;
    const data = await response.json();
    return data.clickCount;
  },

  getHistory: async (): Promise<ShortenedLink[]> => {
    const response = await fetch(`${API_BASE_URL}/history`);
    if (!response.ok) return [];
    return await response.json();
  },
};

const API_BASE_URL = 'http://localhost:5219/api/url';

export const urlApi = {
  shorten: async (longUrl: string): Promise<string> => {
    const response = await fetch(`${API_BASE_URL}/shorten`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(longUrl),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || 'Failed to shorten URL');
    }

    const data = await response.json();
    return data.shortCode;
  },
};

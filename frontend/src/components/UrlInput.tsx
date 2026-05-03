import React, { useState } from 'react';
import { Zap } from 'lucide-react';

interface UrlInputProps {
  onShorten: (url: string) => void;
  isLoading?: boolean;
}

export const UrlInput = ({ onShorten, isLoading }: UrlInputProps) => {
  const [url, setUrl] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (url) onShorten(url);
  };

  return (
    <form 
      onSubmit={handleSubmit} 
      className="bg-white p-2 rounded-2xl shadow-xl shadow-slate-200/50 flex gap-2 border border-slate-100"
    >
      <input 
        type="text" 
        required
        placeholder="https://very-long-link.com..."
        className="flex-1 px-4 py-3 outline-none text-slate-600 placeholder:text-slate-400 bg-transparent"
        value={url}
        onChange={(e) => setUrl(e.target.value)}
      />
      <button 
        disabled={isLoading}
        className="bg-indigo-600 hover:bg-indigo-700 disabled:bg-indigo-400 text-white px-6 py-3 rounded-xl font-semibold transition-all flex items-center gap-2"
      >
        <Zap size={18} />
        {isLoading ? 'Shortening...' : 'Shorten'}
      </button>
    </form>
  );
};

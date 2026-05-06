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
        maxLength={2000}
        required
        placeholder="https://google.com..."
        className="flex-1 px-4 py-3 outline-none text-slate-600 placeholder:text-slate-400 bg-transparent"
        value={url}
        onChange={(e) => setUrl(e.target.value)}
      />
      <button
        disabled={isLoading}
        className="bg-slate-900 text-indigo-400 hover:bg-slate-800 border border-indigo-900/30 font-bold py-3 px-6 rounded-xl transition-all shadow-lg hover:shadow-indigo-500/10 flex items-center justify-center gap-2 disabled:opacity-50"
      >
        {isLoading ? (
          <span className="animate-pulse">Processing...</span>
        ) : (
          <>
            <Zap size={18} fill="currentColor" className="text-indigo-400" />
            <span>Shorten</span>
          </>
        )}
      </button>
    </form>
  );
};

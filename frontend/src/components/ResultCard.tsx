import { useEffect, useState } from 'react';
import { Copy, Check, BarChart3 } from 'lucide-react';
import { urlApi } from '../api/urlApi';

interface ResultCardProps {
  shortUrl: string;
}

export const ResultCard = ({ shortUrl }: ResultCardProps) => {
  const [copied, setCopied] = useState(false);
  const [clicks, setClicks] = useState<number | null>(null);

  useEffect(() => {
    if (shortUrl) {
      const code = shortUrl.split('/').pop() || '';
      
      if (code) {
        urlApi.getStats(code)
          .then(setClicks)
          .catch(err => console.error("Stats fetch error:", err));
      }
    }
  }, [shortUrl]);

  const copyToClipboard = () => {
    if (!shortUrl) return;
    navigator.clipboard.writeText(shortUrl);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  if (!shortUrl) return null;

  return (
    <div className="animate-in fade-in slide-in-from-top-4 duration-500 bg-indigo-50 border border-indigo-100 p-4 rounded-xl flex items-center justify-between">
      <span className="text-indigo-700 font-medium truncate mr-4">
        {shortUrl}
      </span>
      <div className="flex items-center gap-1 bg-slate-100 text-slate-600 px-2 py-1 rounded-md text-xs font-bold uppercase tracking-wider">
        <BarChart3 size={14} />
        {clicks !== null ? `${clicks} Clicks` : '...'}
      </div>
      <button
        onClick={copyToClipboard}
        className="p-2 hover:bg-indigo-100 rounded-lg transition-colors text-indigo-600 flex-shrink-0"
        title="Copy to clipboard"
      >
        {copied ? <Check size={20} /> : <Copy size={20} />}
      </button>
    </div>
  );
};

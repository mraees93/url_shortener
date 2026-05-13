import { useEffect, useState } from 'react';
import { Copy, Check, BarChart3 } from 'lucide-react';
import { urlApi } from '../api/urlApi';

interface ResultCardProps {
  id: string;
  shortUrl: string;
  onDelete: (id: string) => void;
}

export const ResultCard = ({ id, shortUrl, onDelete }: ResultCardProps) => {
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

  const handleDeleteUrl = async () => {
    if (!confirm("Are you sure you want to delete this link?")) return;

    try {
      // 1. Call the API file
      await urlApi.deleteUrlByID(id);

      // 2. Notify parent component to remove it from the list immediately
      onDelete(id);
    } catch (error) {
      console.error("Delete failed:", error);
      alert("Failed to delete the link. Please try again.");
    }
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
      <button
        onClick={handleDeleteUrl}
        className="p-2 text-red-500 border border-transparent hover:bg-red-500/10 hover:border-red-500/20 rounded-lg transition-all duration-200"
        title="Delete Link"
        aria-label="Delete link from history"
      >
        <svg
          xmlns="w3.org"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="w-5 h-5"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="m14.74 9-.34 6m-4.74 0L9.34 9m4.74-0.342.348-10.037A.75.75 0 0 0 14.12 3H9.88a.75.75 0 0 0-.668.406L8.835 4.5H4.75a.75.75 0 0 0 0 1.5h14.5a.75.75 0 0 0 0-1.5h-4.085l-.317-1.094A.75.75 0 0 0 14.12 3ZM4.5 6.75h15v12.75a2.25 2.25 0 0 1-2.25 2.25H6.75A2.25 2.25 0 0 1 4.5 19.5V6.75Z"
          />
        </svg>
      </button>


    </div>
  );
};

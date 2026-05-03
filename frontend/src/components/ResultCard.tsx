import { useState } from 'react';
import { Copy, Check } from 'lucide-react';

interface ResultCardProps {
  shortUrl: string;
}

export const ResultCard = ({ shortUrl }: ResultCardProps) => {
  const [copied, setCopied] = useState(false);

  const copyToClipboard = () => {
    navigator.clipboard.writeText(shortUrl);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="animate-in fade-in slide-in-from-top-4 duration-500 bg-indigo-50 border border-indigo-100 p-4 rounded-xl flex items-center justify-between">
      <span className="text-indigo-700 font-medium truncate mr-4">
        {shortUrl}
      </span>
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

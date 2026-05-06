import { ResultCard } from './ResultCard';
import type { ShortenedLink } from '../api/urlApi';

interface HistoryListProps {
  links: ShortenedLink[];
}

export const HistoryList = ({ links }: HistoryListProps) => {
  if (links.length === 0) return null;

  return (
    <div className="w-full space-y-4 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <h3 className="text-sm font-semibold text-slate-400 uppercase tracking-wider pl-1">
        System Activity Log
      </h3>
      <div className="space-y-3">
        {links.map((link) => (
          <ResultCard
            key={link.shortCode}
            shortUrl={link.shortUrl}
          />
        ))}
      </div>
    </div>
  );
};

import { ResultCard } from './ResultCard';
import type { ShortenedLink } from '../api/urlApi';

interface HistoryListProps {
  links: ShortenedLink[];
  setHistory: React.Dispatch<React.SetStateAction<ShortenedLink[]>>;
}

export const HistoryList = ({ links, setHistory }: HistoryListProps) => {
  if (links.length === 0) return null;

  const handleCardDelete = (idToRemove: string) => {
    setHistory((prev) => prev.filter((link) => link.id !== idToRemove));
  };

  return (
    <div className="w-full space-y-4 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <h3 className="text-sm font-semibold text-slate-400 uppercase tracking-wider pl-1">
        System Activity Log
      </h3>
      <div className="space-y-3">
        {links.map((link) => (
          <ResultCard
            key={link.shortCode}
            id={link.id}
            shortUrl={link.shortUrl}
            onDelete={handleCardDelete}
          />
        ))}
      </div>
    </div>
  );
};

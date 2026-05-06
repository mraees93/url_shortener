interface ServerStatusProps {
  isStarting: boolean;
}

export const ServerStatus = ({ isStarting }: ServerStatusProps) => {
  if (!isStarting) return null;

  return (
    <div className="bg-indigo-50 border border-indigo-100 text-indigo-700 px-4 py-3 rounded-xl flex items-center gap-3 animate-pulse shadow-sm">
      <div className="relative flex h-2 w-2">
        <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-indigo-400 opacity-75"></span>
        <span className="relative inline-flex rounded-full h-2 w-2 bg-indigo-600"></span>
      </div>
      <p className="text-sm font-medium">
        Vertex engine is warming up... This may take a minute on the free tier.
      </p>
    </div>
  );
};

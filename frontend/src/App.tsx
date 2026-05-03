import { useState } from 'react';
import { Link2 } from 'lucide-react';
import { UrlInput } from './components/UrlInput';
import { ResultCard } from './components/ResultCard';

function App() {
  const [shortUrl, setShortUrl] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleShorten = async () => { //removed url param
    setIsLoading(true);
    
    // Mocking the API call for now
    setTimeout(() => {
      setShortUrl(`localhost:5000/Ab123`);
      setIsLoading(false);
    }, 800);
  };

  return (
    <div className="min-h-screen bg-slate-50 flex flex-col items-center justify-center p-6">
      <div className="max-w-xl w-full space-y-8 text-center">
        
        <header className="space-y-2">
          <div className="flex justify-center">
             <div className="bg-indigo-600 p-3 rounded-2xl shadow-lg shadow-indigo-200">
               <Link2 className="text-white w-8 h-8" />
             </div>
          </div>
          <h1 className="text-4xl font-extrabold text-slate-900 tracking-tight">
            Link Shrinker
          </h1>
          <p className="text-slate-500 text-lg">
            Create professional, short links in seconds.
          </p>
        </header>

        <main className="space-y-6">
          <UrlInput onShorten={handleShorten} isLoading={isLoading} />
          {shortUrl && <ResultCard shortUrl={shortUrl} />}
        </main>

      </div>
    </div>
  );
}

export default App;

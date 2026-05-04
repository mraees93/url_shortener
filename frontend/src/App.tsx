import { useHistory } from './hooks/useHistory';
import { UrlInput } from './components/UrlInput';
import { HistoryList } from './components/HistoryList';
import { Link2 } from 'lucide-react';

function App() {
  const { history, isLoading, shortenUrl } = useHistory();
  const BASE_URL = 'http://localhost:5219';

  const handleShorten = async (url: string) => {
    try {
      await shortenUrl(url);
    } catch (error: unknown) {
      if (error instanceof Error) {
        alert('Error: ' + error.message);
      }
    }
  };

  return (
    <div className="min-h-screen bg-slate-50 flex flex-col items-center py-20 px-6">
      <div className="max-w-xl w-full space-y-10">
        <header className="text-center space-y-2">
          <div className="flex justify-center mb-4">
            <div className="bg-indigo-600 p-3 rounded-2xl shadow-lg shadow-indigo-200">
              <Link2 className="text-white w-8 h-8" />
            </div>
          </div>
          <h1 className="text-4xl font-extrabold text-slate-900">Link Shrinker</h1>
          <p className="text-slate-500">Manage and track your short links</p>
        </header>

        <main className="space-y-8">
          <UrlInput onShorten={handleShorten} isLoading={isLoading} />
          <HistoryList links={history} baseUrl={BASE_URL} />
        </main>
      </div>
    </div>
  );
}

export default App;

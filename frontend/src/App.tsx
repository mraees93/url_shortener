import { useHistory } from './hooks/useHistory';
import { ServerStatus } from './components/ServerStatus';
import { UrlInput } from './components/UrlInput';
import { HistoryList } from './components/HistoryList';
import { Share2 } from 'lucide-react';

function App() {
  const { history, isLoading, shortenUrl, isServerStarting } = useHistory();
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

        <ServerStatus isStarting={isServerStarting} />

        <header className="text-center space-y-2">
          <div className="flex justify-center mb-4">
            <div className="bg-slate-900 p-3 rounded-2xl shadow-lg shadow-indigo-200">
              <Share2 className="text-indigo-400" />
            </div>
          </div>
          <h1 className="text-4xl font-extrabold text-slate-900">Vertex</h1>
          <p className="text-slate-500">Scalable Microservice Architecture for URL Redirection & Analytics</p>
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

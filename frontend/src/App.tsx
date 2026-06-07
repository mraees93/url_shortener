import { NotificationModal } from './components/NotificationModal';
import { useHistory } from './hooks/useHistory';
import { ServerStatus } from './components/ServerStatus';
import { UrlInput } from './components/UrlInput';
import { HistoryList } from './components/HistoryList';
import { Share2 } from 'lucide-react';
import { useState } from 'react';
import { urlApi } from './api/urlApi'; 

function App() {
  const { history, isLoading, shortenUrl, isServerStarting, setHistory } = useHistory();
  const [modalConfig, setModalConfig] = useState<{
    isOpen: boolean;
    title: string;
    message: string;
    variant: 'danger' | 'warning' | 'info';
    confirmText?: string;
    onConfirm?: () => void;
  }>({
    isOpen: false,
    title: '',
    message: '',
    variant: 'info',
  });

  const handleShorten = async (url: string) => {
  try {
    await shortenUrl(url);
  } catch (error: unknown) {
    if (error instanceof Error) {
      
      let displayMessage = error.message;
      try {
        const parsed = JSON.parse(error.message);
        if (parsed.message) displayMessage = parsed.message;
      } catch {
        // Fallback to raw message if it isn't JSON text
      }

      setModalConfig({
        isOpen: true,
        title: "Security Firewall Triggered",
        message: displayMessage, 
        variant: 'danger'
      });
    }
  }
};


  const executeActualDelete = async (id: string) => {
    try {
      await urlApi.deleteUrlByID(id);
      
      setHistory((prevHistory) => prevHistory.filter((item) => item.id !== id));
    } catch (error: unknown) {
      if (error instanceof Error) {
        setModalConfig({
          isOpen: true,
          title: "Deletion Failed",
          message: "Failed to delete the link. Please check your cloud network configuration.",
          variant: 'danger'
        });
      }
    }
  };

  const handleDeleteClick = (id: string) => {
    setModalConfig({
      isOpen: true,
      title: "Confirm Permanent Deletion",
      message: "Are you sure you want to delete this short code link? This structural record configuration cannot be restored.",
      variant: 'warning',
      confirmText: "Delete Link",
      onConfirm: () => executeActualDelete(id)
    });
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
          
          <HistoryList 
            links={history} 
            onDeleteClick={handleDeleteClick} 
          />
        </main>
      </div>
      <NotificationModal
        isOpen={modalConfig.isOpen}
        title={modalConfig.title}
        message={modalConfig.message}
        variant={modalConfig.variant}
        confirmText={modalConfig.confirmText}
        onConfirm={modalConfig.onConfirm}
        onClose={() => setModalConfig((prev) => ({ ...prev, isOpen: false }))}
      />
    </div>
  );
}

export default App;

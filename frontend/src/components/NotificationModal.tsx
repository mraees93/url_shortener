import React from 'react';

interface NotificationModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  message: string;
  variant: 'danger' | 'warning' | 'info';
  confirmText?: string;
  onConfirm?: () => void;
}

export const NotificationModal: React.FC<NotificationModalProps> = ({
  isOpen,
  onClose,
  title,
  message,
  variant,
  confirmText,
  onConfirm,
}) => {
  if (!isOpen) return null;

  // Dynamically swap theme styles based on the type of alert
  const variantStyles = {
    danger: {
      bg: 'bg-red-50 text-red-800 border-red-200',
      btn: 'bg-red-600 hover:bg-red-700 focus:ring-red-500',
      iconColor: 'text-red-600',
      iconPath: (
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
      ),
    },
    warning: {
      bg: 'bg-amber-50 text-amber-800 border-amber-200',
      btn: 'bg-amber-600 hover:bg-amber-700 focus:ring-amber-500',
      iconColor: 'text-amber-600',
      iconPath: (
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
      ),
    },
    info: {
      bg: 'bg-blue-50 text-blue-800 border-blue-200',
      btn: 'bg-blue-600 hover:bg-blue-700 focus:ring-blue-500',
      iconColor: 'text-blue-600',
      iconPath: (
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      ),
    },
  };

  const currentStyle = variantStyles[variant];

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm animate-fade-in">
      <div className="w-full max-w-md overflow-hidden bg-white border rounded-20 shadow-xl border-neutral-100 transform transition-all animate-scale-up">
        
        {/* Banner/Header Zone */}
        <div className={`p-6 flex items-start gap-4 border-b ${currentStyle.bg}`}>
          <div className={`p-2 bg-white rounded-12 shadow-sm ${currentStyle.iconColor}`}>
            <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              {currentStyle.iconPath}
            </svg>
          </div>
          <div>
            <h3 className="text-18 font-bold leading-6">{title}</h3>
            <p className="mt-2 text-14 opacity-90 font-medium">{message}</p>
          </div>
        </div>

        {/* Action Button Panel */}
        <div className="px-6 py-4 bg-neutral-50 flex items-center justify-end gap-3">
          <button
            type="button"
            onClick={onClose}
            className="px-4 py-2 text-14 font-semibold text-neutral-600 bg-white border border-neutral-200 rounded-12 hover:bg-neutral-50 transition-colors"
          >
            {onConfirm ? 'Cancel' : 'Dismiss'}
          </button>
          
          {onConfirm && confirmText && (
            <button
              type="button"
              onClick={() => {
                onConfirm();
                onClose();
              }}
              className={`px-4 py-2 text-14 font-semibold text-white rounded-12 shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 transition-all ${currentStyle.btn}`}
            >
              {confirmText}
            </button>
          )}
        </div>
        
      </div>
    </div>
  );
};

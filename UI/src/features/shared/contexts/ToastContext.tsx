import { createContext, useCallback, useContext } from "react";
import { toast } from "sonner";

type ToastContextType = {
  showSuccess: (message: string, description?: string) => void;
  showError: (message: string, description?: string) => void;
  showInfo: (message: string, description?: string) => void;
  showWarning: (message: string, description?: string) => void;
};

const ToastContext = createContext<ToastContextType | undefined>(undefined);

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const showSuccess = useCallback((message: string, description?: string) => {
    toast.success(message, { 
      description
    });
  }, []);

  const showError = useCallback((message: string, description?: string) => {
    toast.error(message, { 
      description
    });
  }, []);

  const showInfo = useCallback((message: string, description?: string) => {
    toast.info(message, { 
      description
    });
  }, []);

  const showWarning = useCallback((message: string, description?: string) => {
    toast.warning(message, { 
      description
    });
  }, []);

  return (
    <ToastContext.Provider value={{ showSuccess, showError, showInfo, showWarning }}>
      {children}
    </ToastContext.Provider>
  );
}

export function useAppToast() {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error("useAppToast must be used within a ToastProvider");
  }
  return context;
}
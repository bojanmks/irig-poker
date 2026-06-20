import './index.css'
import "./lib/i18n.ts";
import App from './App.tsx'
import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, createRoutesFromElements, RouterProvider, Route } from 'react-router-dom';
import AppRoutes from './features/routing/components/AppRoutes.tsx';
import { ThemeProvider } from './features/themes/components/ThemeProvider.tsx';
import { ToastProvider } from './features/shared/contexts/ToastContext.tsx';
import { Toaster } from './features/shared/components/Sonner.tsx';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route
      path='/'
      element={
        <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
          <ToastProvider>
            <Toaster />
            <App />
          </ToastProvider>
        </ThemeProvider>
      }
    >
      {AppRoutes}
    </Route>
  )
);

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
import './index.css'
import "./lib/i18n.ts";

import React from 'react';
import { HelmetProvider } from 'react-helmet-async';
import { Provider } from 'react-redux';
import { createBrowserRouter, createRoutesFromElements, Route,RouterProvider } from 'react-router-dom';
import ReactDOM from 'react-dom/client';

import App from './App.tsx'
import AppRoutes from './features/routing/components/AppRoutes.tsx';
import { Toaster } from './features/shared/components/shadcn/Sonner.tsx';
import { store } from './features/store';
import { ThemeSync } from './features/store/ThemeSync';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route
      path='/'
      element={
        <Provider store={store}>
          <ThemeSync>
            <Toaster position='top-center' />
            <App />
          </ThemeSync>
        </Provider>
      }
    >
      {AppRoutes}
    </Route>
  )
);

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
  <React.StrictMode>
    <HelmetProvider>
      <RouterProvider router={router} />
    </HelmetProvider>
  </React.StrictMode>
);
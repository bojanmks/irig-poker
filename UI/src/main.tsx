import './index.css'
import "./lib/i18n.ts";
import App from './App.tsx'
import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, createRoutesFromElements, RouterProvider, Route } from 'react-router-dom';
import AppRoutes from './features/routing/components/AppRoutes.tsx';
import { Provider } from 'react-redux';
import { store } from './features/store';
import { ThemeSync } from './features/store/ThemeSync';
import { Toaster } from './features/shared/components/shadcn/Sonner.tsx';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route
      path='/'
      element={
        <Provider store={store}>
          <ThemeSync>
            <Toaster />
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
    <RouterProvider router={router} />
  </React.StrictMode>
);
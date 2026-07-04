import './App.scss'

import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Outlet } from 'react-router-dom'

function App() {
  const { t } = useTranslation();

  useEffect(() => {
    document.title = t("appTitle");
  }, [t]);

  return (
    <Outlet />
  )
}

export default App

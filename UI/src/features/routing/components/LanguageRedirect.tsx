import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

const LanguageRedirect = () => {
  const navigate = useNavigate();
  const { i18n } = useTranslation();

  useEffect(() => {
    navigate(`/${i18n.language}`, { replace: true });
  }, [navigate, i18n.language]);

  return null;
};

export default LanguageRedirect;

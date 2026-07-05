import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import i18n from "@/lib/i18n";

const LanguageRedirect = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const lang = i18n.language.startsWith("sr") ? "sr" : "en";
    navigate(`/${lang}`, { replace: true });
  }, [navigate]);

  return null;
};

export default LanguageRedirect;

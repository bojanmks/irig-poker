import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import SeoHead from "@/features/seo/components/SeoHead";
import { buttonVariants } from "@/features/shared/components/shadcn/Button";

const NotFoundPage = () => {
  const { i18n } = useTranslation();
  const navigate = useNavigate();
  const lang = i18n.language.startsWith("sr") ? "sr" : "en";

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] gap-4">
      <SeoHead titleKey="notFoundTitle" descriptionKey="notFoundDescription" />
      <h1 className="text-4xl font-bold">404</h1>
      <p className="text-muted-foreground text-lg">
        {i18n.language.startsWith("sr") ? "Stranica nije pronađena" : "Page not found"}
      </p>
      <button
        className={buttonVariants({ variant: "default" })}
        onClick={() => navigate(`/${lang}`)}
      >
        {i18n.language.startsWith("sr") ? "Početna" : "Go home"}
      </button>
    </div>
  );
};

export default NotFoundPage;

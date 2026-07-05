import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

import SeoHead from "@/features/seo/components/SeoHead";
import { buttonVariants } from "@/features/shared/components/shadcn/Button";

const NotFoundPage = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] gap-4">
      <SeoHead titleKey="meta.notFoundTitle" descriptionKey="meta.notFoundDescription" />
      <h1 className="text-4xl font-bold">404</h1>
      <p className="text-muted-foreground text-lg">
        { t("common.pageNotFound") }
      </p>
      <button
        className={buttonVariants({ variant: "default" })}
        onClick={() => navigate("/")}
      >
        { t("common.goHome") }
      </button>
    </div>
  );
};

export default NotFoundPage;

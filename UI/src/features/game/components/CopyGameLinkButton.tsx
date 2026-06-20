import { Button } from "@/features/shared/components/Button";
import { showSuccess } from "@/features/shared/utils/toast";
import { CopyIcon } from "lucide-react";
import { useState } from "react";
import { useTranslation } from "react-i18next";

export const CopyGameLinkButton = () => {
  const [isHovered, setIsHovered] = useState(false);
  const { t } = useTranslation();

  const gameUrl = window.location.href;

  const handleCopy = () => {
    navigator.clipboard.writeText(gameUrl);
    showSuccess(t("game.linkCopied"));
  };

  return (
    <Button
      variant="outline"
      onClick={handleCopy}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      className="w-full flex items-center justify-center gap-2 h-10"
    >
      {isHovered ? (
        <>
          <span className="truncate">{gameUrl}</span>
          <CopyIcon className="w-4 h-4 flex-shrink-0" />
        </>
      ) : (
        <>
          <span>{t("game.copyLink")}</span>
          <CopyIcon className="w-4 h-4" />
        </>
      )}
    </Button>
  )
}
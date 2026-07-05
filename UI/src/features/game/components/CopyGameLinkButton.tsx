import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";

import { CopyIcon } from "lucide-react";

import { Button } from "@/features/shared/components/shadcn/Button";
import { showSuccess } from "@/features/shared/utils/toast";

export const CopyGameLinkButton = () => {
  const [isHovered, setIsHovered] = useState(false);
  const { t } = useTranslation();
  const { gameCode } = useParams();

  const url = new URL(window.location.href);
  const gameUrl = gameCode ? `${url.origin}/${gameCode}` : url.origin;

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
          <CopyIcon className="w-4 h-4 shrink-0" />
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
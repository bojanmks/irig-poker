import { useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";

import { CopyIcon } from "lucide-react";

import type { Language } from "@/features/localization/types/Language";
import { Button } from "@/features/shared/components/shadcn/Button";
import { showSuccess } from "@/features/shared/utils/toast";

export const CopyGameLinkButton = () => {
  const [isHovered, setIsHovered] = useState(false);
  const { t } = useTranslation();
  const { gameCode } = useParams();

  const displayGameUrl = useMemo(() => {
    const url = new URL(window.location.href);
    return gameCode ? `${url.origin}/${gameCode}` : url.origin;
  }, [gameCode]);

  const copyGameUrl = useMemo(() => {
    const url = new URL(window.location.href);
    return gameCode ? `${url.origin}/${"unknown" satisfies Language}/${gameCode}` : url.origin;
  }, []);

  const handleCopy = useCallback(() => {
    navigator.clipboard.writeText(copyGameUrl);
    showSuccess(t("game.linkCopied"));
  }, [copyGameUrl]);

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
          <span className="truncate">{displayGameUrl}</span>
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
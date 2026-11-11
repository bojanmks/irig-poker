import { useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";

import { CopyIcon } from "lucide-react";

import { QueryParam } from "@/features/routing/consts/QueryParam";
import { Button } from "@/features/shared/components/shadcn/Button";
import { showSuccess } from "@/features/shared/utils/toast";

export const CopyGameLinkButton = () => {
  const [isHovered, setIsHovered] = useState(false);
  const { t } = useTranslation();

  const inviteUrl = useMemo(() => {
    const url = new URL(window.location.href);
    // Assigning a raw string keeps the flag bare ("?join") instead of "?join="
    url.search = QueryParam.Join;
    url.hash = "";
    return url.toString();
  }, []);

  const handleCopy = useCallback(() => {
    navigator.clipboard.writeText(inviteUrl);
    showSuccess(t("game.linkCopied"));
  }, [inviteUrl, t]);

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
          <span className="truncate">{inviteUrl}</span>
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
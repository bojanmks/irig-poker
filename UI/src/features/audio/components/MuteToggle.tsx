import { useTranslation } from "react-i18next";

import { Volume2, VolumeX } from "lucide-react";

import { Button } from "@/features/shared/components/shadcn/Button";
import { useAppDispatch, useAppSelector } from "@/features/store/hooks";

import { toggleMuted } from "../store/audioSlice";

const MuteToggle = () => {
  const { t } = useTranslation();
  const muted = useAppSelector((state) => state.audio.muted);
  const dispatch = useAppDispatch();

  return (
    <Button
      variant="outline"
      size="icon"
      onClick={() => dispatch(toggleMuted())}
      aria-label={muted ? t("audio.unmute") : t("audio.mute")}
    >
      {muted ? <VolumeX size={16} /> : <Volume2 size={16} />}
    </Button>
  );
};

export default MuteToggle;

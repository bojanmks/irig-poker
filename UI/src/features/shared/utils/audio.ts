function isMuted(): boolean {
  return localStorage.getItem("audio-muted") === "true";
}

export function playSound(src: string) {
  if (isMuted()) return;

  try {
    const audio = new Audio(src);
    audio.volume = 0.3;
    audio.play();
  } catch {
    // Ignore audio playback errors
  }
}

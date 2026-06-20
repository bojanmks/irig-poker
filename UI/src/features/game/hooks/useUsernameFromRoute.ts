import { useLocation } from "react-router-dom";
import { useState } from "react";

export function useUsernameFromRoute() {
  const location = useLocation();
  const [username, setUsername] = useState<string | null>(location.state?.username ?? null);

  return { username, setUsername, initialized: true };
}

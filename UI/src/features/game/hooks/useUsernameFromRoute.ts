import { useState } from "react";
import { useLocation } from "react-router-dom";

export function useUsernameFromRoute() {
  const location = useLocation();
  const [username, setUsername] = useState<string | null>(location.state?.username ?? null);

  return { username, setUsername, initialized: true };
}
